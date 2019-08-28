/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.Artifacts;
using Dolittle.Collections;
using Dolittle.Runtime.Events.EventStore;
using Dolittle.Serialization.Json;
using EventStore.ClientAPI;

namespace Dolittle.Runtime.Events.Store.EventStore
{
    /// <summary>
    /// EventStore implementation of <see cref="IEventStore"/>
    /// </summary>
    public class EventStore : IEventStore
    {
        readonly IEventStoreConnection _connection;
        readonly ISerializer _serializer;
        readonly IArtifactTypeMap _artifactTypeMap;
        readonly string _streamPrefix;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="serializer"></param>
        /// <param name="artifactTypeMap"></param>
        public EventStore(EventStoreConnector connector, ISerializer serializer, IArtifactTypeMap artifactTypeMap)
        {
            _streamPrefix = $"{connector.Instance}";
            _connection = connector.Connection;
            _serializer = serializer;
            _artifactTypeMap = artifactTypeMap;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // TODO: Should we do anything here?
        }

        void ThrowIfStreamWasDeleted(string stream, EventReadResult result)
        {
            if (result.Status == EventReadStatus.StreamDeleted)
            {
                throw new CorruptedEventStore($"Stream '{stream}' was deleted");
            }
        }



        StreamVersionedValue<CommitSequenceNumber> GetNextCommitSequenceNumber()
        {
            var result = _connection.ReadEventAsync(GetStreamForCommit(), StreamPosition.End, true).Result;
            ThrowIfStreamWasDeleted(GetStreamForCommit(), result);
            if (result.Status == EventReadStatus.NoStream)
            {
                return new StreamVersionedValue<CommitSequenceNumber>(GetStreamForCommit(), ExpectedVersion.NoStream, 0);
            }
            try
            {
                var metadata = _serializer.FromJsonBytes<EventMetadata>(result.Event.Value.Event.Metadata);
                return new StreamVersionedValue<CommitSequenceNumber>(GetStreamForCommit(), result.Event.Value.Event.EventNumber, metadata.Commit.Sequence+1);
            }
            catch (Exception ex)
            {
                throw new CorruptedEventStore("Could not deserialize event metadata", ex);
            }
        }

        EventSourceVersion GetLastEventSourceVersion(EventSourceKey eventSourceKey)
        {
            var commits = Fetch(eventSourceKey);
            if (commits.IsEmpty)
            {
                return EventSourceVersion.NoVersion;
            }
            return commits.Last().LastEventVersion.ToEventSourceVersion();
        }

        IEnumerable<EventData> CreateEventsFromUncomitted(UncommittedEventStream uncommittedEvents, CommitMetadata commitMetadata)
        {
            var events = new List<EventData>();
            foreach (var @event in uncommittedEvents.Events)
            {
                events.Add(new EventData(
                    @event.Id,
                    _artifactTypeMap.GetTypeFor(@event.Metadata.Artifact).Name,
                    true,
                    _serializer.ToJsonBytes(@event.Event),
                    _serializer.ToJsonBytes(new EventMetadata(@event.Metadata, commitMetadata))
                ));
            }
            return events;
        }



         /// <inheritdoc />
        public CommittedEventStream Commit(UncommittedEventStream uncommittedEvents)
        {
            while (true)
            {
                var nextSequenceNumber = GetNextCommitSequenceNumber();

                var lastEventSourceVersion = GetLastEventSourceVersion(uncommittedEvents.Source.Key);
                
                if (uncommittedEvents.Source.Version.CompareTo(lastEventSourceVersion) <= 0)
                {
                    throw new EventSourceConcurrencyConflict($"Current Version is {lastEventSourceVersion}, tried to commit {uncommittedEvents.Source.Version}");
                }

                var commitMetadata = new CommitMetadata(
                    nextSequenceNumber.Value,
                    uncommittedEvents.Source,
                    uncommittedEvents.Id,
                    DateTimeOffset.UtcNow,
                    uncommittedEvents.CorrelationId
                );

                _connection.AppendToStreamAsync(
                    GetStreamForCommit(),
                    nextSequenceNumber.Version,
                    CreateEventsFromUncomitted(uncommittedEvents, commitMetadata)
                ).Wait();

                return new CommittedEventStream(
                    nextSequenceNumber.Value,
                    uncommittedEvents.Source,
                    uncommittedEvents.Id,
                    uncommittedEvents.CorrelationId,
                    uncommittedEvents.Timestamp,
                    uncommittedEvents.Events
                );
            }
        }



        

        string GetStreamForCommit()
        {
            return $"{_streamPrefix}-events";
        }

        IEnumerable<ResolvedEvent> FetchAllEvents()
        {
            var events = new List<ResolvedEvent>();
            long nextReadPosition = StreamPosition.Start;
            while (true)
            {
                var slice = _connection.ReadStreamEventsForwardAsync(GetStreamForCommit(), nextReadPosition, 100, true).Result;
                events.AddRange(slice.Events);
                if (slice.IsEndOfStream)
                {
                    break;
                }
                nextReadPosition = slice.NextEventNumber;
            }
            return events;
        }

        Commits FetchAllCommits()
        {
            var commits = new List<CommittedEventStream>();
            var events = FetchAllEvents();
            events.GroupBy(_ => _serializer.FromJsonBytes<EventMetadata>(_.Event.Metadata).Commit.Id).ForEach(commitGroup => {
                CommitMetadata commitMetadata = null;
                var commitEvents = new List<EventEnvelope>();
                foreach (var @event in commitGroup)
                {
                    var data = _serializer.PropertyBagFromJsonBytes(@event.Event.Data);
                    var eventMetadata = _serializer.FromJsonBytes<EventMetadata>(@event.Event.Metadata);
                    commitMetadata = eventMetadata.Commit;
                    commitEvents.Add(new EventEnvelope(eventMetadata.Event, data));
                }
                commits.Add(new CommittedEventStream(
                    commitMetadata.Sequence,
                    commitMetadata.Source,
                    commitMetadata.Id,
                    commitMetadata.CorrelationId,
                    commitMetadata.Timestamp,
                    new EventStream(commitEvents)
                ));
            });
            return new Commits(commits);
        }


        /// <inheritdoc />
        public Commits FetchAllCommitsAfter(CommitSequenceNumber commit)
        {
            return new Commits(FetchAllCommits().Where(_ => _.Sequence > commit));
        }

        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfType(ArtifactId eventType)
        {
            return new SingleEventTypeEventStream(FetchAllEvents().Where(_ => _serializer.FromJsonBytes<EventMetadata>(_.Event.Metadata).Event.Artifact.Id.Equals(eventType)).Select(_ => {
                var data = _serializer.PropertyBagFromJsonBytes(_.Event.Data);
                var metadata = _serializer.FromJsonBytes<EventMetadata>(_.Event.Metadata);
                return new CommittedEventEnvelope(
                    metadata.Commit.Sequence,
                    metadata.Event,
                    data
                );
            }));
        }

        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfTypeAfter(ArtifactId eventType, CommitSequenceNumber commit)
        {
            return new SingleEventTypeEventStream(FetchAllEventsOfType(eventType).Where(_ => _.Version.Major >= commit));
        }

        /// <inheritdoc />
        public Commits FetchFrom(EventSourceKey eventSourceKey, CommitVersion commitVersion)
        {
            return new Commits(FetchAllCommits().Where(_ => _.Source.Version.Commit >= commitVersion && _.Source.Artifact.Equals(eventSourceKey.Artifact) && _.Source.EventSource.Equals(eventSourceKey.Id)));
        }

        /// <inheritdoc />
        public Commits Fetch(EventSourceKey eventSourceKey)
        {
            return new Commits(FetchAllCommits().Where(_ => _.Source.Artifact.Equals(eventSourceKey.Artifact) && _.Source.EventSource.Equals(eventSourceKey.Id)));
        }
        

        /// <inheritdoc />
        public EventSourceVersion GetCurrentVersionFor(EventSourceKey eventSource)
        {
            var commits = Fetch(eventSource);
            if (commits.IsEmpty)
            {
                return EventSourceVersion.NoVersion;
            }
            return commits.Last().LastEventVersion.ToEventSourceVersion();
        }

        /// <inheritdoc />
        public EventSourceVersion GetNextVersionFor(EventSourceKey eventSource)
        {
            return GetCurrentVersionFor(eventSource).NextCommit();
        }
    }
}
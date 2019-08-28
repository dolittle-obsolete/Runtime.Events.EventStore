/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.Artifacts;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="serializer"></param>
        public EventStore(EventStoreConnector connector, ISerializer serializer)
        {
            _connection = connector.Connection;
            _serializer = serializer;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // TODO: Should we do anything here?
        }

        CommitEvent GetLastCommitInStore()
        {
            var result = _connection.ReadEventAsync(GetStreamForCommit(), StreamPosition.End, true).Result;
            ThrowIfStreamWasDeleted(GetStreamForCommit(), result);
            
            if (result.Status == EventReadStatus.NoStream)
            {
                return CommitEvent.None;
            }
            else
            {
                ThrowIfIsNotCommitEvent(result);
                return _serializer.FromJsonBytes<CommitEvent>(result.Event.Value.Event.Data);
            }
        }

        void ThrowIfStreamWasDeleted(string stream, EventReadResult result)
        {
            if (result.Status == EventReadStatus.StreamDeleted)
            {
                throw new CorruptedEventStore($"Stream '{stream}' was deleted");
            }
        }

        void ThrowIfIsNotCommitEvent(EventReadResult result)
        {
            if (!result.Event.HasValue || result.Event.Value.Event.EventType != CommitEvent.EventStoreType)
            {
                throw new CorruptedEventStore($"Last event in stream '{GetStreamForCommit()}' was not of type a CommitEvent - expected '{CommitEvent.EventStoreType}', got '{result.Event.Value.Event.EventType}'.");
            }
        }

        IReadOnlyList<EventData> CreateEventsFromUncomitted(UncommittedEventStream uncommittedEvents, CommitEvent expectedCommit)
        {
            var events = new List<EventData>();
            foreach (var @event in uncommittedEvents.Events)
            {
                events.Add(new EventData(
                    @event.Id,
                    @event.Metadata.Artifact.ToString(),
                    true,
                    _serializer.ToJsonBytes(@event.Event),
                    _serializer.ToJsonBytes(@event.Metadata)
                ));
            }
            events.Add(new EventData(
                expectedCommit.Id,
                CommitEvent.EventStoreType,
                true,
                _serializer.ToJsonBytes(expectedCommit),
                Array.Empty<byte>()
            ));
            return events;
        }

        string GetStreamForCommit()
        {
            return "commits";
        }

        /// <inheritdoc />
        public CommittedEventStream Commit(UncommittedEventStream uncommittedEvents)
        {
            // Get last version of eventsource
            

            // Prepare the next commit
            var previous = GetLastCommitInStore();

            var next = previous.NextCommit(uncommittedEvents);

            // Try to append it
            _connection.AppendToStreamAsync(
                GetStreamForCommit(),
                previous.Version,
                CreateEventsFromUncomitted(uncommittedEvents, next)
            ).Wait();

            // Return that it worked
            return new CommittedEventStream(
                next.Sequence,
                uncommittedEvents.Source,
                uncommittedEvents.Id,
                uncommittedEvents.CorrelationId,
                uncommittedEvents.Timestamp,
                uncommittedEvents.Events
            );
        }

        /// <inheritdoc />
        public Commits FetchAllCommitsAfter(CommitSequenceNumber commit)
        {
            // TODO: Do something cool
            return new Commits(Enumerable.Empty<CommittedEventStream>());
        }

        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfType(ArtifactId eventType)
        {
            // TODO: Do something cool
            return new SingleEventTypeEventStream(Enumerable.Empty<CommittedEventEnvelope>());
        }

        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfTypeAfter(ArtifactId eventType, CommitSequenceNumber commit)
        {
            // TODO: Do something cool
            return new SingleEventTypeEventStream(Enumerable.Empty<CommittedEventEnvelope>());
        }

        /// <inheritdoc />
        public Commits FetchFrom(EventSourceKey eventSourceKey, CommitVersion commitVersion)
        {
            // TODO: Do something cool
            return new Commits(Enumerable.Empty<CommittedEventStream>());
        }

        /// <inheritdoc />
        public Commits Fetch(EventSourceKey eventSourceKey)
        {
            // TODO: Do something cool
            return new Commits(Enumerable.Empty<CommittedEventStream>());
        }
        

        /// <inheritdoc />
        public EventSourceVersion GetCurrentVersionFor(EventSourceKey eventSource)
        {
            return EventSourceVersion.NoVersion;
        }

        /// <inheritdoc />
        public EventSourceVersion GetNextVersionFor(EventSourceKey eventSource)
        {
            return GetCurrentVersionFor(eventSource).NextCommit();
        }
    }
}
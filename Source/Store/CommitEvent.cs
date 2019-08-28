/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System;
using Dolittle.Artifacts;
using Dolittle.Execution;
using Dolittle.Runtime.Events.EventStore;
using EventStore.ClientAPI;

namespace Dolittle.Runtime.Events.Store.EventStore
{
    /// <summary>
    /// 
    /// </summary>
    public class CommitEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sequence"></param>
        /// <param name="source"></param>
        /// <param name="version"></param>
        /// <param name="firstEvent"></param>
        /// <param name="lastEvent"></param>
        /// <param name="timestamp"></param>
        /// <param name="correlationId"></param>
        public CommitEvent(CommitId id, CommitSequenceNumber sequence, VersionedEventSource source, StreamVersion version, StreamVersion firstEvent, StreamVersion lastEvent, DateTimeOffset timestamp, CorrelationId correlationId)
        {
            Ensure.IsNotNull(nameof(id), id);
            Ensure.IsNotNull(nameof(sequence), sequence);
            Ensure.IsNotNull(nameof(source), source);
            Ensure.IsNotNull(nameof(version), version);
            Ensure.IsNotNull(nameof(firstEvent), firstEvent);
            Ensure.IsNotNull(nameof(lastEvent), lastEvent);

            Id = id;
            Sequence = sequence;
            Source = source;
            Version = version;
            FirstEvent = firstEvent;
            LastEvent = lastEvent;
            Timestamp = timestamp;
            CorrelationId = correlationId;
        }


        /// <summary>
        /// The unique id in the form of a <see cref="CommitId" />
        /// </summary>
        public CommitId Id { get; }
        /// <summary>
        /// The <see cref="CommitSequenceNumber" /> for this committed event stream
        /// </summary>
        public CommitSequenceNumber Sequence { get; }
        /// <summary>
        /// The <see cref="VersionedEventSource" /> for this committed event stream
        /// </summary>
        public VersionedEventSource Source { get; }
        /// <summary>
        /// The <see cref="StreamVersion" /> of the commit itself
        /// </summary>
        public StreamVersion Version { get; }
        /// <summary>
        /// The <see cref="StreamVersion" /> of the first event committed in this commit
        /// </summary>
        public StreamVersion FirstEvent { get; }
        /// <summary>
        /// The <see cref="StreamVersion" /> of the last event committed in this commit
        /// </summary>
        public StreamVersion LastEvent { get; }
        /// <summary>
        /// A timestamp in the form of a <see cref="DateTimeOffset" /> representing when the stream was committed
        /// </summary>
        public DateTimeOffset Timestamp { get; }
        /// <summary>
        /// A <see cref="CorrelationId" /> used to relate this event stream to other actions in the system
        /// </summary>
        public CorrelationId CorrelationId { get; }



        static CommitEvent _none = new CommitEvent
        (
            CommitId.Empty,
            0,
            new VersionedEventSource(EventSourceVersion.NoVersion, new EventSourceKey(EventSourceId.Empty, Guid.Empty)),
            ExpectedVersion.NoStream,
            -1,
            -1,
            DateTimeOffset.MinValue,
            CorrelationId.Empty
        );

        /// <summary>
        /// 
        /// </summary>
        public static CommitEvent None { get => _none; }

        /// <summary>
        /// 
        /// </summary>
        public const string EventStoreType = "dolittle-commit";
    }
}
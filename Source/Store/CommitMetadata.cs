/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System;
using Dolittle.Execution;

namespace Dolittle.Runtime.Events.Store.EventStore
{
    /// <summary>
    /// 
    /// </summary>
    public class CommitMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="source"></param>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <param name="correlationId"></param>
        public CommitMetadata(CommitSequenceNumber sequence, VersionedEventSource source, CommitId id, DateTimeOffset timestamp, CorrelationId correlationId)
        {
            Sequence = sequence;
            Source = source;
            Id = id;
            Timestamp = timestamp;
            CorrelationId = correlationId;
        }

        /// <summary>
        /// The <see cref="CommitSequenceNumber" /> for this committed event stream
        /// </summary>
        /// <value></value>
        public CommitSequenceNumber Sequence { get; }
        /// <summary>
        /// The <see cref="VersionedEventSource" /> for this committed event stream
        /// </summary>
        /// <value></value>
        public VersionedEventSource Source { get; }
        /// <summary>
        /// The unique id in the form of a <see cref="CommitId" />
        /// </summary>
        /// <value></value>
        public CommitId Id { get; }
        /// <summary>
        /// A timestamp in the form of a <see cref="DateTimeOffset" /> representing when the strwam was committed
        /// </summary>
        /// <value></value>
        public DateTimeOffset Timestamp { get; }
        /// <summary>
        /// A <see cref="CorrelationId" /> used to relate this event stream to other actions in the system
        /// </summary>
        /// <value></value>
        public CorrelationId CorrelationId { get; }
    }
}
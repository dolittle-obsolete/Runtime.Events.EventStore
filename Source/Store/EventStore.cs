/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using Dolittle.Artifacts;

namespace Dolittle.Runtime.Events.Store.EventStore
{
    /// <summary>
    /// EventStore implementation of <see cref="IEventStore"/>
    /// </summary>
    public class EventStore : IEventStore
    {

        /// <inheritdoc />
        public CommittedEventStream Commit(UncommittedEventStream uncommittedEvents)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Commits Fetch(EventSourceKey eventSourceKey)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Commits FetchAllCommitsAfter(CommitSequenceNumber commit)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfType(ArtifactId eventType)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfTypeAfter(ArtifactId eventType, CommitSequenceNumber commit)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Commits FetchFrom(EventSourceKey eventSourceKey, CommitVersion commitVersion)
        {
            throw new System.NotImplementedException();
        }
        
        /// <inheritdoc />
        public EventSourceVersion GetCurrentVersionFor(EventSourceKey eventSource)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public EventSourceVersion GetNextVersionFor(EventSourceKey eventSource)
        {
            throw new System.NotImplementedException();
        }
    }
}
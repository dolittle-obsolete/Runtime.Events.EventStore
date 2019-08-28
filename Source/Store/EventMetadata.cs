/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using Events = Dolittle.Events;

namespace Dolittle.Runtime.Events.Store.EventStore
{
    /// <summary>
    /// 
    /// </summary>
    public class EventMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        public EventMetadata(Events.EventMetadata @event, CommitMetadata commit)
        {
            Event = @event;
            Commit = commit;
        }

        /// <summary>
        /// 
        /// </summary>
        public Events.EventMetadata Event { get; }

        /// <summary>
        /// 
        /// </summary>
        public CommitMetadata Commit { get; }
    }
}
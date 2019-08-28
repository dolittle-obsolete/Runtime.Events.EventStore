/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System;
using System.Linq;

namespace Dolittle.Runtime.Events.Store.EventStore
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommitEventExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="last"></param>
        /// <param name="uncommittedEvents"></param>
        /// <returns></returns>
        public static CommitEvent NextCommit(this CommitEvent last, UncommittedEventStream uncommittedEvents)
        {
            return new CommitEvent
            (
                uncommittedEvents.Id,
                last != CommitEvent.None ? last.Sequence+1 : 0,
                uncommittedEvents.Source,
                last.Version+uncommittedEvents.Events.Count()+1,
                last.Version+1,
                last.Version+uncommittedEvents.Events.Count(),
                DateTimeOffset.UtcNow,
                uncommittedEvents.CorrelationId
            );
        }
    }
}
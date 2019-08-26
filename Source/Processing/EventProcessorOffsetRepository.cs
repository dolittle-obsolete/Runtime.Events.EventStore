/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using Dolittle.Runtime.Events.Store;

namespace Dolittle.Runtime.Events.Processing.EventStore
{
    /// <summary>
    /// EventStore implementation of <see cref="IEventProcessorOffsetRepository"/>
    /// </summary>
    public class EventProcessorOffsetRepository : IEventProcessorOffsetRepository
    {
        /// <inheritdoc />
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public CommittedEventVersion Get(EventProcessorId eventProcessorId)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void Set(EventProcessorId eventProcessorId, CommittedEventVersion committedEventVersion)
        {
            throw new System.NotImplementedException();
        }
    }
}
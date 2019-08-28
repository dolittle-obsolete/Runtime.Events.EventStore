/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/


using Dolittle.Runtime.Events.EventStore.Specs;

namespace Dolittle.Runtime.Events.Processing.EventStore.Specs
{
    public class EventProcessorOffsetRepository : Dolittle.Runtime.Events.Processing.EventStore.EventProcessorOffsetRepository
    {
        public EventProcessorOffsetRepository() : base(EventStoreConnector.Instance)
        {
        }
    }
}
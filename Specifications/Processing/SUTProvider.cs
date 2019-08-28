/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using Dolittle.Runtime.Events.Processing.InMemory.Specs;

namespace Dolittle.Runtime.Events.Processing.EventStore.Specs
{
    public class SUTProvider : IProvideTheOffsetRepository
    {
        public IEventProcessorOffsetRepository Build() => new EventProcessorOffsetRepository();
    }
}
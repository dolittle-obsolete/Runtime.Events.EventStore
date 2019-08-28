/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using Dolittle.Runtime.Events.Store.Specs;

namespace Dolittle.Runtime.Events.Store.EventStore.Specs
{
    public class SUTProvider : IProvideTheEventStore
    {
        public IEventStore Build() => new EventStore();
    }
}
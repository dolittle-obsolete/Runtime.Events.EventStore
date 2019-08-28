/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using Dolittle.Runtime.Events.EventStore.Specs;

namespace Dolittle.Runtime.Events.Store.EventStore.Specs
{
    public class EventStore : Dolittle.Runtime.Events.Store.EventStore.EventStore
    {
        public EventStore() : base(EventStoreConnector.Instance)
        {
        }
    }
}
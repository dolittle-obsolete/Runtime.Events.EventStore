/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using Dolittle.ResourceTypes;

namespace Dolittle.Runtime.Events.EventStore
{
    /// <inheritdoc/>
    public class EventStoreResourceTypeRepresentation : IRepresentAResourceType
    {
        static IDictionary<Type, Type> _bindings = new Dictionary<Type, Type>
        {
            {typeof(Dolittle.Runtime.Events.Store.IEventStore), typeof(Dolittle.Runtime.Events.Store.EventStore.EventStore)},
            {typeof(Dolittle.Runtime.Events.Relativity.IGeodesics), typeof(Dolittle.Runtime.Events.Relativity.EventStore.Geodesics)},
            {typeof(Dolittle.Runtime.Events.Processing.IEventProcessorOffsetRepository), typeof(Dolittle.Runtime.Events.Processing.EventStore.EventProcessorOffsetRepository)}
        };
        
        /// <inheritdoc/>
        public ResourceType Type => "eventStore";
        /// <inheritdoc/>
        public ResourceTypeImplementation ImplementationName => "EventStore";
        /// <inheritdoc/>
        public Type ConfigurationObjectType => typeof(EventStoreConfiguration);
        /// <inheritdoc/>
        public IDictionary<Type, Type> Bindings => _bindings;
    }
}
/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using Moq;

namespace Dolittle.Runtime.Events.EventStore.Specs
{
    public static class EventStoreConnector
    {
        static IEventStoreConnection CreateInMemoryEventStoreConnection()
        {
            var node = EmbeddedVNodeBuilder.AsSingleNode().OnDefaultEndpoints().RunInMemory().Build();
            node.StartAndWaitUntilReady().Wait();
            var connection = EmbeddedEventStoreConnection.Create(node);
            connection.ConnectAsync().Wait();
            return connection;
        }

        public static Dolittle.Runtime.Events.EventStore.EventStoreConnector Instance
        {
            get
            {
                var instance = new Mock<Dolittle.Runtime.Events.EventStore.EventStoreConnector>();
                instance.SetupGet(_ => _.Connection).Returns(() => CreateInMemoryEventStoreConnection());
                return instance.Object;
            }
        }
    }
}
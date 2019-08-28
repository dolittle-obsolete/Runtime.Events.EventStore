/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;

namespace Dolittle.Runtime.Events.EventStore.Specs
{
    public class an_evenstore_connection : IDisposable
    {
        readonly ClusterVNode _node;

        public an_evenstore_connection()
        {
            _node = EmbeddedVNodeBuilder.AsSingleNode().OnDefaultEndpoints().RunInMemory().Build();
            _node.StartAndWaitUntilReady().Wait();

            Connection = EmbeddedEventStoreConnection.Create(_node);
            Connection.ConnectAsync().Wait();
        }

        public IEventStoreConnection Connection { get; }

        public void Dispose()
        {
            Connection.Dispose();
            _node.Stop();
        }
    }
}
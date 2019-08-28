/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/


using System.Threading.Tasks;
using Dolittle.Lifecycle;
using Dolittle.ResourceTypes.Configuration;
using EventStore.ClientAPI;

namespace Dolittle.Runtime.Events.EventStore
{
    /// <summary>
    /// Represents a connection to the EventStore
    /// </summary>
    [SingletonPerTenant]
    public class EventStoreConnector
    {
        readonly EventStoreConfiguration _configuration;

        readonly IEventStoreConnection _connection;

        readonly Task _connectTask;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public EventStoreConnector(IConfigurationFor<EventStoreConfiguration> configuration)
        {
            _configuration = configuration.Instance;
            _connection = EventStoreConnection.Create(_configuration.ConnectionString);
            _connectTask = _connection.ConnectAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public IEventStoreConnection Connection
        {
            get
            {
                _connectTask.Wait();
                return _connection;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Instance
        {
            get => _configuration.Instance;
        }
    }
}
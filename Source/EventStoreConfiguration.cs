/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/


namespace Dolittle.Runtime.Events.EventStore
{
    /// <summary>
    /// Represents a resource configuration for the EventStore event store implementation
    /// </summary>
    public class EventStoreConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the instance id of the event store in the EventStore server
        /// </summary>
        public string Instance { get; set; }
    }
}
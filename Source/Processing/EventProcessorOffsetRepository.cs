/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System;
using Dolittle.Runtime.Events.EventStore;
using Dolittle.Runtime.Events.Store;
using Dolittle.Serialization.Json;
using EventStore.ClientAPI;

namespace Dolittle.Runtime.Events.Processing.EventStore
{
    /// <summary>
    /// EventStore implementation of <see cref="IEventProcessorOffsetRepository"/>
    /// </summary>
    public class EventProcessorOffsetRepository : IEventProcessorOffsetRepository
    {
        readonly IEventStoreConnection _connection;
        readonly ISerializer _serializer;
        readonly string _streamPrefix;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="serializer"></param>
        public EventProcessorOffsetRepository(EventStoreConnector connector, ISerializer serializer)
        {
            _streamPrefix = $"{connector.Instance}";
            _connection = connector.Connection;
            _serializer = serializer;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // TODO: Should we do anything here?
        }

        /// <inheritdoc />
        public CommittedEventVersion Get(EventProcessorId eventProcessorId)
        {
            var result = _connection.ReadEventAsync(GetStreamForEventProcessorId(eventProcessorId), StreamPosition.End, true).Result;
            if (result.Event.HasValue)
            {
                return _serializer.FromJsonBytes<CommittedEventVersion>(result.Event.Value.Event.Data);
            }
            return CommittedEventVersion.None;
        }

        /// <inheritdoc />
        public void Set(EventProcessorId eventProcessorId, CommittedEventVersion committedEventVersion)
        {
            _connection.AppendToStreamAsync(
                GetStreamForEventProcessorId(eventProcessorId),
                ExpectedVersion.Any,
                CreateCommittedEventVersionEvent(committedEventVersion)
            ).Wait();
        }

        string GetStreamForEventProcessorId(EventProcessorId eventProcessorId)
        {
            return $"{_streamPrefix}-offsets-{eventProcessorId}";
        }

        EventData CreateCommittedEventVersionEvent(CommittedEventVersion committedEventVersion)
        {
            return new EventData(
                Guid.NewGuid(),
                "EventProcessorOffset",
                true,
                _serializer.ToJsonBytes(committedEventVersion),
                Array.Empty<byte>()
            );
        }
    }
}
/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System;
using Dolittle.Runtime.Events.EventStore;
using Dolittle.Serialization.Json;
using EventStore.ClientAPI;

namespace Dolittle.Runtime.Events.Relativity.EventStore
{
    /// <summary>
    /// EventStore implementation of <see cref="IGeodesics"/>
    /// </summary>
    public class Geodesics : IGeodesics
    {
        readonly IEventStoreConnection _connection;
        readonly ISerializer _serializer;
        readonly string _streamPrefix;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="serializer"></param>
        public Geodesics(EventStoreConnector connector, ISerializer serializer)
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
        public ulong GetOffset(EventHorizonKey key)
        {
            var result = _connection.ReadEventAsync(GetStreamForEventHorizonKey(key), StreamPosition.End, true).Result;
            if (result.Event.HasValue)
            {
                return _serializer.FromJsonBytes<ulong>(result.Event.Value.Event.Data);
            }
            return 0;
        }

        /// <inheritdoc />
        public void SetOffset(EventHorizonKey key, ulong offset)
        {
            _connection.AppendToStreamAsync(
                GetStreamForEventHorizonKey(key),
                ExpectedVersion.Any,
                CreateEventHorizonOffsetEvent(key, offset)
            ).Wait();
        }


        string GetStreamForEventHorizonKey(EventHorizonKey key)
        {
            return $"{_streamPrefix}-geodesics-{key}";
        }

        EventData CreateEventHorizonOffsetEvent(EventHorizonKey key, ulong offset)
        {
            return new EventData(
                Guid.NewGuid(),
                "EventHorizonOffset",
                true,
                _serializer.ToJsonBytes(offset),
                Array.Empty<byte>()
            );
        }
    }
}
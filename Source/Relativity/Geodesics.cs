/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

namespace Dolittle.Runtime.Events.Relativity.EventStore
{
    /// <summary>
    /// EventStore implementation of <see cref="IGeodesics"/>
    /// </summary>
    public class Geodesics : IGeodesics
    {
        /// <inheritdoc />
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public ulong GetOffset(EventHorizonKey key)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void SetOffset(EventHorizonKey key, ulong offset)
        {
            throw new System.NotImplementedException();
        }
    }
}
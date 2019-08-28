/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System;

namespace Dolittle.Runtime.Events.Store.EventStore
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CorruptedEventStore : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cause"></param>
        /// <returns></returns>
        public CorruptedEventStore(string cause) : base(cause) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cause"></param>
        /// <param name="inner"></param>
        /// <returns></returns>
        public CorruptedEventStore(string cause, Exception inner) : base(cause, inner) { }
    }
}
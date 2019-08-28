/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using Dolittle.Execution;

namespace Dolittle.Runtime.Events.EventStore
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StreamVersionedValue<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="version"></param>
        /// <param name="value"></param>
        public StreamVersionedValue(Stream stream, StreamVersion version, T value)
        {
            Ensure.IsNotNull(nameof(stream), stream);
            
            Stream = stream;
            Version = version;
            Value = value;
        }


        /// <summary>
        /// 
        /// </summary>
        public Stream Stream { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public StreamVersion Version { get; }

        /// <summary>
        /// 
        /// </summary>
        public T Value { get; }
    }
}
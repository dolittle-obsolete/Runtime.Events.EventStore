/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using Dolittle.Concepts;

namespace Dolittle.Runtime.Events.EventStore
{
    /// <summary>
    /// An incrementing number used to identify version of a stream (the index of an event in the stream) in EventStore
    /// </summary>
    public class StreamVersion : ConceptAs<long>
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="StreamVersion" /> with the stream version
        /// </summary>
        /// <param name="value"></param>
        public StreamVersion(long value) => Value = value;
    
        /// <summary>
        /// An implicit conversion from <see cref="ulong" /> to <see cref="StreamVersion" />
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator StreamVersion(long value) => new StreamVersion(value);
    }
}
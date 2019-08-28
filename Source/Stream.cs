/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using Dolittle.Concepts;

namespace Dolittle.Runtime.Events.EventStore
{
    /// <summary>
    /// 
    /// </summary>
    public class Stream : ConceptAs<string>
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="Stream" /> with the stream
        /// </summary>
        /// <param name="value"></param>
        public Stream(string value) => Value = value;
    
        /// <summary>
        /// An implicit conversion from <see cref="string" /> to <see cref="Stream" />
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Stream(string value) => new Stream(value);
    }
}
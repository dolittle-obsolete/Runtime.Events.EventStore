/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

using System.IO;
using System.Text;
using Dolittle.Serialization.Json;

namespace Dolittle.Runtime.Events.EventStore
{
    /// <summary>
    /// 
    /// </summary>
    public static class SerializerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="instance"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static byte[] ToJsonBytes(this ISerializer serializer, object instance, ISerializationOptions options = null)
        {
            return Encoding.UTF8.GetBytes(serializer.ToJson(instance, options));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="data"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromJsonBytes<T>(this ISerializer serializer, byte[] data, ISerializationOptions options = null)
        {
            var json = Encoding.UTF8.GetString(data);
            return serializer.FromJson<T>(json, options);
        }
    }
}
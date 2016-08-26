using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace Snowflake.Protocol
{
    public static class Extensions
    {


        public static byte[] Serialize<T>(this T instance)
        {
            var formatter = Serializer.CreateFormatter<T>();
            using (var memoryStream = new System.IO.MemoryStream())
            {
                formatter.Serialize(memoryStream, instance);
                return memoryStream.ToArray();
            }            
        }

        public static T Deserialize<T>(this byte[] data)
        {
            T obj;
            using (MemoryStream ms = new MemoryStream(data))
            {
                obj = Serializer.Deserialize<T>(ms);
            }
            return obj;
        }
    }
}

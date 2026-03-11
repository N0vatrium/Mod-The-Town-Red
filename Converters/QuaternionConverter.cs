using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::Newtonsoft.Json;
using UnityEngine;

namespace MTTR.Converters
{
   

    namespace Newtonsoft.Json.UnityConverters.Mathematics
    {
        /// <summary>
        /// Custom Newtonsoft.Json converter <see cref="JsonConverter"/> for the Unity.Mathematics type <see cref="quaternion"/>.
        /// </summary>
        public class QuaternionConverter : PartialConverter<Quaternion>
        {
            protected override void ReadValue(ref Quaternion value, string name, JsonReader reader, JsonSerializer serializer)
            {
                switch (name)
                {
                    case nameof(value.x):
                        value.x = ReadAsFloat(reader) ?? 0f;
                        break;
                    case nameof(value.y):
                        value.y = ReadAsFloat(reader) ?? 0f;
                        break;
                    case nameof(value.z):
                        value.z = ReadAsFloat(reader) ?? 0f;
                        break;
                    case nameof(value.w):
                        value.w = ReadAsFloat(reader) ?? 0f;
                        break;
                }
            }

            protected override void WriteJsonProperties(JsonWriter writer, Quaternion value, JsonSerializer serializer)
            {
                writer.WritePropertyName(nameof(value.x));
                writer.WriteValue(value.x);
                writer.WritePropertyName(nameof(value.y));
                writer.WriteValue(value.y);
                writer.WritePropertyName(nameof(value.z));
                writer.WriteValue(value.z);
                writer.WritePropertyName(nameof(value.w));
                writer.WriteValue(value.w);
            }
        }
    }
}

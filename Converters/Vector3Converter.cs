using Newtonsoft.Json;
using System.Globalization;
using UnityEngine;

namespace MTTR.Converters
{

    public class Vector3Converter : PartialConverter<Vector3>
    {
        protected override void ReadValue(ref Vector3 value, string name, JsonReader reader, JsonSerializer serializer)
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
            }
        }

        protected override void WriteJsonProperties(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WritePropertyName(nameof(value.x));
            writer.WriteValue(value.x);
            writer.WritePropertyName(nameof(value.y));
            writer.WriteValue(value.y);
            writer.WritePropertyName(nameof(value.z));
            writer.WriteValue(value.z);
        }
        
    }
}

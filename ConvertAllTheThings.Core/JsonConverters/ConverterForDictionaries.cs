using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;
using System.Reflection;

namespace ConvertAllTheThings.Core.JsonConverters
{
    internal class ConverterForDictionaries : JsonConverter<IEnumerable>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsAssignableTo(typeof(IEnumerable)))
                return false;

            if (typeToConvert.GetProperty("Keys", BindingFlags.Public | BindingFlags.Instance) is null)
                return false;

            if (typeToConvert.GetProperty("Values", BindingFlags.Public | BindingFlags.Instance) is null)
                return false;

            return true;
        }

        public override IEnumerable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (dynamic kvp in value)
            {
                string key = kvp.Key.ToString();
                string val = kvp.Value.ToString();
                writer.WriteString(key, val);
            }

            writer.WriteEndObject();
        }
    }
}

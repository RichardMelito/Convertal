using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConvertAllTheThings.Core.JsonConverters
{
    public class MaybeNamedConverter : JsonConverter<MaybeNamed>
    {
        public override MaybeNamed? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, MaybeNamed value, JsonSerializerOptions options)
        {
            writer.WriteString(nameof(MaybeNamed.MaybeName), value.MaybeName);
        }
    }
}

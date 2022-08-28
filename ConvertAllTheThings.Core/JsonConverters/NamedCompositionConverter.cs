using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConvertAllTheThings.Core.JsonConverters;

public class NamedCompositionConverter : JsonConverter<NamedComposition<IMaybeNamed>>
{
    public override NamedComposition<IMaybeNamed>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, NamedComposition<IMaybeNamed> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

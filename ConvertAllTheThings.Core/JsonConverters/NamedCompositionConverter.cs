using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConvertAllTheThings.Core.JsonConverters;

public class NamedCompositionConverter : JsonConverter<NamedComposition>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableTo(typeof(INamedComposition));
    }

    public override INamedComposition? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, INamedComposition value, JsonSerializerOptions options)
    {

        IReadOnlyDictionary<string, decimal> INamedComposition =>
            Composition.ToDictionary(kvp => kvp.Key.ToString()!, kvp => kvp.Value);

        writer.WritePropertyName(nameof(INamedComposition));
        JsonSerializer.Serialize(writer, value, options);
    }
}

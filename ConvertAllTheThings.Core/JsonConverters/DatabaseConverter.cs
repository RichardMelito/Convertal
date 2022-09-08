using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConvertAllTheThings.Core.JsonConverters
{
    public class DatabaseConverter : JsonConverter<Database>
    {
        public override Database? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Database database = new();

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            reader.ReadStartOfArrayProperty(nameof(Database.Prefixes));
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                var proto = JsonSerializer.Deserialize<PrefixProto>(ref reader, options)!;
                database.DefinePrefix(proto);
                reader.ReadThrowIfFalse();
            }

            reader.ReadStartOfArrayProperty(nameof(Database.BaseQuantitys));
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                var proto = JsonSerializer.Deserialize<BaseQuantityProto>(ref reader, options)!;
                database.DefineBaseQuantity(proto);
                reader.ReadThrowIfFalse();
            }

            reader.ReadStartOfArrayProperty(nameof(Database.DerivedQuantitys));
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                var proto = JsonSerializer.Deserialize<DerivedQuantityProto>(ref reader, options)!;
                database.DefineDerivedQuantity(proto);
                reader.ReadThrowIfFalse();
            }

            return database;
        }

        public override void Write(Utf8JsonWriter writer, Database value, JsonSerializerOptions options)
        {
            //JsonSerializer.Serialize(writer, value, options);
            //return;
            
            // Just need MaybeNamedsByType. That plus constructors are everything we need.
            writer.WriteStartObject();

            // Prefixes first since they're the simplest
            writer.WritePropertyName(nameof(Database.Prefixes));
            JsonSerializer.Serialize(writer, value.Prefixes, options);

            // Now BaseQuantitys since they are the most fundamental.
            // Don't need their BaseQuantityComposition since it is just themselves.
            // For all Quantitys, only store the names of their FundamentalUnits to avoid circular references.
            writer.WritePropertyName(nameof(Database.BaseQuantitys));
            JsonSerializer.Serialize(writer, value.BaseQuantitys, options);

            writer.WritePropertyName(nameof(Database.DerivedQuantitys));
            JsonSerializer.Serialize(writer, value.DerivedQuantitys, options);

            writer.WritePropertyName(nameof(Database.BaseUnits));
            JsonSerializer.Serialize(writer, value.BaseUnits, options);

            writer.WritePropertyName(nameof(Database.DerivedUnits));
            JsonSerializer.Serialize(writer, value.DerivedUnits, options);

            writer.WritePropertyName(nameof(Database.MeasurementSystems));
            JsonSerializer.Serialize(writer, value.MeasurementSystems, options);

            writer.WriteEndObject();
        }
    }
}

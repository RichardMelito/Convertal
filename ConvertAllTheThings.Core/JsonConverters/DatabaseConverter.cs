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

            List<UnitProto> selfComposedBaseUnits = new();
            LinkedList<UnitProto> otherComposedBaseUnits = new();
            reader.ReadStartOfArrayProperty(nameof(Database.BaseUnits));
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                var proto = JsonSerializer.Deserialize<UnitProto>(ref reader, options)!;
                if (proto.OtherUnitComposition is null)
                    selfComposedBaseUnits.Add(proto);
                else
                    otherComposedBaseUnits.AddLast(proto);
                reader.ReadThrowIfFalse();
            }

            List<UnitProto> selfComposedDerivedUnits = new();
            LinkedList<UnitProto> otherComposedDerivedUnits = new();
            reader.ReadStartOfArrayProperty(nameof(Database.DerivedUnits));
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                var proto = JsonSerializer.Deserialize<UnitProto>(ref reader, options)!;
                if (proto.OtherUnitComposition is null)
                    selfComposedDerivedUnits.Add(proto);
                else
                    otherComposedDerivedUnits.AddLast(proto);
                reader.ReadThrowIfFalse();
            }

            foreach (var proto in selfComposedBaseUnits)
                database.DefineBaseUnit(proto);

            foreach (var proto in selfComposedDerivedUnits)
                database.DefineDerivedUnit(proto);
            
            while (otherComposedBaseUnits.Count > 0 || otherComposedDerivedUnits.Count > 0)
            {
                var countAtStartOfLoop = otherComposedBaseUnits.Count + otherComposedDerivedUnits.Count;
                var node = otherComposedBaseUnits.First;
                while (node != null)
                {
                    var canParseComposition = _DatabaseCanParseComposition();
                    var oldNode = node;
                    node = node.Next;

                    if (canParseComposition)
                    {
                        database.DefineBaseUnit(oldNode.Value);
                        otherComposedBaseUnits.Remove(oldNode);
                    }
                }

                node = otherComposedDerivedUnits.First;
                while (node != null)
                {
                    var canParseComposition = _DatabaseCanParseComposition();
                    var oldNode = node;
                    node = node.Next;

                    if (canParseComposition)
                    {
                        database.DefineDerivedUnit(oldNode.Value);
                        otherComposedDerivedUnits.Remove(oldNode);
                    }
                }

                bool _DatabaseCanParseComposition()
                {
                    // TODO can precalculate this rather than rerunning every iteration
                    var unitNames = node.Value.OtherUnitComposition!
                        .Select(kvp => kvp.Key.Split('_'))
                        .Select(split => split.Length > 2 ? throw new InvalidOperationException() : split.Last())
                        .ToArray();

                    foreach (var name in unitNames)
                    {
                        if (!database.TryGetFromName<Unit>(name, out _))
                            return false;
                    }

                    return true;
                }

                var countAtEndOfLoop = otherComposedBaseUnits.Count + otherComposedDerivedUnits.Count;
                if (countAtEndOfLoop == countAtStartOfLoop)
                    throw new InvalidOperationException("Infinite loop detected. Check the JSON file for circular or orphan unit composition references.");
            }

            reader.ReadStartOfArrayProperty(nameof(Database.MeasurementSystems));
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                var proto = JsonSerializer.Deserialize<MeasurementSystemProto>(ref reader, options)!;
                var quantityUnitPairs = proto.QuantityToUnitDictionary.ToDictionary(
                    kvp => database.GetFromName<Quantity>(kvp.Key),
                    kvp => database.ParseIUnit(kvp.Value))
                    .ToArray();

                MeasurementSystem measurementSystem = new(database, proto.Name!);
                measurementSystem.SetQuantityUnitPairs(quantityUnitPairs);
                reader.ReadThrowIfFalse();
            }

            reader.ReadExpectTokenType(JsonTokenType.EndObject);
            return database;
        }

        public override void Write(Utf8JsonWriter writer, Database value, JsonSerializerOptions options)
        {
            // Just need MaybeNamedsByType. That plus constructors are everything we need.
            writer.WriteStartObject();

            // Prefixes first since they're the simplest
            writer.WritePropertyName(nameof(Database.Prefixes));
            JsonSerializer.Serialize(writer, value.Prefixes.Select(q => q.ToProto()), options);

            // Now BaseQuantitys since they are the most fundamental.
            // Don't need their BaseQuantityComposition since it is just themselves.
            // For all Quantitys, only store the names of their FundamentalUnits to avoid circular references.
            writer.WritePropertyName(nameof(Database.BaseQuantitys));
            JsonSerializer.Serialize(writer, value.BaseQuantitys.Select(q => q.ToProto()), options);

            writer.WritePropertyName(nameof(Database.DerivedQuantitys));
            JsonSerializer.Serialize(writer, value.DerivedQuantitys.Select(q => q.ToProto()), options);

            writer.WritePropertyName(nameof(Database.BaseUnits));
            JsonSerializer.Serialize(writer, value.BaseUnits.Select(q => q.ToProto()), options);

            writer.WritePropertyName(nameof(Database.DerivedUnits));
            JsonSerializer.Serialize(writer, value.DerivedUnits.Select(q => q.ToProto()), options);

            writer.WritePropertyName(nameof(Database.MeasurementSystems));
            JsonSerializer.Serialize(writer, value.MeasurementSystems.Select(q => q.ToProto()), options);

            writer.WriteEndObject();
        }
    }
}

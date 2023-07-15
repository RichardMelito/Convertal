// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Convertal.Core.Extensions;

namespace Convertal.Core.JsonConverters;

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

        reader.ReadStartOfArrayProperty(nameof(Database.ScalarBaseQuantitys));
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var proto = JsonSerializer.Deserialize<ScalarBaseQuantityProto>(ref reader, options)!;
            database.DefineScalarBaseQuantity(proto);
            reader.ReadThrowIfFalse();
        }

        reader.ReadStartOfArrayProperty(nameof(Database.VectorBaseQuantitys));
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var proto = JsonSerializer.Deserialize<VectorBaseQuantityProto>(ref reader, options)!;
            database.DefineVectorBaseQuantity(proto);
            reader.ReadThrowIfFalse();
        }

        reader.ReadStartOfArrayProperty(nameof(Database.ScalarDerivedQuantitys));
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var proto = JsonSerializer.Deserialize<ScalarDerivedQuantityProto>(ref reader, options)!;
            database.DefineScalarDerivedQuantity(proto);
            reader.ReadThrowIfFalse();
        }

        reader.ReadStartOfArrayProperty(nameof(Database.VectorDerivedQuantitys));
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var proto = JsonSerializer.Deserialize<VectorDerivedQuantityProto>(ref reader, options)!;
            database.DefineVectorDerivedQuantity(proto);
            reader.ReadThrowIfFalse();
        }

        List<ScalarUnitProto> selfComposedScalarBaseUnits = new();
        LinkedList<ScalarUnitProto> otherComposedScalarBaseUnits = new();
        reader.ReadStartOfArrayProperty(nameof(Database.ScalarBaseUnits));
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var proto = JsonSerializer.Deserialize<ScalarUnitProto>(ref reader, options)!;
            if (proto.OtherUnitComposition is null)
                selfComposedScalarBaseUnits.Add(proto);
            else
                otherComposedScalarBaseUnits.AddLast(proto);
            reader.ReadThrowIfFalse();
        }

        //List<ScalarUnitProto> selfComposedVectorBaseUnits = new();
        //LinkedList<ScalarUnitProto> otherComposedVectorBaseUnits = new();
        //reader.ReadStartOfArrayProperty(nameof(Database.VectorBaseUnits));
        //while (reader.TokenType != JsonTokenType.EndArray)
        //{
        //    var proto = JsonSerializer.Deserialize<ScalarUnitProto>(ref reader, options)!;
        //    if (proto.OtherUnitComposition is null)
        //        selfComposedVectorBaseUnits.Add(proto);
        //    else
        //        otherComposedVectorBaseUnits.AddLast(proto);
        //    reader.ReadThrowIfFalse();
        //}

        List<ScalarUnitProto> selfComposedScalarDerivedUnits = new();
        LinkedList<ScalarUnitProto> otherComposedScalarDerivedUnits = new();
        reader.ReadStartOfArrayProperty(nameof(Database.ScalarDerivedUnits));
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var proto = JsonSerializer.Deserialize<ScalarUnitProto>(ref reader, options)!;
            if (proto.OtherUnitComposition is null)
                selfComposedScalarDerivedUnits.Add(proto);
            else
                otherComposedScalarDerivedUnits.AddLast(proto);
            reader.ReadThrowIfFalse();
        }

        //List<ScalarUnitProto> selfComposedVectorDerivedUnits = new();
        //LinkedList<ScalarUnitProto> otherComposedVectorDerivedUnits = new();
        //reader.ReadStartOfArrayProperty(nameof(Database.VectorDerivedUnits));
        //while (reader.TokenType != JsonTokenType.EndArray)
        //{
        //    var proto = JsonSerializer.Deserialize<ScalarUnitProto>(ref reader, options)!;
        //    if (proto.OtherUnitComposition is null)
        //        selfComposedVectorDerivedUnits.Add(proto);
        //    else
        //        otherComposedVectorDerivedUnits.AddLast(proto);
        //    reader.ReadThrowIfFalse();
        //}

        foreach (var proto in selfComposedScalarBaseUnits)
            database.DefineScalarBaseUnit(proto);

        //foreach (var proto in selfComposedVectorBaseUnits)
        //    database.DefineVectorBaseUnit(proto);

        foreach (var proto in selfComposedScalarDerivedUnits)
            database.DefineScalarDerivedUnit(proto);

        //foreach (var proto in selfComposedVectorDerivedUnits)
        //    database.DefineVectorDerivedUnit(proto);

        while (
            otherComposedScalarBaseUnits.Count > 0 ||
            otherComposedScalarDerivedUnits.Count > 0
            //||
            //otherComposedVectorBaseUnits.Count > 0 ||
            //otherComposedVectorDerivedUnits.Count > 0
            )
        {
            var countAtStartOfLoop = otherComposedScalarBaseUnits.Count +
                otherComposedScalarDerivedUnits.Count
                //+
                //otherComposedVectorBaseUnits.Count +
                //otherComposedVectorDerivedUnits.Count
                ;


            var node = otherComposedScalarBaseUnits.First;
            while (node != null)
            {
                var canParseComposition = _DatabaseCanParseComposition();
                var oldNode = node;
                node = node.Next;

                if (canParseComposition)
                {
                    database.DefineScalarBaseUnit(oldNode.Value);
                    otherComposedScalarBaseUnits.Remove(oldNode);
                }
            }

            //node = otherComposedVectorBaseUnits.First;
            //while (node != null)
            //{
            //    var canParseComposition = _DatabaseCanParseComposition();
            //    var oldNode = node;
            //    node = node.Next;

            //    if (canParseComposition)
            //    {
            //        database.DefineVectorBaseUnit(oldNode.Value);
            //        otherComposedVectorBaseUnits.Remove(oldNode);
            //    }
            //}

            node = otherComposedScalarDerivedUnits.First;
            while (node != null)
            {
                var canParseComposition = _DatabaseCanParseComposition();
                var oldNode = node;
                node = node.Next;

                if (canParseComposition)
                {
                    database.DefineScalarDerivedUnit(oldNode.Value);
                    otherComposedScalarDerivedUnits.Remove(oldNode);
                }
            }

            //node = otherComposedVectorDerivedUnits.First;
            //while (node != null)
            //{
            //    var canParseComposition = _DatabaseCanParseComposition();
            //    var oldNode = node;
            //    node = node.Next;

            //    if (canParseComposition)
            //    {
            //        database.DefineVectorDerivedUnit(oldNode.Value);
            //        otherComposedVectorDerivedUnits.Remove(oldNode);
            //    }
            //}

            bool _DatabaseCanParseComposition()
            {
                // TODO can precalculate this rather than rerunning every iteration
                var unitNames = node.Value.OtherUnitComposition!
                    .Select(kvp => kvp.Key.Split('_'))
                    .Select(split => split.Length > 2 ? throw new InvalidOperationException() : split.Last())
                    .ToArray();

                foreach (var name in unitNames)
                {
                    if (!database.TryGetFromName<ScalarUnit>(name, out _))
                        return false;
                }

                return true;
            }

            var countAtEndOfLoop = otherComposedScalarBaseUnits.Count +
                otherComposedScalarDerivedUnits.Count
                //+
                //otherComposedVectorBaseUnits.Count +
                //otherComposedVectorDerivedUnits.Count
                ;

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
        writer.WritePropertyName(nameof(Database.ScalarBaseQuantitys));
        JsonSerializer.Serialize(writer, value.ScalarBaseQuantitys.Select(q => q.ToProto()), options);
        writer.WritePropertyName(nameof(Database.VectorBaseQuantitys));
        JsonSerializer.Serialize(writer, value.VectorBaseQuantitys.Select(q => q.ToProto()), options);

        writer.WritePropertyName(nameof(Database.ScalarDerivedQuantitys));
        JsonSerializer.Serialize(writer, value.ScalarDerivedQuantitys.Select(q => q.ToProto()), options);
        writer.WritePropertyName(nameof(Database.VectorDerivedQuantitys));
        JsonSerializer.Serialize(writer, value.VectorDerivedQuantitys.Select(q => q.ToProto()), options);

        writer.WritePropertyName(nameof(Database.ScalarBaseUnits));
        JsonSerializer.Serialize(writer, value.ScalarBaseUnits.Select(q => q.ToProto()), options);
        //writer.WritePropertyName(nameof(Database.VectorBaseUnits));
        //JsonSerializer.Serialize(writer, value.VectorBaseUnits.Select(q => q.ToProto()), options);

        writer.WritePropertyName(nameof(Database.ScalarDerivedUnits));
        JsonSerializer.Serialize(writer, value.ScalarDerivedUnits.Select(q => q.ToProto()), options);
        //writer.WritePropertyName(nameof(Database.VectorDerivedUnits));
        //JsonSerializer.Serialize(writer, value.VectorDerivedUnits.Select(q => q.ToProto()), options);

        writer.WritePropertyName(nameof(Database.MeasurementSystems));
        JsonSerializer.Serialize(writer, value.MeasurementSystems.Select(q => q.ToProto()), options);

        writer.WriteEndObject();
    }
}

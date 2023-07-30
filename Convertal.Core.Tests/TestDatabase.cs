// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Xunit;

namespace Convertal.Core.Tests;

public class TestDatabase : BaseTestClass
{
    public readonly Prefix Milli;
    public readonly Prefix Kilo;

    public readonly ScalarBaseQuantity Length;
    public readonly ScalarBaseQuantity Time;
    public readonly ScalarBaseQuantity Mass;

    public readonly VectorBaseQuantity Displacement;

    public readonly ScalarBaseUnit Meter;
    public readonly ScalarBaseUnit Second;
    public readonly ScalarBaseUnit Hour;
    public readonly ScalarBaseUnit Foot;

    public readonly ScalarPrefixedBaseUnit KiloGram;

    public readonly ScalarDerivedQuantity Speed;
    public readonly ScalarDerivedQuantity SAcceleration;
    public readonly ScalarDerivedQuantity Tension;

    public readonly VectorDerivedQuantity Velocity;
    public readonly VectorDerivedQuantity VAcceleration;
    public readonly VectorDerivedQuantity Force;

    public readonly VectorDerivedUnit Newton;
    public readonly VectorDerivedUnit PoundForce;

    public readonly Unit FeetPerSecond;

    public readonly MeasurementSystem Metric;
    public readonly MeasurementSystem Imperial;

    public TestDatabase()
    {
        Milli = Database.DefinePrefix("milli", 1e-3m, "m");
        Kilo = Database.DefinePrefix("kilo", 1e3m, "k");

        Length = Database.DefineScalarBaseQuantity(nameof(Length), "Meter", quantitySymbol: "l", unitSymbol: "m");
        Time = Database.DefineScalarBaseQuantity(nameof(Time), "Second", quantitySymbol: "t", unitSymbol: "s");
        Mass = Database.DefineScalarBaseQuantity(nameof(Mass), "Gram", unitPrefix: Kilo, quantitySymbol: "m", unitSymbol: "g");

        Displacement = Database.DefineVectorBaseQuantity(Length, nameof(Displacement));

        Meter = (ScalarBaseUnit)Length.FundamentalUnit;
        Second = (ScalarBaseUnit)Time.FundamentalUnit;
        KiloGram = (ScalarPrefixedBaseUnit)Mass.FundamentalUnit;
        Foot = Database.DefineScalarBaseUnit(nameof(Foot), Meter, 0.3048m);

        Hour = Database.DefineScalarBaseUnit(nameof(Hour), Second, 3600m, symbol: "h");

        Database.DefineScalarDerivedQuantity(() => Length / Time, out Speed, nameof(Speed));
        SAcceleration = Database.DefineScalarDerivedQuantity(() => Speed / Time, nameof(SAcceleration), "a");
        Database.DefineScalarDerivedQuantity(() => Mass * SAcceleration, out Tension);

        Velocity = Database.DefineVectorDerivedQuantity(() => Displacement / Time, nameof(Velocity), "v");
        VAcceleration = Database.DefineVectorDerivedQuantity(() => Velocity / Time, nameof(VAcceleration), "a");
        Force = Database.DefineVectorDerivedQuantity(() => Mass * VAcceleration, nameof(Force), "F");

        Newton = Force.FundamentalUnit.CastAndChangeNameAndSymbol<VectorDerivedUnit>(nameof(Newton), "N");
        PoundForce = Database.DefineVectorDerivedUnit(nameof(PoundForce), Newton, 4.4482216282509m, symbol: "lbf");

        FeetPerSecond = Database.DefineScalarUnitFromComposition(nameof(FeetPerSecond), Foot.UnitComposition / Second.UnitComposition);

        Metric = new(Database, nameof(Metric));
        Metric.SetQuantityUnitPairs(new KeyValuePair<Quantity, IUnit>[]
        {
            new(Length, Meter),
            new(Time, Second),
            new(Mass, KiloGram),
            new(Force, Newton),
        });

        Imperial = new(Database, nameof(Imperial));
        Imperial.SetQuantityUnitPairs(new KeyValuePair<Quantity, IUnit>[]
        {
            new(Length, Foot),
            new(Time, Second),
            new(Force, PoundForce),
        });
    }

    [Fact]
    public void TestSerialization()
    {
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
        var text = JsonSerializer.Serialize(Database, jsonSerializerOptions);

        // TODO check that anonymous units/quantities are tested


        var deserialized = JsonSerializer.Deserialize<Database>(text, jsonSerializerOptions)!;
        
        Database.Prefixes.Should().BeEquivalentTo(deserialized.Prefixes);
        Database.ScalarBaseQuantitys.Should().BeEquivalentTo(deserialized.ScalarBaseQuantitys);
        Database.VectorBaseQuantitys.Should().BeEquivalentTo(deserialized.VectorBaseQuantitys);
        Database.ScalarDerivedQuantitys.Should().BeEquivalentTo(deserialized.ScalarDerivedQuantitys);
        Database.VectorDerivedQuantitys.Should().BeEquivalentTo(deserialized.VectorDerivedQuantitys);
        Database.ScalarBaseUnits.Should().BeEquivalentTo(deserialized.ScalarBaseUnits);
        Database.VectorBaseUnits.Should().BeEquivalentTo(deserialized.VectorBaseUnits);
        Database.ScalarPrefixedBaseUnits.Should().BeEquivalentTo(deserialized.ScalarPrefixedBaseUnits);
        Database.VectorPrefixedBaseUnits.Should().BeEquivalentTo(deserialized.VectorPrefixedBaseUnits);
        Database.ScalarDerivedUnits.Should().BeEquivalentTo(deserialized.ScalarDerivedUnits);
        Database.VectorDerivedUnits.Should().BeEquivalentTo(deserialized.VectorDerivedUnits);
        Database.ScalarPrefixedDerivedUnits.Should().BeEquivalentTo(deserialized.ScalarPrefixedDerivedUnits);
        Database.VectorPrefixedDerivedUnits.Should().BeEquivalentTo(deserialized.VectorPrefixedDerivedUnits);
        Database.MeasurementSystems.Should().BeEquivalentTo(deserialized.MeasurementSystems);
        Database.MaybeNamedsByType.Should().BeEquivalentTo(deserialized.MaybeNamedsByType);
        Database.QuantitiesByComposition.Should().BeEquivalentTo(deserialized.QuantitiesByComposition);
        Database.ScalarEmptyQuantity.Should().BeEquivalentTo(deserialized.ScalarEmptyQuantity);
        Database.VectorEmptyQuantity.Should().BeEquivalentTo(deserialized.VectorEmptyQuantity);
        Database.ScalarEmptyUnit.Should().BeEquivalentTo(deserialized.ScalarEmptyUnit);
        Database.VectorEmptyUnit.Should().BeEquivalentTo(deserialized.VectorEmptyUnit);
        Database.PrefixedUnits.Should().BeEquivalentTo(deserialized.PrefixedUnits);

        // Needs 'Equal' or else the dictionary comparison messes up
        Database.CompositionAndQuantitiesDictionary.Should().Equal(deserialized.CompositionAndQuantitiesDictionary);
    }
}

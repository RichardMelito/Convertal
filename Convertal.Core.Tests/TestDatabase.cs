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

    public readonly ScalarDerivedUnit Newton;
    public readonly ScalarDerivedUnit PoundForce;

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

        Meter = (ScalarBaseUnit)Length.FundamentalUnit;
        Second = (ScalarBaseUnit)Time.FundamentalUnit;
        KiloGram = (ScalarPrefixedBaseUnit)Mass.FundamentalUnit;
        Foot = Database.DefineScalarBaseUnit(nameof(Foot), Meter, 0.3048m);

        Hour = Database.DefineScalarBaseUnit(nameof(Hour), Second, 3600m, symbol: "h");

        Database.DefineScalarDerivedQuantity(() => Length / Time, out Speed, "v");
        SAcceleration = Database.DefineScalarDerivedQuantity(() => Speed / Time, "acceleration", "a");
        Database.DefineScalarDerivedQuantity(() => Mass * SAcceleration, out Tension, "F");

        Velocity = Database.DefineDerivedQuantity(() => Length / Time, nameof(Velocity), "v");
        Acceleration = Database.DefineDerivedQuantity(() => Velocity / Time, nameof(Acceleration), "a");
        Force = Database.DefineDerivedQuantity(() => Mass * Acceleration, nameof(Force), "F");

        Newton = Force.FundamentalUnit.CastAndChangeNameAndSymbol<DerivedUnit>(nameof(Newton), "N");
        PoundForce = Database.DefineDerivedUnit(nameof(PoundForce), Newton, 4.4482216282509m, symbol: "lbf");

        FeetPerSecond = Database.DefineFromComposition(nameof(FeetPerSecond), Foot.UC / Second.UC);

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
        // TODO don't write this out to a file
        //File.WriteAllText("database.json", text);

        // TODO check that anonymous units/quantities are tested


        var deserialized = JsonSerializer.Deserialize<Database>(text, jsonSerializerOptions)!;
        Database.Should().BeEquivalentTo(deserialized);
    }
}

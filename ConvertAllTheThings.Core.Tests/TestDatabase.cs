using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Xunit;

namespace ConvertAllTheThings.Core.Tests
{
    public class TestDatabase : BaseTestClass
    {
        public readonly Prefix Milli;
        public readonly Prefix Kilo;

        public readonly BaseQuantity Length;
        public readonly BaseQuantity Time;
        public readonly BaseQuantity Mass;

        public readonly BaseUnit Meter;
        public readonly BaseUnit Second;
        public readonly BaseUnit Hour;
        public readonly BaseUnit Foot;

        public readonly PrefixedBaseUnit KiloGram;

        public readonly DerivedQuantity Velocity;
        public readonly DerivedQuantity Acceleration;
        public readonly DerivedQuantity Force;

        public readonly DerivedUnit Newton;
        public readonly DerivedUnit PoundForce;

        public readonly Unit FeetPerSecond;

        public readonly MeasurementSystem Metric;
        public readonly MeasurementSystem Imperial;

        public TestDatabase()
        {
            Milli = Database.DefinePrefix("milli", 1e-3m, "m");
            Kilo = Database.DefinePrefix("kilo", 1e3m, "k");

            Length = Database.DefineBaseQuantity(nameof(Length), "Meter", quantitySymbol: "l", unitSymbol: "m");
            Time = Database.DefineBaseQuantity(nameof(Time), "Second", quantitySymbol: "t", unitSymbol: "s");
            Mass = Database.DefineBaseQuantity(nameof(Mass), "Gram", unitPrefix: Kilo, quantitySymbol: "m", unitSymbol: "g");

            Meter = (BaseUnit)Length.FundamentalUnit;
            Second = (BaseUnit)Time.FundamentalUnit;
            KiloGram = (PrefixedBaseUnit)Mass.FundamentalUnit;
            Foot = Database.DefineBaseUnit(nameof(Foot), Meter, 0.3048m);

            Hour = Database.DefineBaseUnit(nameof(Hour), Second, 3600m, symbol: "h");

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
            File.WriteAllText("database.json", text);

            // TODO check that anonymous units/quantities are tested


            var deserialized = JsonSerializer.Deserialize<Database>(text, jsonSerializerOptions)!;

            Database.EmptyQuantity.Should().BeEquivalentTo(deserialized.EmptyQuantity);
            Database.EmptyUnit.Should().BeEquivalentTo(deserialized.EmptyUnit);
            Database.Prefixes.Should().BeEquivalentTo(deserialized.Prefixes);
            Database.BaseQuantitys.Should().BeEquivalentTo(deserialized.BaseQuantitys);
            Database.DerivedQuantitys.Should().BeEquivalentTo(deserialized.DerivedQuantitys);
            Database.BaseUnits.Should().BeEquivalentTo(deserialized.BaseUnits);
            Database.DerivedUnits.Should().BeEquivalentTo(deserialized.DerivedUnits);


            Database.Should().BeEquivalentTo(deserialized);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConvertAllTheThings.Defaults;
using ConvertAllTheThings.Core;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConvertAllTheThings.Core.JsonConverters;

namespace ConvertAllTheThings.Defaults.Tests
{
    [TestClass]
    public class TestAll
    {
        static TestAll()
        {
            Global.InitializeAssembly();
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<Quantity, decimal>());
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<BaseQuantity, decimal>());
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<DerivedQuantity, decimal>());

            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<Unit, decimal>());
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<BaseUnit, decimal>());
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<DerivedUnit, decimal>());

            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<IBaseUnit, decimal>());
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<IDerivedUnit, decimal>());
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<IUnit, decimal>());

            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<PrefixedBaseUnit, decimal>());
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<PrefixedDerivedUnit, decimal>());
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<PrefixedUnit, decimal>());

            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<IMaybeNamed, decimal>());
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<Prefix, decimal>());
            JsonSettings.Converters.Add(new MaybeNamedDictionaryJsonConverter<MeasurementSystem, decimal>());

            JsonSettings.Converters.Add(new MaybeNamedConverter());
        }

        [TestMethod]
        public void TestDerivedQuantities()
        {
            foreach (var field in typeof(DerivedQuantities).GetFields())
            {
                var quantity = (DerivedQuantity)field.GetValue(field.Name)!;
                Assert.AreEqual(field.Name, quantity.Name);
            }
        }

        [TestMethod]
        public void TestBaseQuantities()
        {
            foreach (var field in typeof(BaseQuantities).GetFields())
            {
                var quantity = (BaseQuantity)field.GetValue(field.Name)!;
                Assert.AreEqual(field.Name, quantity.Name);
            }
        }

        [TestMethod]
        public void TestBaseUnits()
        {
            foreach (var field in typeof(BaseUnits).GetFields())
            {
                var unit = field.GetValue(field.Name) as BaseUnit;
                if (unit is null)
                    continue;

                Assert.AreEqual(field.Name, unit.Name);
            }
        }

        [TestMethod]
        public void TestDerivedUnits()
        {
            foreach (var field in typeof(DerivedUnits).GetFields())
            {
                var unit = field.GetValue(field.Name) as DerivedUnit;
                if (unit is null)
                    continue;

                Assert.AreEqual(field.Name, unit.Name);
            }
        }

        static readonly JsonSerializerOptions JsonSettings = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        [TestMethod]
        public void TestSerialization()
        {
            var fileName = "test.json";
            var jsonString = JsonSerializer.Serialize(BaseQuantities.Mass, JsonSettings);
            File.WriteAllText(fileName, jsonString);
        }

        [TestMethod]
        public void TestDeserialization()
        {
            //var fileName = "test.json";
            //var jsonString = File.ReadAllText(fileName);
            //var hertz = JsonConvert.DeserializeObject<DerivedUnit>(jsonString,
            //    JsonSettings);
        }
    }
}

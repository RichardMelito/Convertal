using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConvertAllTheThings.Defaults;
using ConvertAllTheThings.Core;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConvertAllTheThings.Defaults.Tests
{
    [TestClass]
    public class TestAll
    {
        static TestAll()
        {
            Global.InitializeAssembly();
        }

        [TestMethod]
        public void TestDerivedQuantities()
        {
            foreach (var field in typeof(DerivedQuantities).GetFields())
            {
                var quantity = (DerivedQuantity)field.GetValue(field.Name)!;
                Assert.AreEqual(field.Name, quantity.MaybeName);
            }
        }

        [TestMethod]
        public void TestBaseQuantities()
        {
            foreach (var field in typeof(BaseQuantities).GetFields())
            {
                var quantity = (BaseQuantity)field.GetValue(field.Name)!;
                Assert.AreEqual(field.Name, quantity.MaybeName);
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

                Assert.AreEqual(field.Name, unit.MaybeName);
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

                Assert.AreEqual(field.Name, unit.MaybeName);
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
            var jsonString = JsonSerializer.Serialize(DerivedUnits.Hertz, JsonSettings);
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

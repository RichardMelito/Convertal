using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConvertAllTheThings.Defaults;
using ConvertAllTheThings.Core;

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
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConvertAllTheThings.Defaults;
using ConvertAllTheThings.Core;

namespace ConvertAllTheThings.Defaults.Tests
{
    [TestClass]
    public class TestDerivedQuantities
    {
        [TestMethod]
        public void TestMethod1()
        {
            Global.InitializeAssembly();

            foreach (var field in typeof(DerivedQuantities).GetFields())
            {
                var quantity = (DerivedQuantity)field.GetValue(field.Name)!;
                Assert.AreEqual(field.Name, quantity.MaybeName);
            }
        }
    }
}

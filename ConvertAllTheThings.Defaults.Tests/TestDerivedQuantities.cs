using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConvertAllTheThings.Defaults;

namespace ConvertAllTheThings.Defaults.Tests
{
    [TestClass]
    public class TestDerivedQuantities
    {
        [TestMethod]
        public void TestMethod1()
        {
            ConvertAllTheThings.Core.Global.InitializeAssembly();
            var x = DerivedQuantities.Acceleration;
        }
    }
}

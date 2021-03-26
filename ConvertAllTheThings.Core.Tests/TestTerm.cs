using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core;
using static ConvertAllTheThings.Core.Tests.MoreAssertions;

namespace ConvertAllTheThings.Core.Tests
{
    [TestClass]
    public class TestTerm
    {
        const decimal DELTA = 0.000000001m;

        static TestTerm()
        {
            Global.InitializeAssembly();
        }

        [TestMethod]
        public void TestTemperatures()
        {
            using var temperature = BaseQuantity.DefineNewBaseQuantity(
                "Temperature", "Kelvin");

            var kelvin = temperature.FundamentalUnit;
            using var celsius = new BaseUnit("Celsius", kelvin, 1m, 273m);
            using var fahrenheit = new BaseUnit("Fahrenheit", celsius, 5m / 9m, -32m);
            
            AssertAreEqual(0m, kelvin.ConvertToFundamental(0m).Magnitude, DELTA);
            AssertAreEqual(273m, celsius.ConvertToFundamental(0m).Magnitude, DELTA);
            AssertAreEqual(373m, celsius.ConvertToFundamental(100m).Magnitude, DELTA);
            AssertAreEqual(273m, fahrenheit.ConvertToFundamental(32m).Magnitude, DELTA);
            AssertAreEqual(373m, fahrenheit.ConvertToFundamental(212m).Magnitude, DELTA);
            AssertAreEqual(-40m, celsius.ConvertTo(-40m, fahrenheit).Magnitude, DELTA);
            AssertAreEqual(-40m, fahrenheit.ConvertTo(-40m, celsius).Magnitude, DELTA);
            AssertAreEqual(0m, fahrenheit.ConvertTo(32m, celsius).Magnitude, DELTA);
            AssertAreEqual(32m, celsius.ConvertTo(0, fahrenheit).Magnitude, DELTA);
            AssertAreEqual(100m, fahrenheit.ConvertTo(212m, celsius).Magnitude, DELTA);
            AssertAreEqual(212m, celsius.ConvertTo(100m, fahrenheit).Magnitude, DELTA);
            AssertAreEqual(373m, fahrenheit.ConvertToFundamental(212m).Magnitude, DELTA);
            AssertAreEqual(0m, kelvin.ConvertTo(273m, celsius).Magnitude, DELTA);
            AssertAreEqual(32m, kelvin.ConvertTo(273m, fahrenheit).Magnitude, DELTA);
            AssertAreEqual(100m, kelvin.ConvertTo(373m, celsius).Magnitude, DELTA);
            AssertAreEqual(212m, kelvin.ConvertTo(373m, fahrenheit).Magnitude, DELTA);
        }
    }
}

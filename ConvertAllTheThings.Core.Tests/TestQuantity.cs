using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core;
using static ConvertAllTheThings.Core.Quantity;

namespace ConvertAllTheThings.Core.Tests
{
    [TestClass]
    public class TestQuantity
    {
        /*  Empty operations
         *  define basequantities
         *  *, /, pow
         *  GetFromBaseCOmposotions
         *  Correct fundamentalunits
         */

        static readonly Prefix s_testPrefix = new(2, "TestPrefix");
        static readonly BaseQuantity s_baseQuantity1;
        static readonly BaseQuantity s_baseQuantity2;


        static TestQuantity()
        {
            Global.InitializeAssembly();
            s_baseQuantity1 = BaseQuantity.DefineNewBaseQuantity(
                "Base1",
                "Fu1");

            s_baseQuantity2 = BaseQuantity.DefineNewBaseQuantity(
                "Base2",
                "Fu2",
                s_testPrefix);

            Assert.AreEqual(1, s_baseQuantity1.BaseQuantityComposition.Composition.Count);
            Assert.AreEqual(1m, s_baseQuantity1.BaseQuantityComposition.Composition[s_baseQuantity1]);
            Assert.AreEqual("Base1", s_baseQuantity1.MaybeName);
            Assert.AreEqual("Fu1", s_baseQuantity1.FundamentalUnit.Name);
            Assert.IsInstanceOfType(s_baseQuantity1.FundamentalUnit, typeof(BaseUnit));
            Assert.AreEqual(1m, s_baseQuantity1.FundamentalUnit.FundamentalMultiplier);
            Assert.IsTrue(s_baseQuantity1.FundamentalUnit.IsFundamental);

            Assert.AreEqual(1, s_baseQuantity2.BaseQuantityComposition.Composition.Count);
            Assert.AreEqual(1m, s_baseQuantity2.BaseQuantityComposition.Composition[s_baseQuantity2]);
            Assert.AreEqual("Base2", s_baseQuantity2.MaybeName);
            Assert.AreEqual("TestPrefix_Fu2", s_baseQuantity2.FundamentalUnit.Name);
            Assert.IsInstanceOfType(s_baseQuantity2.FundamentalUnit, typeof(PrefixedBaseUnit));
            Assert.AreEqual(1m, s_baseQuantity2.FundamentalUnit.FundamentalMultiplier);
            Assert.IsTrue(s_baseQuantity2.FundamentalUnit.IsFundamental);

            var fu2 = (PrefixedBaseUnit)s_baseQuantity2.FundamentalUnit;
            Assert.AreEqual("TestPrefix", s_testPrefix.MaybeName);
            Assert.AreSame(s_testPrefix, fu2.Prefix);
            Assert.AreEqual(2m, fu2.Prefix.Multiplier);
            Assert.AreEqual(0.5m, fu2.Unit.FundamentalMultiplier);
        }

        [AssemblyCleanup]
        public static void CleanupAssembly()
        {
            MaybeNamed.ClearAll();
        }

        [TestMethod]
        public void TestMultiplication()
        {
            using var prod1 = s_baseQuantity1 * s_baseQuantity1;
            Assert.IsInstanceOfType(prod1, typeof(DerivedQuantity));
            Assert.AreEqual(
                s_baseQuantity1.BaseQuantityComposition * s_baseQuantity1.BaseQuantityComposition, 
                prod1.BaseQuantityComposition);

            using var prod2 = s_baseQuantity1 * s_baseQuantity2;
            Assert.IsInstanceOfType(prod2, typeof(DerivedQuantity));
            Assert.AreEqual(
                s_baseQuantity1.BaseQuantityComposition * s_baseQuantity2.BaseQuantityComposition,
                prod2.BaseQuantityComposition);

            using var prod3 = prod1 * prod2;
            Assert.IsInstanceOfType(prod3, typeof(DerivedQuantity));
            Assert.AreEqual(
                prod1.BaseQuantityComposition * prod2.BaseQuantityComposition,
                prod3.BaseQuantityComposition);
        }

        [TestMethod]
        public void TestDivision()
        {
            using var quotient1 = s_baseQuantity1 / s_baseQuantity2;
            Assert.IsInstanceOfType(quotient1, typeof(DerivedQuantity));
            Assert.AreEqual(
                s_baseQuantity1.BaseQuantityComposition / s_baseQuantity2.BaseQuantityComposition,
                quotient1.BaseQuantityComposition);

            using var quotient2 = s_baseQuantity2 / s_baseQuantity1;
            Assert.IsInstanceOfType(quotient2, typeof(DerivedQuantity));
            Assert.AreEqual(
                s_baseQuantity2.BaseQuantityComposition / s_baseQuantity1.BaseQuantityComposition,
                quotient2.BaseQuantityComposition);

            using var quotient3 = quotient1 / quotient2;
            Assert.IsInstanceOfType(quotient3, typeof(DerivedQuantity));
            Assert.AreEqual(
                quotient1.BaseQuantityComposition / quotient2.BaseQuantityComposition,
                quotient3.BaseQuantityComposition);
        }

        [TestMethod]
        public void TestMultiplicationAndDivision()
        {
            using var product1 = s_baseQuantity1 * s_baseQuantity1;
            var quotient1 = product1 / s_baseQuantity1;
            Assert.AreSame(s_baseQuantity1, quotient1);

            using var product2 = s_baseQuantity2 * s_baseQuantity2 * s_baseQuantity1;
            using var quotient2 = product2 / (s_baseQuantity1 * s_baseQuantity2);
            Assert.AreSame(s_baseQuantity2, quotient2);
        }

        [TestMethod]
        public void TestEmpty()
        {
            var quotient1 = s_baseQuantity1 / s_baseQuantity1;
            Assert.AreSame(Empty, quotient1);

            using var derived = s_baseQuantity1 * s_baseQuantity1 / s_baseQuantity2;
            var quotient2 = derived / derived;
            Assert.AreSame(Empty, quotient2);

            var product1 = Empty * s_baseQuantity1;
            Assert.AreSame(s_baseQuantity1, product1);

            var quotient3 = s_baseQuantity2 / Empty;
            Assert.AreSame(s_baseQuantity2, quotient3);
        }
    }
}

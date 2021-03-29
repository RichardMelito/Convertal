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
    public class TestQuantity : BaseTestClass
    {
        readonly Prefix _testPrefix = new("TestPrefix", 2);
        readonly BaseQuantity _baseQuantity1;
        readonly BaseQuantity _baseQuantity2;

        public TestQuantity()
        {
            _baseQuantity1 = BaseQuantity.DefineNewBaseQuantity(
                "Base1",
                "Fu1");

            _baseQuantity2 = BaseQuantity.DefineNewBaseQuantity(
                "Base2",
                "Fu2",
                _testPrefix);

            Assert.AreEqual(1, _baseQuantity1.BaseQuantityComposition.Composition.Count);
            Assert.AreEqual(1m, _baseQuantity1.BaseQuantityComposition.Composition[_baseQuantity1]);
            Assert.AreEqual("Base1", _baseQuantity1.MaybeName);
            Assert.AreEqual("Fu1", _baseQuantity1.FundamentalUnit.Name);
            Assert.IsInstanceOfType(_baseQuantity1.FundamentalUnit, typeof(BaseUnit));
            Assert.AreEqual(1m, _baseQuantity1.FundamentalUnit.FundamentalMultiplier);
            Assert.IsTrue(_baseQuantity1.FundamentalUnit.IsFundamental);

            Assert.AreEqual(1, _baseQuantity2.BaseQuantityComposition.Composition.Count);
            Assert.AreEqual(1m, _baseQuantity2.BaseQuantityComposition.Composition[_baseQuantity2]);
            Assert.AreEqual("Base2", _baseQuantity2.MaybeName);
            Assert.AreEqual("TestPrefix_Fu2", _baseQuantity2.FundamentalUnit.Name);
            Assert.IsInstanceOfType(_baseQuantity2.FundamentalUnit, typeof(PrefixedBaseUnit));
            Assert.AreEqual(1m, _baseQuantity2.FundamentalUnit.FundamentalMultiplier);
            Assert.IsTrue(_baseQuantity2.FundamentalUnit.IsFundamental);

            var fu2 = (PrefixedBaseUnit)_baseQuantity2.FundamentalUnit;
            Assert.AreEqual("TestPrefix", _testPrefix.MaybeName);
            Assert.AreSame(_testPrefix, fu2.Prefix);
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
            var prod1 = _baseQuantity1 * _baseQuantity1;
            Assert.IsInstanceOfType(prod1, typeof(DerivedQuantity));
            Assert.AreEqual(
                _baseQuantity1.BaseQuantityComposition * _baseQuantity1.BaseQuantityComposition, 
                prod1.BaseQuantityComposition);

            var prod2 = _baseQuantity1 * _baseQuantity2;
            Assert.IsInstanceOfType(prod2, typeof(DerivedQuantity));
            Assert.AreEqual(
                _baseQuantity1.BaseQuantityComposition * _baseQuantity2.BaseQuantityComposition,
                prod2.BaseQuantityComposition);

            var prod3 = prod1 * prod2;
            Assert.IsInstanceOfType(prod3, typeof(DerivedQuantity));
            Assert.AreEqual(
                prod1.BaseQuantityComposition * prod2.BaseQuantityComposition,
                prod3.BaseQuantityComposition);
        }

        [TestMethod]
        public void TestDivision()
        {
            var quotient1 = _baseQuantity1 / _baseQuantity2;
            Assert.IsInstanceOfType(quotient1, typeof(DerivedQuantity));
            Assert.AreEqual(
                _baseQuantity1.BaseQuantityComposition / _baseQuantity2.BaseQuantityComposition,
                quotient1.BaseQuantityComposition);

            var quotient2 = _baseQuantity2 / _baseQuantity1;
            Assert.IsInstanceOfType(quotient2, typeof(DerivedQuantity));
            Assert.AreEqual(
                _baseQuantity2.BaseQuantityComposition / _baseQuantity1.BaseQuantityComposition,
                quotient2.BaseQuantityComposition);

            var quotient3 = quotient1 / quotient2;
            Assert.IsInstanceOfType(quotient3, typeof(DerivedQuantity));
            Assert.AreEqual(
                quotient1.BaseQuantityComposition / quotient2.BaseQuantityComposition,
                quotient3.BaseQuantityComposition);
        }

        [TestMethod]
        public void TestMultiplicationAndDivision()
        {
            var product1 = _baseQuantity1 * _baseQuantity1;
            var quotient1 = product1 / _baseQuantity1;
            Assert.AreSame(_baseQuantity1, quotient1);

            var product2 = _baseQuantity2 * _baseQuantity2 * _baseQuantity1;
            var quotient2 = product2 / (_baseQuantity1 * _baseQuantity2);
            Assert.AreSame(_baseQuantity2, quotient2);
        }

        [TestMethod]
        public void TestEmpty()
        {
            var quotient1 = _baseQuantity1 / _baseQuantity1;
            Assert.AreSame(Empty, quotient1);

            var derived = _baseQuantity1 * _baseQuantity1 / _baseQuantity2;
            var quotient2 = derived / derived;
            Assert.AreSame(Empty, quotient2);

            var product1 = Empty * _baseQuantity1;
            Assert.AreSame(_baseQuantity1, product1);

            var quotient3 = _baseQuantity2 / Empty;
            Assert.AreSame(_baseQuantity2, quotient3);
        }
    }
}

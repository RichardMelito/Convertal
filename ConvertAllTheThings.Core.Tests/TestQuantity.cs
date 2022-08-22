using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core;

namespace ConvertAllTheThings.Core.Tests
{
    public class TestQuantity : BaseTestClass
    {
        readonly Prefix _testPrefix;
        readonly BaseQuantity _baseQuantity1;
        readonly BaseQuantity _baseQuantity2;

        public TestQuantity()
        {
            _testPrefix = new(Database, "TestPrefix", 2);

            _baseQuantity1 = Database.DefineBaseQuantity(
                "Base1",
                "Fu1");

            _baseQuantity2 = Database.DefineBaseQuantity(
                "Base2",
                "Fu2",
                _testPrefix);

            Assert.Equal(1, _baseQuantity1.BaseQuantityComposition.Composition.Count);
            Assert.Equal(1m, _baseQuantity1.BaseQuantityComposition.Composition[_baseQuantity1]);
            Assert.Equal("Base1", _baseQuantity1.MaybeName);
            Assert.Equal("Fu1", _baseQuantity1.FundamentalUnit.Name);
            Assert.IsType<BaseUnit>(_baseQuantity1.FundamentalUnit);
            Assert.Equal(1m, _baseQuantity1.FundamentalUnit.FundamentalMultiplier);
            Assert.True(_baseQuantity1.FundamentalUnit.IsFundamental);

            Assert.Equal(1, _baseQuantity2.BaseQuantityComposition.Composition.Count);
            Assert.Equal(1m, _baseQuantity2.BaseQuantityComposition.Composition[_baseQuantity2]);
            Assert.Equal("Base2", _baseQuantity2.MaybeName);
            Assert.Equal("TestPrefix_Fu2", _baseQuantity2.FundamentalUnit.Name);
            Assert.IsType<PrefixedBaseUnit>(_baseQuantity2.FundamentalUnit);
            Assert.Equal(1m, _baseQuantity2.FundamentalUnit.FundamentalMultiplier);
            Assert.True(_baseQuantity2.FundamentalUnit.IsFundamental);

            var fu2 = (PrefixedBaseUnit)_baseQuantity2.FundamentalUnit;
            Assert.Equal("TestPrefix", _testPrefix.MaybeName);
            Assert.Same(_testPrefix, fu2.Prefix);
            Assert.Equal(2m, fu2.Prefix.Multiplier);
            Assert.Equal(0.5m, fu2.Unit.FundamentalMultiplier);
        }

        [Fact]
        public void TestMultiplication()
        {
            var prod1 = _baseQuantity1 * _baseQuantity1;
            Assert.IsType<DerivedQuantity>(prod1);
            Assert.Equal(
                _baseQuantity1.BaseQuantityComposition * _baseQuantity1.BaseQuantityComposition, 
                prod1.BaseQuantityComposition);

            var prod2 = _baseQuantity1 * _baseQuantity2;
            Assert.IsType<DerivedQuantity>(prod2);
            Assert.Equal(
                _baseQuantity1.BaseQuantityComposition * _baseQuantity2.BaseQuantityComposition,
                prod2.BaseQuantityComposition);

            var prod3 = prod1 * prod2;
            Assert.IsType<DerivedQuantity>(prod3);
            Assert.Equal(
                prod1.BaseQuantityComposition * prod2.BaseQuantityComposition,
                prod3.BaseQuantityComposition);
        }

        [Fact]
        public void TestDivision()
        {
            var quotient1 = _baseQuantity1 / _baseQuantity2;
            Assert.IsType<DerivedQuantity>(quotient1);
            Assert.Equal(
                _baseQuantity1.BaseQuantityComposition / _baseQuantity2.BaseQuantityComposition,
                quotient1.BaseQuantityComposition);

            var quotient2 = _baseQuantity2 / _baseQuantity1;
            Assert.IsType<DerivedQuantity>(quotient2);
            Assert.Equal(
                _baseQuantity2.BaseQuantityComposition / _baseQuantity1.BaseQuantityComposition,
                quotient2.BaseQuantityComposition);

            var quotient3 = quotient1 / quotient2;
            Assert.IsType<DerivedQuantity>(quotient3);
            Assert.Equal(
                quotient1.BaseQuantityComposition / quotient2.BaseQuantityComposition,
                quotient3.BaseQuantityComposition);
        }

        [Fact]
        public void TestMultiplicationAndDivision()
        {
            var product1 = _baseQuantity1 * _baseQuantity1;
            var quotient1 = product1 / _baseQuantity1;
            Assert.Same(_baseQuantity1, quotient1);

            var product2 = _baseQuantity2 * _baseQuantity2 * _baseQuantity1;
            var quotient2 = product2 / (_baseQuantity1 * _baseQuantity2);
            Assert.Same(_baseQuantity2, quotient2);
        }

        [Fact]
        public void TestEmpty()
        {
            var quotient1 = _baseQuantity1 / _baseQuantity1;
            Assert.Same(Database.EmptyQuantity, quotient1);

            var derived = _baseQuantity1 * _baseQuantity1 / _baseQuantity2;
            var quotient2 = derived / derived;
            Assert.Same(Database.EmptyQuantity, quotient2);

            var product1 = Database.EmptyQuantity * _baseQuantity1;
            Assert.Same(_baseQuantity1, product1);

            var quotient3 = _baseQuantity2 / Database.EmptyQuantity;
            Assert.Same(_baseQuantity2, quotient3);
        }
    }
}

// Created by Richard Melito and licensed to you under The Clear BSD License.

using Xunit;
using FluentAssertions;

namespace Convertal.Core.Tests;

public class TestQuantity : BaseTestClass
{
    readonly Prefix _testPrefix;
    readonly ScalarBaseQuantity _baseQuantity1;
    readonly ScalarBaseQuantity _baseQuantity2;

    public TestQuantity()
    {
        _testPrefix = new(Database, "TestPrefix", 2);

        _baseQuantity1 = Database.DefineScalarBaseQuantity(
            "Base1",
            "Fu1");

        _baseQuantity2 = Database.DefineScalarBaseQuantity(
            "Base2",
            "Fu2",
            _testPrefix);

        _baseQuantity1.BaseQuantityComposition.Should().ContainSingle();
        _baseQuantity1.BaseQuantityComposition[_baseQuantity1].Should().Be(1m);
        _baseQuantity1.Name.Should().Be("Base1");
        _baseQuantity1.FundamentalUnit.Name.Should().Be("Fu1");
        _baseQuantity1.FundamentalUnit.Should().BeOfType<ScalarBaseUnit>();
        _baseQuantity1.FundamentalUnit.FundamentalMultiplier.Should().Be(1m);
        _baseQuantity1.FundamentalUnit.IsFundamental.Should().BeTrue();

        _baseQuantity2.BaseQuantityComposition.Should().ContainSingle();
        _baseQuantity2.BaseQuantityComposition[_baseQuantity2].Should().Be(1m);
        _baseQuantity2.Name.Should().Be("Base2");
        _baseQuantity2.FundamentalUnit.Name.Should().Be("TestPrefix_Fu2");
        _baseQuantity2.FundamentalUnit.Should().BeOfType<ScalarPrefixedBaseUnit>();
        _baseQuantity2.FundamentalUnit.FundamentalMultiplier.Should().Be(1m);
        _baseQuantity2.FundamentalUnit.IsFundamental.Should().BeTrue();

        var fu2 = (ScalarPrefixedBaseUnit)_baseQuantity2.FundamentalUnit;
        _testPrefix.Name.Should().Be("TestPrefix");
        fu2.Prefix.Should().BeSameAs(_testPrefix);
        fu2.Prefix.Multiplier.Should().Be(2m);
        fu2.Unit.FundamentalMultiplier.Should().Be(0.5m);
    }

    [Fact]
    public void TestMultiplication()
    {
        var prod1 = _baseQuantity1 * _baseQuantity1;
        prod1.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            _baseQuantity1.BaseQuantityComposition * _baseQuantity1.BaseQuantityComposition,
            prod1.BaseQuantityComposition);

        var prod2 = _baseQuantity1 * _baseQuantity2;
        prod2.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            _baseQuantity1.BaseQuantityComposition * _baseQuantity2.BaseQuantityComposition,
            prod2.BaseQuantityComposition);

        var prod3 = prod1 * prod2;
        prod3.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            prod1.BaseQuantityComposition * prod2.BaseQuantityComposition,
            prod3.BaseQuantityComposition);
    }

    [Fact]
    public void TestDivision()
    {
        var quotient1 = _baseQuantity1 / _baseQuantity2;
        quotient1.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            _baseQuantity1.BaseQuantityComposition / _baseQuantity2.BaseQuantityComposition,
            quotient1.BaseQuantityComposition);

        var quotient2 = _baseQuantity2 / _baseQuantity1;
        quotient2.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            _baseQuantity2.BaseQuantityComposition / _baseQuantity1.BaseQuantityComposition,
            quotient2.BaseQuantityComposition);

        var quotient3 = quotient1 / quotient2;
        quotient3.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            quotient1.BaseQuantityComposition / quotient2.BaseQuantityComposition,
            quotient3.BaseQuantityComposition);
    }

    [Fact]
    public void TestMultiplicationAndDivision()
    {
        var product1 = _baseQuantity1 * _baseQuantity1;
        var quotient1 = product1 / _baseQuantity1;
        _baseQuantity1.Should().BeSameAs(quotient1);

        var product2 = _baseQuantity2 * _baseQuantity2 * _baseQuantity1;
        var quotient2 = product2 / (_baseQuantity1 * _baseQuantity2);
        _baseQuantity2.Should().BeSameAs(quotient2);
    }

    [Fact]
    public void TestEmpty()
    {
        var quotient1 = _baseQuantity1 / _baseQuantity1;
        quotient1.Should().BeSameAs(Database.ScalarEmptyQuantity);

        var derived = _baseQuantity1 * _baseQuantity1 / _baseQuantity2;
        var quotient2 = derived / derived;
        quotient2.Should().BeSameAs(Database.ScalarEmptyQuantity);

        var product1 = Database.ScalarEmptyQuantity * _baseQuantity1;
        product1.Should().BeSameAs(_baseQuantity1);

        var quotient3 = _baseQuantity2 / Database.ScalarEmptyQuantity;
        quotient3.Should().BeSameAs(_baseQuantity2);
    }
}

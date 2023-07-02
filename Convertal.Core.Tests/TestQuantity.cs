// Created by Richard Melito and licensed to you under The Clear BSD License.

using Xunit;
using FluentAssertions;

namespace Convertal.Core.Tests;

public class TestQuantity : BaseTestClass
{
    readonly Prefix _testPrefix;
    readonly ScalarBaseQuantity _scalarBaseQuantity1;
    readonly ScalarBaseQuantity _scalarBaseQuantity2;
    readonly VectorBaseQuantity _vectorBaseQuantity1;

    public TestQuantity()
    {
        _testPrefix = new(Database, "TestPrefix", 2);

        _scalarBaseQuantity1 = Database.DefineScalarBaseQuantity(
            "ScalarBase1",
            "Fu1");

        _scalarBaseQuantity2 = Database.DefineScalarBaseQuantity(
            "ScalarBase2",
            "Fu2",
            _testPrefix);

        _vectorBaseQuantity1 = Database.DefineVectorBaseQuantity(_scalarBaseQuantity1, "VectorBase1");

        _scalarBaseQuantity1.BaseQuantityComposition.Should().ContainSingle();
        _scalarBaseQuantity1.BaseQuantityComposition[_scalarBaseQuantity1].Should().Be(1m);
        _scalarBaseQuantity1.Name.Should().Be("ScalarBase1");
        _scalarBaseQuantity1.FundamentalUnit.Name.Should().Be("Fu1");
        _scalarBaseQuantity1.FundamentalUnit.Should().BeOfType<ScalarBaseUnit>();
        _scalarBaseQuantity1.FundamentalUnit.FundamentalMultiplier.Should().Be(1m);
        _scalarBaseQuantity1.FundamentalUnit.IsFundamental.Should().BeTrue();

        _scalarBaseQuantity2.BaseQuantityComposition.Should().ContainSingle();
        _scalarBaseQuantity2.BaseQuantityComposition[_scalarBaseQuantity2].Should().Be(1m);
        _scalarBaseQuantity2.Name.Should().Be("ScalarBase2");
        _scalarBaseQuantity2.FundamentalUnit.Name.Should().Be("TestPrefix_Fu2");
        _scalarBaseQuantity2.FundamentalUnit.Should().BeOfType<ScalarPrefixedBaseUnit>();
        _scalarBaseQuantity2.FundamentalUnit.FundamentalMultiplier.Should().Be(1m);
        _scalarBaseQuantity2.FundamentalUnit.IsFundamental.Should().BeTrue();

        var fu2 = (ScalarPrefixedBaseUnit)_scalarBaseQuantity2.FundamentalUnit;
        _testPrefix.Name.Should().Be("TestPrefix");
        fu2.Prefix.Should().BeSameAs(_testPrefix);
        fu2.Prefix.Multiplier.Should().Be(2m);
        fu2.Unit.FundamentalMultiplier.Should().Be(0.5m);
    }

    [Fact]
    public void TestScalarMultiplication()
    {
        using var prod1 = _scalarBaseQuantity1 * _scalarBaseQuantity1;
        prod1.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            _scalarBaseQuantity1.BaseQuantityComposition * _scalarBaseQuantity1.BaseQuantityComposition,
            prod1.BaseQuantityComposition);

        using var prod2 = _scalarBaseQuantity1 * _scalarBaseQuantity2;
        prod2.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            _scalarBaseQuantity1.BaseQuantityComposition * _scalarBaseQuantity2.BaseQuantityComposition,
            prod2.BaseQuantityComposition);

        using var prod3 = prod1 * prod2;
        prod3.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            prod1.BaseQuantityComposition * prod2.BaseQuantityComposition,
            prod3.BaseQuantityComposition);
    }

    [Fact]
    public void TestVectorMultiplication()
    {
        using var prod1 = _scalarBaseQuantity1 * _vectorBaseQuantity1;
        prod1.Should().BeOfType<VectorDerivedQuantity>();
        prod1.BaseQuantityComposition.Should().BeEquivalentTo(new Dictionary<IBaseQuantity, decimal>()
        {
            [_scalarBaseQuantity1] = 1m,
            [_vectorBaseQuantity1] = 1m,
        });

        using var prod2 = prod1 * _scalarBaseQuantity1 * _scalarBaseQuantity2;
        prod2.Should().BeOfType<VectorDerivedQuantity>();
        prod2.BaseQuantityComposition.Should().BeEquivalentTo(new Dictionary<IBaseQuantity, decimal>()
        {
            [_scalarBaseQuantity1] = 2m,
            [_scalarBaseQuantity2] = 1m,
            [_vectorBaseQuantity1] = 1m,
        });

        using var dotProd1 = prod2 * prod1;
        dotProd1.Should().BeOfType<ScalarDerivedQuantity>();
        dotProd1.BaseQuantityComposition.Should().BeEquivalentTo(new Dictionary<IBaseQuantity, decimal>()
        {
            [_scalarBaseQuantity1] = 5m,
            [_scalarBaseQuantity2] = 1m,
        });

        using var crossProd1 = _vectorBaseQuantity1 & _vectorBaseQuantity1;
        crossProd1.Should().BeOfType<VectorDerivedQuantity>();
        crossProd1.BaseQuantityComposition.Should().BeEquivalentTo(new Dictionary<IBaseQuantity, decimal>()
        {
            [_scalarBaseQuantity1] = 1m,
            [_vectorBaseQuantity1] = 1m,
        });

        using var crossProd2 = _vectorBaseQuantity1 & crossProd1;
        crossProd2.Should().BeOfType<VectorDerivedQuantity>();
        crossProd2.BaseQuantityComposition.Should().BeEquivalentTo(new Dictionary<IBaseQuantity, decimal>()
        {
            [_scalarBaseQuantity1] = 2m,
            [_vectorBaseQuantity1] = 1m,
        });

        using var dotProd2 = crossProd1 * crossProd2;
        dotProd2.Should().BeOfType<ScalarDerivedQuantity>();
        dotProd2.BaseQuantityComposition.Should().BeEquivalentTo(new Dictionary<IBaseQuantity, decimal>()
        {
            [_scalarBaseQuantity1] = 5m,
        });
    }

    [Fact]
    public void TestScalarDivision()
    {
        using var quotient1 = _scalarBaseQuantity1 / _scalarBaseQuantity2;
        quotient1.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            _scalarBaseQuantity1.BaseQuantityComposition / _scalarBaseQuantity2.BaseQuantityComposition,
            quotient1.BaseQuantityComposition);

        using var quotient2 = _scalarBaseQuantity2 / _scalarBaseQuantity1;
        quotient2.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            _scalarBaseQuantity2.BaseQuantityComposition / _scalarBaseQuantity1.BaseQuantityComposition,
            quotient2.BaseQuantityComposition);

        using var quotient3 = quotient1 / quotient2;
        quotient3.Should().BeOfType<ScalarDerivedQuantity>();
        Assert.Equal(
            quotient1.BaseQuantityComposition / quotient2.BaseQuantityComposition,
            quotient3.BaseQuantityComposition);
    }

    [Fact]
    public void TestVectorDivision()
    {
        using var quotient1 = _vectorBaseQuantity1 / _scalarBaseQuantity1;
        quotient1.Should().BeOfType<VectorEmptyQuantity>();
        quotient1.BaseQuantityComposition.Should().BeEmpty();

        using var quotient2 = _vectorBaseQuantity1 / _scalarBaseQuantity2;
        quotient2.Should().BeOfType<VectorDerivedQuantity>();
        quotient2.BaseQuantityComposition.Should().BeEquivalentTo(new Dictionary<IBaseQuantity, decimal>()
        {
            [_scalarBaseQuantity2] = -1m,
            [_vectorBaseQuantity1] = 1m,
        });

        using var prod = quotient2 * _scalarBaseQuantity2;
        prod.Should().BeOfType<VectorBaseQuantity>();
        prod.BaseQuantityComposition.Should().BeEquivalentTo(new Dictionary<IBaseQuantity, decimal>()
        {
            [_vectorBaseQuantity1] = 1m,
        });
    }

    [Fact]
    public void TestScalarMultiplicationAndDivision()
    {
        using var product1 = _scalarBaseQuantity1 * _scalarBaseQuantity1;
        using var quotient1 = product1 / _scalarBaseQuantity1;
        _scalarBaseQuantity1.Should().BeSameAs(quotient1);

        using var product2 = _scalarBaseQuantity2 * _scalarBaseQuantity2 * _scalarBaseQuantity1;
        using var quotient2 = product2 / (_scalarBaseQuantity1 * _scalarBaseQuantity2);
        _scalarBaseQuantity2.Should().BeSameAs(quotient2);
    }

    [Fact]
    public void TestScalarEmpty()
    {
        using var quotient1 = _scalarBaseQuantity1 / _scalarBaseQuantity1;
        quotient1.Should().BeSameAs(Database.ScalarEmptyQuantity);

        using var derived = _scalarBaseQuantity1 * _scalarBaseQuantity1 / _scalarBaseQuantity2;
        using var quotient2 = derived / derived;
        quotient2.Should().BeSameAs(Database.ScalarEmptyQuantity);

        using var product1 = Database.ScalarEmptyQuantity * _scalarBaseQuantity1;
        product1.Should().BeSameAs(_scalarBaseQuantity1);

        using var quotient3 = _scalarBaseQuantity2 / Database.ScalarEmptyQuantity;
        quotient3.Should().BeSameAs(_scalarBaseQuantity2);
    }

    [Fact]
    public void TestVectorEmpty()
    {
        using var quotient1 = _vectorBaseQuantity1 / _scalarBaseQuantity1;
        quotient1.Should().BeOfType<VectorEmptyQuantity>();
        quotient1.BaseQuantityComposition.Should().BeEmpty();

        quotient1.Should().BeSameAs(Database.VectorEmptyQuantity);
        Database.VectorEmptyQuantity.ScalarAnalog.Should().Be(Database.ScalarEmptyQuantity);
        Database.ScalarEmptyQuantity.VectorAnalog.Should().Be(Database.VectorEmptyQuantity);

        using var product1 = _vectorBaseQuantity1 * Database.ScalarEmptyQuantity;
        product1.Should().BeSameAs(_vectorBaseQuantity1);

        using var product2 = _vectorBaseQuantity1 & Database.VectorEmptyQuantity;
        product2.Should().BeSameAs(_vectorBaseQuantity1);

        using var product3 = _vectorBaseQuantity1 * Database.VectorEmptyQuantity;
        product3.Should().BeSameAs(_scalarBaseQuantity1);

        using var product4 = _scalarBaseQuantity1 * Database.VectorEmptyQuantity;
        product4.Should().BeSameAs(_vectorBaseQuantity1);

        using var vectorEmpty = Database.VectorEmptyQuantity & Database.VectorEmptyQuantity;
        vectorEmpty.Should().BeSameAs(Database.VectorEmptyQuantity);

        using var scalarEmpty = Database.VectorEmptyQuantity * Database.VectorEmptyQuantity;
        scalarEmpty.Should().BeSameAs(Database.ScalarEmptyQuantity);

        (scalarEmpty * vectorEmpty).Should().BeSameAs(vectorEmpty);
    }
}

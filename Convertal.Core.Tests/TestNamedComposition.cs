// Created by Richard Melito and licensed to you under The Clear BSD License.

using Convertal.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace Convertal.Core.Tests;
abstract class AbstractSimBase : MaybeNamed, IBase, IEquatable<AbstractSimBase>
{
    private static int s_id = -1;

    public int Id { get; }
    public abstract bool IsVector { get; }

    public AbstractSimBase(Database database)
        : base(database, "SimBase" + (++s_id))
    {
        Id = s_id;
    }

    public AbstractSimBase(Database database, string name)
        : base(database, name)
    {
        Id = ++s_id;
    }

    public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        return Array.Empty<IMaybeNamed>().SortByTypeAndName();
    }

    //public ScalarComposition<AbstractSimBase> MakeComposition()
    //{
    //    return new ScalarComposition<AbstractSimBase>(this);
    //}

    public bool Equals(AbstractSimBase? other)
    {
        return base.Equals(other);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as AbstractSimBase);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override MaybeNamedProto ToProto()
    {
        throw new NotImplementedException();
    }

    public abstract IVectorOrScalar ToScalar();
    public abstract IVectorOrScalar? ToVector();
}

class ScalarSimBase : AbstractSimBase
{
    public VectorSimBase? VectorAnalog { get; set; }
    public ScalarSimBase(Database database) : base(database)
    {
    }

    public ScalarSimBase(Database database, string name) : base(database, name)
    {
    }

    public override bool IsVector => false;

    public override IVectorOrScalar ToScalar() => this;
    public override IVectorOrScalar? ToVector() => VectorAnalog;
}

class VectorSimBase : AbstractSimBase
{
    public ScalarSimBase ScalarAnalog { get; }
    public VectorSimBase(ScalarSimBase scalarAnalog) : base(scalarAnalog.Database)
    {
        ScalarAnalog = scalarAnalog;
        ScalarAnalog.VectorAnalog = this;
    }

    public override bool IsVector => true;

    public override IVectorOrScalar ToScalar() => ScalarAnalog;
    public override IVectorOrScalar? ToVector() => this;
}



public class TestNamedComposition : BaseTestClass
{

    [Fact]
    public void ScalarComposition_ConstructionGivenScalarKey_Works()
    {
        ScalarSimBase scalarKey = new(Database);
        ScalarComposition<AbstractSimBase> toTest = new(scalarKey);
        toTest.Should().ContainSingle();
        toTest[scalarKey].Should().Be(1m);
    }

    [Fact]
    public void ScalarComposition_ConstructionGivenVectorKey_Fails()
    {
        ScalarSimBase scalarKey = new(Database);
        VectorSimBase vectorKey = new(scalarKey);
        Assert.Throws<InvalidOperationException>(() => new ScalarComposition<AbstractSimBase>(vectorKey));
    }

    [Fact]
    public void VectorComposition_ConstructionGivenVectorKey_Works()
    {
        ScalarSimBase scalarKey = new(Database);
        VectorSimBase vectorKey = new(scalarKey);
        VectorComposition<AbstractSimBase> toTest = new(vectorKey);
        toTest.Should().ContainSingle();
        toTest[vectorKey].Should().Be(1m);
    }

    [Fact]
    public void VectorComposition_ConstructionGivenScalarKey_Fails()
    {
        ScalarSimBase scalarKey = new(Database);;
        Assert.Throws<InvalidOperationException>(() => new VectorComposition<AbstractSimBase>(scalarKey));
    }

    [Fact]
    public void ScalarComposition_ToScalar_ReturnsSelf()
    {
        ScalarSimBase scalarKey = new(Database);
        ScalarComposition<AbstractSimBase> toTest = new(scalarKey);
        toTest.ToScalar().Should().BeSameAs(toTest);
    }

    [Fact]
    public void ScalarComposition_ToVectorGivenKeyHasNoVectorAnalog_IsNull()
    {
        ScalarSimBase scalarKey = new(Database);
        scalarKey.VectorAnalog.Should().BeNull();
        ScalarComposition<AbstractSimBase> scalarComp = new(scalarKey);

        var vectorComp = scalarComp.ToVector();
        vectorComp.Should().BeNull();
    }

    [Fact]
    public void ScalarComposition_ToVectorGivenKeyHasVectorAnalog_RountripsWithToScalar()
    {
        ScalarSimBase scalarKey = new(Database);
        VectorSimBase vectorKey = new(scalarKey);
        scalarKey.VectorAnalog.Should().Be(vectorKey);
        vectorKey.ScalarAnalog.Should().Be(scalarKey);
        ScalarComposition<AbstractSimBase> scalarComp = new(scalarKey);

        var vectorComp = scalarComp.ToVector()!;
        vectorComp.Should().ContainSingle();
        vectorComp[vectorKey].Should().Be(1m);

        var scalarComp2 = vectorComp.ToScalar();
        scalarComp2.Should().BeEquivalentTo(scalarComp);
    }

    [Fact]
    public void ScalarComposition_Multiplication_Works()
    {
        ScalarSimBase lhsKey = new(Database);
        ScalarSimBase rhsKey = new(Database);

        ScalarComposition<AbstractSimBase> lhs = new(lhsKey);
        ScalarComposition<AbstractSimBase> rhs = new(rhsKey);

        var product = lhs * rhs;
        product.Should().BeOfType<ScalarComposition<AbstractSimBase>>();

        product.Should().HaveCount(2);
        product[lhsKey].Should().Be(1m);
        product[rhsKey].Should().Be(1m);

        product *= lhs;
        product.Should().HaveCount(2);
        product[lhsKey].Should().Be(2m);
        product[rhsKey].Should().Be(1m);

        product *= product;
        product.Should().HaveCount(2);
        product[lhsKey].Should().Be(4m);
        product[rhsKey].Should().Be(2m);

        ScalarSimBase thirdKey = new(Database);
        ScalarComposition<AbstractSimBase> thirdFactor = new(thirdKey);
        product *= thirdFactor;
        Assert.Equal(3, product.Count);
        Assert.Equal(4m, product[lhsKey]);
        Assert.Equal(2m, product[rhsKey]);
        Assert.Equal(1m, product[thirdKey]);
    }

    [Fact]
    public void ScalarComposition_Division_Works()
    {
        ScalarSimBase lhsKey = new(Database);
        ScalarSimBase rhsKey = new(Database);

        ScalarComposition<AbstractSimBase> lhs = new(lhsKey);
        ScalarComposition<AbstractSimBase> rhs = new(rhsKey);

        var quotient = lhs / rhs;
        quotient.Should().BeOfType<ScalarComposition<AbstractSimBase>>();

        quotient.Should().HaveCount(2);
        quotient[lhsKey].Should().Be(1m);
        quotient[rhsKey].Should().Be(-1m);

        quotient /= lhs;
        quotient.Should().ContainSingle();
        quotient[rhsKey].Should().Be(-1m);

        quotient /= rhs;
        quotient.Should().ContainSingle();
        quotient[rhsKey].Should().Be(-2m);

        quotient /= lhs;
        quotient.Should().HaveCount(2);
        quotient[lhsKey].Should().Be(-1m);
        quotient[rhsKey].Should().Be(-2m);

        ScalarSimBase thirdKey = new(Database);
        ScalarComposition<AbstractSimBase> thirdFactor = new(thirdKey);
        quotient /= thirdFactor;
        quotient.Should().HaveCount(3);
        quotient[lhsKey].Should().Be(-1m);
        quotient[rhsKey].Should().Be(-2m);
        quotient[thirdKey].Should().Be(-1m);

        quotient /= quotient;
        quotient.Should().BeEmpty();
        quotient.Should().BeSameAs(ScalarComposition<AbstractSimBase>.Empty);
    }

    [Fact]
    public void ScalarComposition_MultiplicationAndDivision_Works()
    {
        ScalarSimBase lhsKey = new(Database);
        ScalarSimBase rhsKey = new(Database);

        ScalarComposition<AbstractSimBase> lhs = new(lhsKey);
        ScalarComposition<AbstractSimBase> rhs = new(rhsKey);

        var result = lhs * rhs * rhs * rhs / (lhs * lhs * lhs * rhs);
        result.Should().HaveCount(2);
        result[lhsKey].Should().Be(-2m);
        result[rhsKey].Should().Be(2m);
    }

    [Fact]
    public void ScalarComposition_EmptyOperations_Work()
    {
        var empty = ScalarComposition<AbstractSimBase>.Empty;
        var product = empty * empty;
        product.Should().BeSameAs(empty);

        var quotient = empty / empty;
        quotient.Should().BeSameAs(empty);

        ScalarSimBase notEmptyKey = new(Database);
        ScalarComposition<AbstractSimBase> notEmpty = new(notEmptyKey);

        product = notEmpty * empty;
        product.Should().BeEquivalentTo(notEmpty);
        product = empty * notEmpty;
        product.Should().BeEquivalentTo(notEmpty);

        quotient = notEmpty / empty;
        quotient.Should().BeEquivalentTo(notEmpty);
        quotient = empty / notEmpty;
        quotient.Should().ContainSingle();
        quotient[notEmptyKey].Should().Be(-1m);
    }

    [Fact]
    public void ScalarComposition_Equality_Works()
    {
        ScalarSimBase lhsKey = new(Database);
        ScalarSimBase rhsKey = new(Database);

        ScalarComposition<AbstractSimBase> lhs = new(lhsKey);
        ScalarComposition<AbstractSimBase> rhs = new(rhsKey);

        var sameResult1 = lhs * lhs / rhs;
        var sameResult2 = lhs / rhs * lhs;
        var sameResult3 = (ScalarComposition<AbstractSimBase>.Empty / rhs) * lhs * lhs;
        sameResult1.Should().NotBeSameAs(sameResult2);
        sameResult2.Should().NotBeSameAs(sameResult3);
        sameResult1.Should().NotBeSameAs(sameResult3);
        sameResult1.Should().Equal(sameResult2);
        sameResult2.Should().Equal(sameResult3);
        Assert.Equal(sameResult1, sameResult2);
        Assert.Equal(sameResult2, sameResult3);

        var differentResult1 = lhs * lhs;
        var differentResult2 = lhs * rhs;
        var differentResult3 = lhs / lhs;
        differentResult1.Should().NotBeSameAs(differentResult2);
        differentResult2.Should().NotBeSameAs(differentResult3);
        differentResult3.Should().NotBeSameAs(differentResult1);
        differentResult1.Should().NotEqual(differentResult2);
        differentResult2.Should().NotEqual(differentResult3);
        differentResult3.Should().NotEqual(differentResult1);
        Assert.NotEqual(differentResult1, differentResult2);
        Assert.NotEqual(differentResult2, differentResult3);
        Assert.NotEqual(differentResult3, differentResult1);
    }

    //[Fact]
    //public void TestPow()
    //{
    //    AbstractSimBase lhsBase = new(Database);
    //    AbstractSimBase rhsBase = new(Database);

    //    var lhs = lhsBase.MakeComposition();
    //    var rhs = rhsBase.MakeComposition();

    //    var product = lhs * lhs * lhs / rhs;
    //    var squared = product.Pow(2);
    //    Assert.Equal(6m, squared[lhsBase]);
    //    Assert.Equal(-2m, squared[rhsBase]);

    //    var reciprocal = squared.Pow(-1);
    //    Assert.Equal(-6m, reciprocal[lhsBase]);
    //    Assert.Equal(2m, reciprocal[rhsBase]);

    //    var sqrRoot1 = reciprocal.Pow(0.5m);
    //    Assert.Equal(-3m, sqrRoot1[lhsBase]);
    //    Assert.Equal(1m, sqrRoot1[rhsBase]);

    //    var sqrRoot2 = sqrRoot1.Pow(0.5m);
    //    Assert.Equal(-1.5m, sqrRoot2[lhsBase]);
    //    Assert.Equal(0.5m, sqrRoot2[rhsBase]);
    //}

    //[Fact]
    //public void TestToString()
    //{
    //    AbstractSimBase simBase1 = new(Database, "sim1");
    //    AbstractSimBase simBase2 = new(Database, "sim2");
    //    AbstractSimBase simBase3 = new(Database, "sim3");

    //    var sim1 = simBase1.MakeComposition();
    //    var sim2 = simBase2.MakeComposition();
    //    var sim3 = simBase3.MakeComposition();

    //    var product = sim2 * sim1 * sim2 / sim3 / sim3;
    //    Assert.Equal(
    //        "(sim1^1)*(sim2^2)*(sim3^-2)",
    //        product.ToString());

    //    var fourthRoot = product.Pow(0.25m);
    //    Assert.Equal(
    //        "(sim1^0.25)*(sim2^0.5)*(sim3^-0.5)",
    //        fourthRoot.ToString());
    //}
}

// Created by Richard Melito and licensed to you under The Clear BSD License.

using ConvertAllTheThings.Core.Extensions;
using Xunit;

namespace ConvertAllTheThings.Core.Tests;

public class TestNamedComposition : BaseTestClass
{
    class SimBase : MaybeNamed, IBase, IEquatable<SimBase>
    {
        private static int s_id = -1;

        public int Id { get; }

        public SimBase(Database database)
            : base(database, "SimBase" + (++s_id))
        {
            Id = s_id;
        }

        public SimBase(Database database, string name)
            : base(database, name)
        {
            Id = ++s_id;
        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            return Array.Empty<IMaybeNamed>().SortByTypeAndName();
        }

        public NamedComposition<SimBase> MakeComposition()
        {
            return new NamedComposition<SimBase>(this);
        }

        public bool Equals(SimBase? other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SimBase);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override MaybeNamedProto ToProto()
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void TestConstruction()
    {
        SimBase simBase = new(Database);
        NamedComposition<SimBase> toTest = new(simBase);
        Assert.Equal(1, toTest.Count);
        Assert.True(toTest.ContainsKey(simBase));
        Assert.Equal(1m, toTest[simBase]);
    }

    [Fact]
    public void TestMultiplication()
    {
        SimBase lhsBase = new(Database);
        SimBase rhsBase = new(Database);

        var lhs = lhsBase.MakeComposition();
        var rhs = rhsBase.MakeComposition();

        var product = lhs * rhs;
        Assert.Equal(2, product.Count);
        Assert.Equal(1m, product[lhsBase]);
        Assert.Equal(1m, product[rhsBase]);

        product *= lhs;
        Assert.Equal(2, product.Count);
        Assert.Equal(2m, product[lhsBase]);
        Assert.Equal(1m, product[rhsBase]);

        product *= product;
        Assert.Equal(2, product.Count);
        Assert.Equal(4m, product[lhsBase]);
        Assert.Equal(2m, product[rhsBase]);

        SimBase thirdBase = new(Database);
        var thirdFactor = thirdBase.MakeComposition();
        product *= thirdFactor;
        Assert.Equal(3, product.Count);
        Assert.Equal(4m, product[lhsBase]);
        Assert.Equal(2m, product[rhsBase]);
        Assert.Equal(1m, product[thirdBase]);
    }

    [Fact]
    public void TestDivision()
    {
        SimBase lhsBase = new(Database);
        SimBase rhsBase = new(Database);

        var lhs = lhsBase.MakeComposition();
        var rhs = rhsBase.MakeComposition();

        var quotient = lhs / rhs;
        Assert.Equal(2, quotient.Count);
        Assert.Equal(1m, quotient[lhsBase]);
        Assert.Equal(-1m, quotient[rhsBase]);

        quotient /= lhs;
        Assert.Equal(1, quotient.Count);
        Assert.Equal(-1m, quotient[rhsBase]);

        quotient /= rhs;
        Assert.Equal(1, quotient.Count);
        Assert.Equal(-2m, quotient[rhsBase]);

        quotient /= lhs;
        Assert.Equal(2, quotient.Count);
        Assert.Equal(-1m, quotient[lhsBase]);
        Assert.Equal(-2m, quotient[rhsBase]);

        SimBase thirdBase = new(Database);
        var thirdFactor = thirdBase.MakeComposition();
        quotient /= thirdFactor;
        Assert.Equal(3, quotient.Count);
        Assert.Equal(-1m, quotient[lhsBase]);
        Assert.Equal(-2m, quotient[rhsBase]);
        Assert.Equal(-1m, quotient[thirdBase]);

        quotient /= quotient;
        Assert.Equal(0, quotient.Count);
        Assert.Same(NamedComposition<SimBase>.Empty, quotient);
    }

    [Fact]
    public void TestMultiplicationAndDivision()
    {
        SimBase lhsBase = new(Database);
        SimBase rhsBase = new(Database);

        var lhs = lhsBase.MakeComposition();
        var rhs = rhsBase.MakeComposition();

        var product = lhs * rhs * rhs * rhs / (lhs * lhs * lhs * rhs);
        Assert.Equal(2, product.Count);
        Assert.Equal(-2m, product[lhsBase]);
        Assert.Equal(2m, product[rhsBase]);
    }

    [Fact]
    public void TestEmptyOperations()
    {
        var empty = NamedComposition<SimBase>.Empty;
        var product = empty * empty;
        Assert.Same(empty, product);

        var quotient = empty / empty;
        Assert.Same(empty, quotient);

        SimBase notEmptyBase = new(Database);
        var notEmpty = notEmptyBase.MakeComposition();

        product = notEmpty * empty;
        Assert.Equal(notEmpty, product);
        product = empty * notEmpty;
        Assert.Equal(notEmpty, product);

        quotient = notEmpty / empty;
        Assert.Equal(notEmpty, quotient);
        quotient = empty / notEmpty;
        Assert.Equal(1, quotient.Count);
        Assert.Equal(-1m, quotient[notEmptyBase]);
    }

    [Fact]
    public void TestEquality()
    {
        SimBase lhsBase = new(Database);
        SimBase rhsBase = new(Database);

        var lhs = lhsBase.MakeComposition();
        var rhs = rhsBase.MakeComposition();

        var sameResult1 = lhs * lhs / rhs;
        var sameResult2 = lhs / rhs * lhs;
        var sameResult3 = (NamedComposition<SimBase>.Empty / rhs) * lhs * lhs;
        Assert.NotSame(sameResult1, sameResult2);
        Assert.NotSame(sameResult2, sameResult3);
        Assert.NotSame(sameResult1, sameResult3);
        Assert.Equal(sameResult1, sameResult2);
        Assert.Equal(sameResult2, sameResult3);

        var differentResult1 = lhs * lhs;
        var differentResult2 = lhs * rhs;
        var differentResult3 = lhs / lhs;
        Assert.NotSame(differentResult1, differentResult2);
        Assert.NotSame(differentResult2, differentResult3);
        Assert.NotSame(differentResult1, differentResult3);
        Assert.NotEqual(differentResult1, differentResult2);
        Assert.NotEqual(differentResult2, differentResult3);
        Assert.NotEqual(differentResult1, differentResult3);
    }

    [Fact]
    public void TestPow()
    {
        SimBase lhsBase = new(Database);
        SimBase rhsBase = new(Database);

        var lhs = lhsBase.MakeComposition();
        var rhs = rhsBase.MakeComposition();

        var product = lhs * lhs * lhs / rhs;
        var squared = product.Pow(2);
        Assert.Equal(6m, squared[lhsBase]);
        Assert.Equal(-2m, squared[rhsBase]);

        var reciprocal = squared.Pow(-1);
        Assert.Equal(-6m, reciprocal[lhsBase]);
        Assert.Equal(2m, reciprocal[rhsBase]);

        var sqrRoot1 = reciprocal.Pow(0.5m);
        Assert.Equal(-3m, sqrRoot1[lhsBase]);
        Assert.Equal(1m, sqrRoot1[rhsBase]);

        var sqrRoot2 = sqrRoot1.Pow(0.5m);
        Assert.Equal(-1.5m, sqrRoot2[lhsBase]);
        Assert.Equal(0.5m, sqrRoot2[rhsBase]);
    }

    [Fact]
    public void TestToString()
    {
        SimBase simBase1 = new(Database, "sim1");
        SimBase simBase2 = new(Database, "sim2");
        SimBase simBase3 = new(Database, "sim3");

        var sim1 = simBase1.MakeComposition();
        var sim2 = simBase2.MakeComposition();
        var sim3 = simBase3.MakeComposition();

        var product = sim2 * sim1 * sim2 / sim3 / sim3;
        Assert.Equal(
            "(sim1^1)*(sim2^2)*(sim3^-2)",
            product.ToString());

        var fourthRoot = product.Pow(0.25m);
        Assert.Equal(
            "(sim1^0.25)*(sim2^0.5)*(sim3^-0.5)",
            fourthRoot.ToString());
    }
}

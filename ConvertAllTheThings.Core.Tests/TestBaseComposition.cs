using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConvertAllTheThings.Core;

namespace ConvertAllTheThings.Core.Tests
{
    [TestClass]
    public class TestBaseComposition
    {
        class SimBase : Named, IBase, IEquatable<SimBase>
        {
            private static int s_id = -1;

            public int Id { get; }

            static SimBase()
            {
                AddTypeToDictionary<SimBase>();
            }

            public SimBase()
                : base((++s_id).ToString(), "TEST")
            {
                Id = s_id;
            }

            public BaseComposition<SimBase> MakeBaseComposition()
            {
                return new BaseComposition<SimBase>(this);
            }

            public bool Equals(SimBase? other)
            {
                return base.Equals(other);
            }
        }

        [TestMethod]
        public void TestConstruction()
        {
            SimBase simBase = new();
            BaseComposition<SimBase> toTest = new(simBase);
            Assert.AreEqual(1, toTest.Composition.Count);
            Assert.IsTrue(toTest.Composition.ContainsKey(simBase));
            Assert.AreEqual(1m, toTest.Composition[simBase]);
        }

        [TestMethod]
        public void TestMultiplication()
        {
            SimBase lhsBase = new();
            SimBase rhsBase = new();

            var lhs = lhsBase.MakeBaseComposition();
            var rhs = rhsBase.MakeBaseComposition();

            var product = lhs * rhs;
            Assert.AreEqual(2, product.Composition.Count);
            Assert.AreEqual(1m, product.Composition[lhsBase]);
            Assert.AreEqual(1m, product.Composition[rhsBase]);

            product *= lhs;
            Assert.AreEqual(2, product.Composition.Count);
            Assert.AreEqual(2m, product.Composition[lhsBase]);
            Assert.AreEqual(1m, product.Composition[rhsBase]);

            product *= product;
            Assert.AreEqual(2, product.Composition.Count);
            Assert.AreEqual(4m, product.Composition[lhsBase]);
            Assert.AreEqual(2m, product.Composition[rhsBase]);

            SimBase thirdBase = new();
            var thirdFactor = thirdBase.MakeBaseComposition();
            product *= thirdFactor;
            Assert.AreEqual(3, product.Composition.Count);
            Assert.AreEqual(4m, product.Composition[lhsBase]);
            Assert.AreEqual(2m, product.Composition[rhsBase]);
            Assert.AreEqual(1m, product.Composition[thirdBase]);
        }

        [TestMethod]
        public void TestDivision()
        {
            SimBase lhsBase = new();
            SimBase rhsBase = new();

            var lhs = lhsBase.MakeBaseComposition();
            var rhs = rhsBase.MakeBaseComposition();

            var quotient = lhs / rhs;
            Assert.AreEqual(2, quotient.Composition.Count);
            Assert.AreEqual(1m, quotient.Composition[lhsBase]);
            Assert.AreEqual(-1m, quotient.Composition[rhsBase]);

            quotient /= lhs;
            Assert.AreEqual(1, quotient.Composition.Count);
            Assert.AreEqual(-1m, quotient.Composition[rhsBase]);

            quotient /= rhs;
            Assert.AreEqual(1, quotient.Composition.Count);
            Assert.AreEqual(-2m, quotient.Composition[rhsBase]);

            quotient /= lhs;
            Assert.AreEqual(2, quotient.Composition.Count);
            Assert.AreEqual(-1m, quotient.Composition[lhsBase]);
            Assert.AreEqual(-2m, quotient.Composition[rhsBase]);

            SimBase thirdBase = new();
            var thirdFactor = thirdBase.MakeBaseComposition();
            quotient /= thirdFactor;
            Assert.AreEqual(3, quotient.Composition.Count);
            Assert.AreEqual(-1m, quotient.Composition[lhsBase]);
            Assert.AreEqual(-2m, quotient.Composition[rhsBase]);
            Assert.AreEqual(-1m, quotient.Composition[thirdBase]);

            quotient /= quotient;
            Assert.AreEqual(0, quotient.Composition.Count);
            Assert.AreSame(BaseComposition<SimBase>.Empty, quotient);
        }

        [TestMethod]
        public void TestMultiplicationAndDivision()
        {
            SimBase lhsBase = new();
            SimBase rhsBase = new();

            var lhs = lhsBase.MakeBaseComposition();
            var rhs = rhsBase.MakeBaseComposition();

            var product = lhs * rhs * rhs * rhs / (lhs * lhs * lhs * rhs);
            Assert.AreEqual(2, product.Composition.Count);
            Assert.AreEqual(-2m, product.Composition[lhsBase]);
            Assert.AreEqual(2m, product.Composition[rhsBase]);
        }

        [TestMethod]
        public void TestEmptyOperations()
        {
            var empty = BaseComposition<SimBase>.Empty;
            var product = empty * empty;
            Assert.AreSame(empty, product);

            var quotient = empty / empty;
            Assert.AreSame(empty, quotient);

            SimBase notEmptyBase = new();
            var notEmpty = notEmptyBase.MakeBaseComposition();

            product = notEmpty * empty;
            Assert.AreEqual(notEmpty, product);
            product = empty * notEmpty;
            Assert.AreEqual(notEmpty, product);

            quotient = notEmpty / empty;
            Assert.AreEqual(notEmpty, quotient);
            quotient = empty / notEmpty;
            Assert.AreEqual(1, quotient.Composition.Count);
            Assert.AreEqual(-1m, quotient.Composition[notEmptyBase]);
        }

        [TestMethod]
        public void TestEquality()
        {
            SimBase lhsBase = new();
            SimBase rhsBase = new();

            var lhs = lhsBase.MakeBaseComposition();
            var rhs = rhsBase.MakeBaseComposition();

            var sameResult1 = lhs * lhs / rhs;
            var sameResult2 = lhs / rhs * lhs;
            var sameResult3 = (BaseComposition<SimBase>.Empty / rhs) * lhs * lhs;
            Assert.AreNotSame(sameResult1, sameResult2);
            Assert.AreNotSame(sameResult2, sameResult3);
            Assert.AreNotSame(sameResult1, sameResult3);
            Assert.AreEqual(sameResult1, sameResult2);
            Assert.AreEqual(sameResult2, sameResult3);

            var differentResult1 = lhs * lhs;
            var differentResult2 = lhs * rhs;
            var differentResult3 = lhs / lhs;
            Assert.AreNotSame(differentResult1, differentResult2);
            Assert.AreNotSame(differentResult2, differentResult3);
            Assert.AreNotSame(differentResult1, differentResult3);
            Assert.AreNotEqual(differentResult1, differentResult2);
            Assert.AreNotEqual(differentResult2, differentResult3);
            Assert.AreNotEqual(differentResult1, differentResult3);
        }
    }
}

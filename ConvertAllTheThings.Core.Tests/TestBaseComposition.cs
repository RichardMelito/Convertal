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

            public int CompareTo(SimBase? other)
            {
                return 0;
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
    }
}

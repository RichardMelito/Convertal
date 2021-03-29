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
    public class TestIUnit : BaseTestClass
    {
        [TestMethod]
        public void TestOffsets()
        {
            /*  A = fundamental
             *      A = 2*B + 40
             *      A = 8*C + 360
             *  
             *  B = (A/2) - 20 = 4*C + 160
             *      B.multiplier = 2
             *      B.offset = 20
             *      
             *  C = (B/4) - 40 = (A/8) - 45
             *      C.multiplier = 8
             *      C.offset = 45
             */

            using var quant = BaseQuantity.DefineNewBaseQuantity(
                "quantity", "a");

            var a = quant.FundamentalUnit;
            using BaseUnit b = new ("b", a,
                multiplier: 2m,
                offset: 20m);

            using BaseUnit c = new ("c", b,
                multiplier: 4m,
                offset: 40m);


            Assert.AreEqual(1m, a.FundamentalMultiplier);
            Assert.AreEqual(0m, a.FundamentalOffset);

            Assert.AreEqual(2m, b.FundamentalMultiplier);
            Assert.AreEqual(20m, b.FundamentalOffset);

            Assert.AreEqual(8m, c.FundamentalMultiplier);
            Assert.AreEqual(45m, c.FundamentalOffset);

            {
                // Convert to fundamental
                var aAsFund = a.ConvertToFundamental(3m);
                Assert.AreEqual(3m, aAsFund.Magnitude);
                Assert.AreEqual(a, aAsFund.Unit);

                var bAsFund = b.ConvertToFundamental(3m);
                Assert.AreEqual(46m, bAsFund.Magnitude);
                Assert.AreEqual(a, bAsFund.Unit);

                var cAsFund = c.ConvertToFundamental(3m);
                Assert.AreEqual(384m, cAsFund.Magnitude);
                Assert.AreEqual(a, cAsFund.Unit);
            }

            {
                // Convert to A
                var aAsA = a.ConvertTo(3m, a);
                Assert.AreEqual(3m, aAsA.Magnitude);
                Assert.AreEqual(a, aAsA.Unit);

                var bAsA = b.ConvertToFundamental(3m);
                Assert.AreEqual(46m, bAsA.Magnitude);
                Assert.AreEqual(a, bAsA.Unit);

                var cAsA = c.ConvertToFundamental(3m);
                Assert.AreEqual(384m, cAsA.Magnitude);
                Assert.AreEqual(a, cAsA.Unit);
            }

            {
                // Convert to B
                var aAsB = a.ConvertTo(4m, b);
                Assert.AreEqual(-18m, aAsB.Magnitude);
                Assert.AreEqual(b, aAsB.Unit);

                var bAsB = b.ConvertTo(3m, b);
                Assert.AreEqual(3m, bAsB.Magnitude);
                Assert.AreEqual(b, bAsB.Unit);

                var cAsB = c.ConvertTo(2m, b);
                Assert.AreEqual(168m, cAsB.Magnitude);
                Assert.AreEqual(b, cAsB.Unit);
            }

            {
                // Convert to C
                var aAsC = a.ConvertTo(16m, c);
                Assert.AreEqual(-43m, aAsC.Magnitude);
                Assert.AreEqual(c, aAsC.Unit);

                var bAsC = b.ConvertTo(8m, c);
                Assert.AreEqual(-38m, bAsC.Magnitude);
                Assert.AreEqual(c, bAsC.Unit);

                var cAsC = c.ConvertTo(3m, c);
                Assert.AreEqual(3m, cAsC.Magnitude);
                Assert.AreEqual(c, cAsC.Unit);
            }
        }
    }
}

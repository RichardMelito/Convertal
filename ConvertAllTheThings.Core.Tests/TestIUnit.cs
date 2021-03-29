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
        public void TestConversions()
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

            var quant = BaseQuantity.DefineNewBaseQuantity(
                "quantity", "a");

            var a = (BaseUnit)quant.FundamentalUnit;
            BaseUnit b = new ("b", a,
                multiplier: 2m,
                offset: 20m);

            BaseUnit c = new ("c", b,
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
                Assert.AreSame(a, aAsFund.Unit);

                var bAsFund = b.ConvertToFundamental(3m);
                Assert.AreEqual(46m, bAsFund.Magnitude);
                Assert.AreSame(a, bAsFund.Unit);

                var cAsFund = c.ConvertToFundamental(3m);
                Assert.AreEqual(384m, cAsFund.Magnitude);
                Assert.AreSame(a, cAsFund.Unit);
            }

            {
                // Convert to A
                var aAsA = a.ConvertTo(3m, a);
                Assert.AreEqual(3m, aAsA.Magnitude);
                Assert.AreSame(a, aAsA.Unit);

                var bAsA = b.ConvertToFundamental(3m);
                Assert.AreEqual(46m, bAsA.Magnitude);
                Assert.AreSame(a, bAsA.Unit);

                var cAsA = c.ConvertToFundamental(3m);
                Assert.AreEqual(384m, cAsA.Magnitude);
                Assert.AreSame(a, cAsA.Unit);
            }

            {
                // Convert to B
                var aAsB = a.ConvertTo(4m, b);
                Assert.AreEqual(-18m, aAsB.Magnitude);
                Assert.AreSame(b, aAsB.Unit);

                var bAsB = b.ConvertTo(3m, b);
                Assert.AreEqual(3m, bAsB.Magnitude);
                Assert.AreSame(b, bAsB.Unit);

                var cAsB = c.ConvertTo(2m, b);
                Assert.AreEqual(168m, cAsB.Magnitude);
                Assert.AreSame(b, cAsB.Unit);
            }

            {
                // Convert to C
                var aAsC = a.ConvertTo(16m, c);
                Assert.AreEqual(-43m, aAsC.Magnitude);
                Assert.AreSame(c, aAsC.Unit);

                var bAsC = b.ConvertTo(8m, c);
                Assert.AreEqual(-38m, bAsC.Magnitude);
                Assert.AreSame(c, bAsC.Unit);

                var cAsC = c.ConvertTo(3m, c);
                Assert.AreEqual(3m, cAsC.Magnitude);
                Assert.AreSame(c, cAsC.Unit);
            }

            Prefix testPrefix = new("TestPrefix", 10m);
            PrefixedBaseUnit pA = new(a, testPrefix);
            PrefixedBaseUnit pB = new(b, testPrefix);
            PrefixedBaseUnit pC = new(c, testPrefix);

            Assert.AreEqual(10m, pA.FundamentalMultiplier); 
            Assert.AreEqual(0m, pA.FundamentalOffset);

            Assert.AreEqual(20m, pB.FundamentalMultiplier);
            Assert.AreEqual(2m, pB.FundamentalOffset);

            Assert.AreEqual(80m, pC.FundamentalMultiplier);
            Assert.AreEqual(4.5m, pC.FundamentalOffset);

            {
                var aAsPa = a.ConvertTo(20m, pA);
                Assert.AreEqual(2m, aAsPa.Magnitude);
                Assert.AreSame(pA, aAsPa.Unit);

                var cAsPc = c.ConvertTo(20m, pC);
                Assert.AreEqual(2m, cAsPc.Magnitude);
                Assert.AreSame(pC, cAsPc.Unit);

                var paAsA = pA.ConvertTo(2m, a);
                Assert.AreEqual(20m, paAsA.Magnitude);
                Assert.AreSame(a, paAsA.Unit);

                var pcAsC = pC.ConvertTo(2m, c);
                Assert.AreEqual(20m, pcAsC.Magnitude);
                Assert.AreSame(c, pcAsC.Unit);
            }

            {
                var aAsPc = a.ConvertTo(3200m, pC);
                Assert.AreEqual(35.5m, aAsPc.Magnitude);
                Assert.AreSame(pC, aAsPc.Unit);

                var cAsPa = c.ConvertTo(30m, pA);
                Assert.AreEqual(60m, cAsPa.Magnitude);
                Assert.AreSame(pA, cAsPa.Unit);

                var paAsC = pA.ConvertTo(60m, c);
                Assert.AreEqual(30m, paAsC.Magnitude);
                Assert.AreSame(c, paAsC.Unit);

                var pCAsA = pC.ConvertTo(35.5m, a);
                Assert.AreEqual(3200m, pCAsA.Magnitude);
                Assert.AreSame(a, pCAsA.Unit);
            }
        }
    }
}

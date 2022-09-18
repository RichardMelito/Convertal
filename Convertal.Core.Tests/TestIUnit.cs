// Created by Richard Melito and licensed to you under The Clear BSD License.

using Xunit;

namespace ConvertAllTheThings.Core.Tests;

public class TestIUnit : BaseTestClass
{
    [Fact]
    public void TestUnitCompositions()
    {
        var quantA = Database.DefineBaseQuantity("quantA",
            "a");

        var a = (BaseUnit)quantA.FundamentalUnit;
        var b = new BaseUnit(Database, "b", a, 2);
        var prefix = new Prefix(Database, "prefix", 4);
        var c = Database.GetPrefixedUnit(a, prefix);

        var composition =
            a.UnitComposition *
            b.UnitComposition /
            c.UnitComposition;

        var d = Database.DefineFromComposition("d", composition);
        Assert.Same(quantA, d.Quantity);
        Assert.IsType<BaseUnit>(d);
        Assert.Equal(0.5m, d.FundamentalMultiplier);
    }

    [Fact]
    public void TestConversionsAndDefinitions()
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

        var quant = Database.DefineBaseQuantity(
            "quantity", "a");

        var a = (BaseUnit)quant.FundamentalUnit;
        BaseUnit b = new(Database, "b", a,
            multiplier: 2m,
            offset: 20m);

        BaseUnit c = new(Database, "c", b,
            multiplier: 4m,
            offset: 40m);


        Assert.Equal(1m, a.FundamentalMultiplier);
        Assert.Equal(0m, a.FundamentalOffset);

        Assert.Equal(2m, b.FundamentalMultiplier);
        Assert.Equal(20m, b.FundamentalOffset);

        Assert.Equal(8m, c.FundamentalMultiplier);
        Assert.Equal(45m, c.FundamentalOffset);

        {
            // Convert to fundamental
            var aAsFund = a.ConvertToFundamental(3m);
            Assert.Equal(3m, aAsFund.Magnitude);
            Assert.Same(a, aAsFund.Unit);

            var bAsFund = b.ConvertToFundamental(3m);
            Assert.Equal(46m, bAsFund.Magnitude);
            Assert.Same(a, bAsFund.Unit);

            var cAsFund = c.ConvertToFundamental(3m);
            Assert.Equal(384m, cAsFund.Magnitude);
            Assert.Same(a, cAsFund.Unit);
        }

        {
            // Convert to A
            var aAsA = a.ConvertTo(3m, a);
            Assert.Equal(3m, aAsA.Magnitude);
            Assert.Same(a, aAsA.Unit);

            var bAsA = b.ConvertToFundamental(3m);
            Assert.Equal(46m, bAsA.Magnitude);
            Assert.Same(a, bAsA.Unit);

            var cAsA = c.ConvertToFundamental(3m);
            Assert.Equal(384m, cAsA.Magnitude);
            Assert.Same(a, cAsA.Unit);
        }

        {
            // Convert to B
            var aAsB = a.ConvertTo(4m, b);
            Assert.Equal(-18m, aAsB.Magnitude);
            Assert.Same(b, aAsB.Unit);

            var bAsB = b.ConvertTo(3m, b);
            Assert.Equal(3m, bAsB.Magnitude);
            Assert.Same(b, bAsB.Unit);

            var cAsB = c.ConvertTo(2m, b);
            Assert.Equal(168m, cAsB.Magnitude);
            Assert.Same(b, cAsB.Unit);
        }

        {
            // Convert to C
            var aAsC = a.ConvertTo(16m, c);
            Assert.Equal(-43m, aAsC.Magnitude);
            Assert.Same(c, aAsC.Unit);

            var bAsC = b.ConvertTo(8m, c);
            Assert.Equal(-38m, bAsC.Magnitude);
            Assert.Same(c, bAsC.Unit);

            var cAsC = c.ConvertTo(3m, c);
            Assert.Equal(3m, cAsC.Magnitude);
            Assert.Same(c, cAsC.Unit);
        }

        Prefix testPrefix = new(Database, "TestPrefix", 10m);
        var pA = Database.GetPrefixedUnit(a, testPrefix);
        var pB = Database.GetPrefixedUnit(b, testPrefix);
        var pC = Database.GetPrefixedUnit(c, testPrefix);

        Assert.Equal(10m, pA.FundamentalMultiplier);
        Assert.Equal(0m, pA.FundamentalOffset);

        Assert.Equal(20m, pB.FundamentalMultiplier);
        Assert.Equal(2m, pB.FundamentalOffset);

        Assert.Equal(80m, pC.FundamentalMultiplier);
        Assert.Equal(4.5m, pC.FundamentalOffset);

        {
            var aAsPa = a.ConvertTo(20m, pA);
            Assert.Equal(2m, aAsPa.Magnitude);
            Assert.Same(pA, aAsPa.Unit);

            var cAsPc = c.ConvertTo(20m, pC);
            Assert.Equal(2m, cAsPc.Magnitude);
            Assert.Same(pC, cAsPc.Unit);

            var paAsA = pA.ConvertTo(2m, a);
            Assert.Equal(20m, paAsA.Magnitude);
            Assert.Same(a, paAsA.Unit);

            var pcAsC = pC.ConvertTo(2m, c);
            Assert.Equal(20m, pcAsC.Magnitude);
            Assert.Same(c, pcAsC.Unit);
        }

        {
            var aAsPc = a.ConvertTo(3200m, pC);
            Assert.Equal(35.5m, aAsPc.Magnitude);
            Assert.Same(pC, aAsPc.Unit);

            var cAsPa = c.ConvertTo(30m, pA);
            Assert.Equal(60m, cAsPa.Magnitude);
            Assert.Same(pA, cAsPa.Unit);

            var paAsC = pA.ConvertTo(60m, c);
            Assert.Equal(30m, paAsC.Magnitude);
            Assert.Same(c, paAsC.Unit);

            var pCAsA = pC.ConvertTo(35.5m, a);
            Assert.Equal(3200m, pCAsA.Magnitude);
            Assert.Same(a, pCAsA.Unit);
        }
    }
}

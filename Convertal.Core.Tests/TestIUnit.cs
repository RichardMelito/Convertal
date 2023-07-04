// Created by Richard Melito and licensed to you under The Clear BSD License.

using FluentAssertions;
using Xunit;

namespace Convertal.Core.Tests;

public class TestIUnit : BaseTestClass
{
    [Fact]
    public void TestUnitCompositions()
    {
        var quantA = Database.DefineScalarBaseQuantity("quantA",
            "a");

        var a = (ScalarBaseUnit)quantA.FundamentalUnit;
        var b = new ScalarBaseUnit(Database, "b", a, 2);
        var prefix = new Prefix(Database, "prefix", 4);
        var c = Database.GetPrefixedUnit(a, prefix);

        var composition =
            a.UnitComposition *
            b.UnitComposition /
            c.UnitComposition;

        var d = Database.DefineFromScalarComposition("d", composition);
        quantA.Should().BeSameAs(d.Quantity);
        d.Should().BeOfType<ScalarBaseUnit>();
        d.FundamentalMultiplier.Should().Be(0.5m);
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

        var quant = Database.DefineScalarBaseQuantity(
            "quantity", "a");

        var a = quant.FundamentalUnit;
        a.Should().BeOfType<ScalarBaseUnit>();
        IScalarBaseUnit b = new ScalarBaseUnit(Database, "b", a,
            multiplier: 2m,
            offset: 20m);

        IScalarBaseUnit c = new ScalarBaseUnit(Database, "c", b,
            multiplier: 4m,
            offset: 40m);

        a.FundamentalMultiplier.Should().Be(1m);
        a.FundamentalOffset.Should().Be(0m);

        b.FundamentalMultiplier.Should().Be(2m);
        b.FundamentalOffset.Should().Be(20m);

        c.FundamentalMultiplier.Should().Be(8m);
        c.FundamentalOffset.Should().Be(45m);

        {
            // Convert to fundamental
            var aAsFund = a.ConvertToFundamental(3m);
            aAsFund.Magnitude.Should().Be(3m);
            aAsFund.Unit.Should().BeSameAs(a);

            var bAsFund = b.ConvertToFundamental(3m);
            bAsFund.Magnitude.Should().Be(46m);
            bAsFund.Unit.Should().BeSameAs(a);

            var cAsFund = c.ConvertToFundamental(3m);
            cAsFund.Magnitude.Should().Be(384m);
            cAsFund.Unit.Should().BeSameAs(a);
        }

        {
            // Convert to A
            var aAsA = a.ConvertTo(3m, a);
            aAsA.Magnitude.Should().Be(3m);
            aAsA.Unit.Should().BeSameAs(a);

            var bAsA = b.ConvertToFundamental(3m);
            bAsA.Magnitude.Should().Be(46m);
            bAsA.Unit.Should().BeSameAs(a);

            var cAsA = c.ConvertToFundamental(3m);
            cAsA.Magnitude.Should().Be(384m);
            cAsA.Unit.Should().BeSameAs(a);
        }

        {
            // Convert to B
            var aAsB = a.ConvertTo(4m, b);
            aAsB.Magnitude.Should().Be(-18m);
            aAsB.Unit.Should().BeSameAs(b);

            var bAsB = b.ConvertTo(3m, b);
            bAsB.Magnitude.Should().Be(3m);
            bAsB.Unit.Should().BeSameAs(b);

            var cAsB = c.ConvertTo(2m, b);
            cAsB.Magnitude.Should().Be(168m);
            cAsB.Unit.Should().BeSameAs(b);
        }

        {
            // Convert to C
            var aAsC = a.ConvertTo(16m, c);
            aAsC.Magnitude.Should().Be(-43m);
            aAsC.Unit.Should().BeSameAs(c);

            var bAsC = b.ConvertTo(8m, c);
            bAsC.Magnitude.Should().Be(-38m);
            bAsC.Unit.Should().BeSameAs(c);

            var cAsC = c.ConvertTo(3m, c);
            cAsC.Magnitude.Should().Be(3m);
            cAsC.Unit.Should().BeSameAs(c);
        }

        Prefix testPrefix = new(Database, "TestPrefix", 10m);
        var pA = Database.GetPrefixedUnit((ScalarBaseUnit)a, testPrefix);
        var pB = Database.GetPrefixedUnit((ScalarBaseUnit)b, testPrefix);
        var pC = Database.GetPrefixedUnit((ScalarBaseUnit)c, testPrefix);

        pA.FundamentalMultiplier.Should().Be(10m);
        pA.FundamentalOffset.Should().Be(0m);

        pB.FundamentalMultiplier.Should().Be(20m);
        pB.FundamentalOffset.Should().Be(2m);

        pC.FundamentalMultiplier.Should().Be(80m);
        pC.FundamentalOffset.Should().Be(4.5m);

        {
            var aAsPa = a.ConvertTo(20m, pA);
            aAsPa.Magnitude.Should().Be(2m);
            aAsPa.Unit.Should().BeSameAs(pA);

            var cAsPc = c.ConvertTo(20m, pC);
            cAsPc.Magnitude.Should().Be(2m);
            cAsPc.Unit.Should().BeSameAs(pC);

            var paAsA = ((IScalarUnit)pA).ConvertTo(2m, a);
            paAsA.Magnitude.Should().Be(20m);
            paAsA.Unit.Should().BeSameAs(a);

            var pcAsC = ((IScalarUnit)pC).ConvertTo(2m, c);
            pcAsC.Magnitude.Should().Be(20m);
            pcAsC.Unit.Should().BeSameAs(c);
        }

        {
            var aAsPc = a.ConvertTo(3200m, pC);
            aAsPc.Magnitude.Should().Be(35.5m);
            aAsPc.Unit.Should().BeSameAs(pC);

            var cAsPa = c.ConvertTo(30m, pA);
            cAsPa.Magnitude.Should().Be(60m);
            cAsPa.Unit.Should().BeSameAs(pA);

            var paAsC = ((IScalarUnit)pA).ConvertTo(60m, c);
            paAsC.Magnitude.Should().Be(30m);
            paAsC.Unit.Should().BeSameAs(c);

            var pCAsA = ((IScalarUnit)pC).ConvertTo(35.5m, a);
            pCAsA.Magnitude.Should().Be(3200m);
            pCAsA.Unit.Should().BeSameAs(a);
            Assert.Equal(3200m, pCAsA.Magnitude);
            Assert.Same(a, pCAsA.Unit);
        }
    }
}

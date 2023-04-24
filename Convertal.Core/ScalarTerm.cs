// Created by Richard Melito and licensed to you under The Clear BSD License.

using DecimalMath;

namespace Convertal.Core;

public record ScalarTerm : Term, IScalar<ScalarTerm, VectorTerm>
{
    public override bool IsVector => false;

    public override decimal Magnitude { get; }
    public override IScalarUnit Unit => (IScalarUnit)base.Unit;
    public override ScalarQuantity Quantity => (ScalarQuantity)base.Quantity;

    public VectorTerm? VectorAnalog => Unit.VectorAnalog?.ToTerm(Magnitude);

    public ScalarTerm(decimal magnitude, IScalarUnit unit): base(unit) => Magnitude = magnitude;


    public ScalarTerm Pow(decimal power)
    {
        var fundamental = ConvertUnitToFundamental();
        var resMagnitude = DecimalEx.Pow(fundamental.Magnitude, power);
        var resQuantity = fundamental.Quantity.Pow(power);
        return new(resMagnitude, resQuantity.FundamentalUnit);
    }


    public ScalarTerm ConvertUnitToPreferredSystem(MeasurementSystem? input = null)
    {
        var system = input ?? MeasurementSystem.Current;
        var resultingUnit = system?.GetScalarUnit(Quantity) ?? Quantity.FundamentalUnit;
        return ConvertUnitTo(resultingUnit);
    }

    public ScalarTerm ConvertUnitToFundamental()
    {
        return Unit.ConvertToFundamental(Magnitude);
    }

    public ScalarTerm ConvertUnitTo(IScalarUnit resultingIUnit)
    {
        return Unit.ConvertTo(Magnitude, resultingIUnit);
    }

    public ScalarTerm Multiply(ScalarTerm other)
    {
        var fund = ConvertUnitToFundamental();
        var otherFund = other.ConvertUnitToFundamental();
        var resQuantity = Quantity.Multiply(other.Quantity);
        return new(fund.Magnitude * otherFund.Magnitude, resQuantity.FundamentalUnit);
    }
    public VectorTerm Multiply(VectorTerm vector)
    {
        var fund = ConvertUnitToFundamental();
        var vectorFund = vector.ConvertUnitToFundamental();
        var resQuantity = Quantity.Multiply(vector.Quantity);
        return new(
            fund.Magnitude * vectorFund.I,
            fund.Magnitude * vectorFund.J,
            fund.Magnitude * vectorFund.K,
            resQuantity.FundamentalUnit);
    }

    public ScalarTerm Divide(ScalarTerm other)
    {
        var fund = ConvertUnitToFundamental();
        var otherFund = other.ConvertUnitToFundamental();
        var resQuantity = Quantity.Divide(other.Quantity);
        return new(fund.Magnitude / otherFund.Magnitude, resQuantity.FundamentalUnit);
    }

    public override ScalarTerm Multiply(decimal multiplier) => new(Magnitude * multiplier, Unit);
    public override ScalarTerm Divide(decimal divisor) => new(Magnitude / divisor, Unit);

    public static ScalarTerm operator *(ScalarTerm term, decimal multiplier) => term.Multiply(multiplier);
    public static ScalarTerm operator *(decimal multiplier, ScalarTerm term) => term.Multiply(multiplier);
    public static ScalarTerm operator /(ScalarTerm term, decimal divisor) => term.Divide(divisor);
    public static ScalarTerm operator /(decimal numerator, ScalarTerm term)
    {
        var resMagnitude = numerator / term.ConvertUnitToFundamental().Magnitude;
        var resQuantity = term.Quantity.Database.EmptyQuantity / term.Quantity;
        return new(resMagnitude, resQuantity.FundamentalUnit);
    }

    public static ScalarTerm operator +(ScalarTerm lhs, ScalarTerm rhs)
    {
        var convertedRhs = rhs.ConvertUnitTo(lhs.Unit);
        return new(lhs.Magnitude + convertedRhs.Magnitude, lhs.Unit);
    }

    public static ScalarTerm operator -(ScalarTerm lhs, ScalarTerm rhs)
    {
        var convertedRhs = rhs.ConvertUnitTo(lhs.Unit);
        return new(lhs.Magnitude - convertedRhs.Magnitude, lhs.Unit);
    }
}

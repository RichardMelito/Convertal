// Created by Richard Melito and licensed to you under The Clear BSD License.

using DecimalMath;

namespace ConvertAllTheThings.Core;

public class Term
{
    public decimal Magnitude { get; }
    public IUnit Unit { get; }
    public Quantity Quantity => Unit.Quantity;
    public Term(decimal magnitude, IUnit unit)
    {
        Magnitude = magnitude;
        Unit = unit;
    }

    public override string ToString()
    {
        return Magnitude + " " + Unit;
    }

    public string ToStringSymbol()
    {
        return Magnitude + " " + Unit.ToStringSymbol();
    }

    public Term ConvertUnitToPreferredSystem(MeasurementSystem? input = null)
    {
        var system = input ?? MeasurementSystem.Current;
        var resultingUnit = system?.GetUnit(Quantity) ?? Quantity.FundamentalUnit;
        return ConvertUnitTo(resultingUnit);
    }

    public Term ConvertUnitToFundamental()
    {
        return Unit.ConvertToFundamental(Magnitude);
    }

    public Term ConvertUnitTo(IUnit resultingIUnit)
    {
        return Unit.ConvertTo(Magnitude, resultingIUnit);
    }

    public Term Pow(decimal power)
    {
        var fundamental = ConvertUnitToFundamental();
        var resMagnitude = DecimalEx.Pow(fundamental.Magnitude, power);
        var resQuantity = fundamental.Quantity.Pow(power);
        return new(resMagnitude, resQuantity.FundamentalUnit);
    }

    public static Term operator *(decimal multiplier, Term term)
        => new(multiplier * term.Magnitude, term.Unit);

    public static Term operator *(Term term, decimal multiplier)
        => new(multiplier * term.Magnitude, term.Unit);

    public static Term operator /(Term term, decimal divisor)
        => new(term.Magnitude / divisor, term.Unit);

    public static Term operator /(decimal divisor, Term term)
    {
        var resMagnitude = divisor / term.ConvertUnitToFundamental().Magnitude;
        var resQuantity = term.Quantity.Database.EmptyQuantity / term.Quantity;
        return new(resMagnitude, resQuantity.FundamentalUnit);
    }

    public static Term MultiplyOrDivide(Term lhs, Term rhs, bool multiplication)
    {
        var fundLhs = lhs.ConvertUnitToFundamental();
        var fundRhs = rhs.ConvertUnitToFundamental();

        decimal resMagnitude;
        if (multiplication)
            resMagnitude = fundLhs.Magnitude * fundRhs.Magnitude;
        else
            resMagnitude = fundLhs.Magnitude / fundRhs.Magnitude;

        var resQuantity = lhs.Quantity.MultiplyOrDivide(
            lhs.Quantity,
            rhs.Quantity,
            multiplication: multiplication);

        return new(resMagnitude, resQuantity.FundamentalUnit);
    }

    public static Term operator *(Term lhs, Term rhs)
    {
        return MultiplyOrDivide(lhs, rhs, multiplication: true);
    }

    public static Term operator /(Term lhs, Term rhs)
    {
        return MultiplyOrDivide(lhs, rhs, multiplication: false);
    }

    public static Term operator +(Term lhs, Term rhs)
    {
        var convertedRhs = rhs.ConvertUnitTo(lhs.Unit);
        return new(lhs.Magnitude + convertedRhs.Magnitude, lhs.Unit);
    }

    public static Term operator -(Term lhs, Term rhs)
    {
        var convertedRhs = rhs.ConvertUnitTo(lhs.Unit);
        return new(lhs.Magnitude - convertedRhs.Magnitude, lhs.Unit);
    }
}

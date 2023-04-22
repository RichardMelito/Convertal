// Created by Richard Melito and licensed to you under The Clear BSD License.

using DecimalMath;

namespace Convertal.Core;

public abstract record Term : IVectorOrScalar
{
    public abstract decimal Magnitude { get; }
    public virtual IUnit Unit { get; }
    public virtual Quantity Quantity => Unit.Quantity;

    protected virtual string AmountString => Magnitude.ToString();

    protected Term(IUnit unit) => Unit = unit;

    public override string ToString()
    {
        return AmountString + " " + Unit;
    }

    public string ToStringSymbol()
    {
        return AmountString + " " + Unit.ToStringSymbol();
    }

    public virtual Term ConvertUnitToPreferredSystem(MeasurementSystem? input = null)
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

    /*  For vector stuff need:
     *      Flag on Quantity for whether it is a vector or scalar or either (example of either: distance (scalar) vs displacement (vector))
     *          Should those be separate Quantities? Maybe new Quantity functionality for child Quantities that differ only on directionality?
     *      "Relaxed" mode: carry directionality between operations but do simple magnitude-only multiplication.
     *          For when users only want to enter the magnitude and assume everything is orthogonal.
     *      "Strict" mode: must use full i-j-k notation
     *      Can use "_i..._j..._k" for text representations
     *      Maybe need special i-j-k empty quantity term?
     *      
     *      For quantities and units:
     *          scalar: pow
     *          vector: dotP pow
     *          scalar scalar: multiply, divide
     *          scalar vector: multiply, divide
     *          vector scalar: multiply, divide
     *          vector vector: crossP, dotP
     *      For Terms, can do multiply, divide, crossP, add, subtract, pow
     *          scalar: pow, negate
     *          vector: dotP pow, negate
     *          scalar scalar: multiply, divide, 
     *              same quantity: add, subtract
     *          scalar vector: multiply, divide
     *          vector scalar: multiply, divide
     *          vector vector: crossP, dotP
     *              same quantity: add, subtract
     */
}

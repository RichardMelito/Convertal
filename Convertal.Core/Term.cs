// Created by Richard Melito and licensed to you under The Clear BSD License.

using DecimalMath;

namespace Convertal.Core;

public abstract record Term : IVectorOrScalar
{
    public abstract decimal Magnitude { get; }
    public virtual IUnit Unit { get; }
    public virtual Quantity Quantity => Unit.Quantity;

    protected virtual string AmountString => Magnitude.ToString();

    public abstract bool IsVector { get; }

    protected Term(IUnit unit) => Unit = unit;

    public override string ToString()
    {
        return AmountString + " " + Unit;
    }

    public string ToStringSymbol()
    {
        return AmountString + " " + Unit.ToStringSymbol();
    }

    public abstract Term Multiply(decimal multiplier);
    public abstract Term Divide(decimal divisor);

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

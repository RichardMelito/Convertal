// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

/* We need compile-time type safety for scalar and vector multiplication operations.
 * 
 * IUnit
 *  \ ScalarUnit : IScalar
 *    \ ScalarBaseUnit : IBase
 *    \ ScalarDerivedUnit : IDerived
 *    \ ScalarPrefixedUnit
 *      \ ScalarPrefixedBaseUnit : IBase
 *      \ ScalarPrefixedDerivedUnit : IDerived
 *  \ VectorUnit : IVector
 *    \ VectorBaseUnit : IBase
 *    \ VectorDerivedUnit : IDerived
 *    \ VectorPrefixedUnit
 *      \ VectorPrefixedBaseUnit : IBase
 *      \ VectorPrefixedDerivedUnit : IDerived
 *    
 * IUnit
 *  \ IScalarUnit : IScalar
 *  \ IVectorUnit : IVector
 *  \ IScalarBaseUnit : IScalarUnit, IBaseUnit
 *  \ IVectorBaseUnit : IVectorUnit, IBaseUnit
 *  \ IScalarDerivedUnit : IScalarUnit, IDerivedUnit
 *  \ IVectorDerivedUnit : IVectorUnit, IDerivedUnit
 *  \ Unit
 *    \ ScalarUnit : IScalarUnit
 *      \ ScalarBaseUnit : IBaseUnit
 *      \ ScalarDerivedUnit : IDerivedUnit
 *    \ VectorUnit : IVectorUnit
 *      \ VectorBaseUnit : IBaseUnit
 *      \ VectorDerivedUnit : IDerivedUnit
 *  \ PrefixedUnit
 *    \ ScalarPrefixedUnit : IScalarUnit
 *      \ ScalarPrefixedBaseUnit : IBaseUnit
 *      \ ScalarPrefixedDerivedUnit : IDerivedUnit
 *    \ VectorPrefixedUnit : IVectorUnit
 *      \ VectorPrefixedBaseUnit : IBaseUnit
 *      \ VectorPrefixedDerivedUnit : IDerivedUnit
 */

public interface IScalarUnit : IUnit, IScalar<IScalarUnit, IVectorUnit>
{
    // TODO
    static virtual IScalarUnit operator *(IScalarUnit left, IScalarUnit right) => throw new NotImplementedException();
    static virtual IScalarUnit operator /(IScalarUnit left, IScalarUnit right) => throw new NotImplementedException();

    static virtual IVectorUnit operator *(IScalarUnit scalar, IVectorUnit vector) => throw new NotImplementedException();

    decimal FundamentalOffset { get; }
    /*  K is fundamental
     *  C = K - 273
     *  C's FundamentalOffset = +273
     */

    ScalarTerm ToTerm(decimal magnitude) => new(magnitude, this);

    static ScalarTerm ConvertTo(IScalarUnit toConvert, decimal magnitudeToConvert, IScalarUnit resultingIUnit)
    {
        if (toConvert.Equals(resultingIUnit))
            return new ScalarTerm(magnitudeToConvert, toConvert);

        if (resultingIUnit.Quantity != toConvert.Quantity)
            throw new InvalidOperationException("Can only convert to other units of the same quantity.");

        var fundamental = toConvert.ConvertToFundamental(magnitudeToConvert);
        var magnitude =
            (fundamental.Magnitude / resultingIUnit.FundamentalMultiplier)

            // TODO is subtraction correct?
            - resultingIUnit.FundamentalOffset;
        return new (magnitude, resultingIUnit);

        /*  converting from km to mm
         *      km multiplier = 1000
         *      mm multiplier = 0.001
         *      1 km = 1,000,000 mm
         *      => multiplier = km.multiplier / mm.multiplier
         */
    }

    static ScalarTerm ConvertToFundamental(IScalarUnit toConvert, decimal magnitudeToConvert)
    {
        if (toConvert.IsFundamental)
            return new (magnitudeToConvert, toConvert);

        var magnitude = toConvert.FundamentalMultiplier * (magnitudeToConvert + toConvert.FundamentalOffset);
        return new(magnitude, (IScalarUnit)toConvert.Quantity.FundamentalUnit);
    }

    ScalarTerm ConvertTo(decimal magnitudeOfThis, IScalarUnit resultingIUnit)
    {
        return ConvertTo(this, magnitudeOfThis, resultingIUnit);
    }

    ScalarTerm ConvertToFundamental(decimal magnitudeOfThis)
    {
        return ConvertToFundamental(this, magnitudeOfThis);
    }
}

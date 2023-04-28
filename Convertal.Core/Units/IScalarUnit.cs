// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Numerics;

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
    //// TODO
    //static virtual IScalarUnit operator *(IScalarUnit left, IScalarUnit right) => throw new NotImplementedException();
    //static virtual IScalarUnit operator /(IScalarUnit left, IScalarUnit right) => throw new NotImplementedException();

    //static virtual IVectorUnit operator *(IScalarUnit scalar, IVectorUnit vector) => throw new NotImplementedException();

    NamedComposition<IUnit> IUnit.UnitComposition => UnitComposition;
    new ScalarComposition<IUnit> UnitComposition { get; }

    Quantity IUnit.Quantity => Quantity;
    new ScalarQuantity Quantity { get; }

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
        return new(magnitude, toConvert.Quantity.FundamentalUnit);
    }

    ScalarTerm ConvertTo(decimal magnitudeOfThis, IScalarUnit resultingIUnit) => ConvertTo(this, magnitudeOfThis, resultingIUnit);

    ScalarTerm ConvertToFundamental(decimal magnitudeOfThis) => ConvertToFundamental(this, magnitudeOfThis);

    IScalarUnit IScalar<IScalarUnit, IVectorUnit>.Pow(decimal power) => Database.GetUnitFromBaseComposition(UnitComposition.Pow(power));

    IScalarUnit IScalar<IScalarUnit, IVectorUnit>.Multiply(IScalarUnit other) => Database.GetUnitFromBaseComposition(UnitComposition * other.UnitComposition); //Multiply(other);
    //new IScalarUnit Multiply(IScalarUnit other) => Database.GetUnitFromBaseComposition(UnitComposition * other.UnitComposition);

    IVectorUnit IScalar<IScalarUnit, IVectorUnit>.Multiply(IVectorUnit vector) => Database.GetUnitFromBaseComposition(UnitComposition * vector.UnitComposition); //Multiply(vector);
    //new IVectorUnit Multiply(IVectorUnit vector) => Database.GetUnitFromBaseComposition(UnitComposition * vector.UnitComposition);

    IScalarUnit IScalar<IScalarUnit, IVectorUnit>.Divide(IScalarUnit other) => Database.GetUnitFromBaseComposition(UnitComposition / other.UnitComposition); //Divide(other);
    //new IScalarUnit Divide(IScalarUnit other) => Database.GetUnitFromBaseComposition(UnitComposition / other.UnitComposition);
}

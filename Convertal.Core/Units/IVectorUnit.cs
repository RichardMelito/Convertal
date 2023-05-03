// Created by Richard Melito and licensed to you under The Clear BSD License.
using System;

namespace Convertal.Core;

public interface IVectorUnit : IUnit, IVector<IVectorUnit, IScalarUnit>
{
    NamedComposition<IUnit> IUnit.UnitComposition => UnitComposition;
    new VectorComposition<IUnit> UnitComposition { get; }

    Quantity IUnit.Quantity => Quantity;
    new VectorQuantity Quantity { get; }

    VectorTerm ToTerm(decimal i, decimal j, decimal k) => new(i, j, k, this);
    VectorTerm ToTerm(decimal magnitude) => new(magnitude, this);

    static VectorTerm ConvertTo(IVectorUnit toConvert, decimal i, decimal j, decimal k, IVectorUnit resultingIUnit)
    {
        if (toConvert.Equals(resultingIUnit))
            return new VectorTerm(i, j, k, toConvert);

        if (resultingIUnit.Quantity != toConvert.Quantity)
            throw new InvalidOperationException("Can only convert to other units of the same quantity.");

        var fundamental = toConvert.ConvertToFundamental(i, j, k);
        return new(
            fundamental.I / resultingIUnit.FundamentalMultiplier,
            fundamental.J / resultingIUnit.FundamentalMultiplier,
            fundamental.K / resultingIUnit.FundamentalMultiplier,
            resultingIUnit);
    }

    static VectorTerm ConvertToFundamental(IVectorUnit toConvert, decimal i, decimal j, decimal k)
    {
        if (toConvert.IsFundamental)
            return new(i, j, k, toConvert);

        return new(
            i / toConvert.FundamentalMultiplier,
            j / toConvert.FundamentalMultiplier,
            k / toConvert.FundamentalMultiplier,
            toConvert.Quantity.FundamentalUnit);
    }

    VectorTerm ConvertTo(decimal i, decimal j, decimal k, IVectorUnit resultingIUnit) => ConvertTo(this, i, j, k, resultingIUnit);

    VectorTerm ConvertToFundamental(decimal i, decimal j, decimal k) => ConvertToFundamental(this, i, j, k);

    IVectorUnit IVector<IVectorUnit, IScalarUnit>.Multiply(IScalarUnit scalar) => scalar.Multiply(this);
    //IVectorUnit IVector<IVectorUnit, IScalarUnit>.Multiply(IScalarUnit scalar) => Database.GetUnitFromBaseComposition(UnitComposition * scalar.UnitComposition); //Multiply(scalar);
    //new IVectorUnit Multiply(IScalarUnit scalar) => Database.GetUnitFromBaseComposition(UnitComposition * scalar.UnitComposition);

    IVectorUnit IVector<IVectorUnit, IScalarUnit>.Divide(IScalarUnit scalar) =>
        Database.GetVectorQuantityFromBaseComposition(Quantity.BaseQuantityComposition / scalar.Quantity.BaseQuantityComposition).FundamentalUnit;
    //IVectorUnit IVector<IVectorUnit, IScalarUnit>.Divide(IScalarUnit scalar) => Database.GetUnitFromBaseComposition(UnitComposition / scalar.UnitComposition); //Divide(scalar);
    //new IVectorUnit Divide(IScalarUnit scalar) => Database.GetUnitFromBaseComposition(UnitComposition / scalar.UnitComposition);

    IScalarUnit IVector<IVectorUnit, IScalarUnit>.DotP(IVectorUnit other) =>
        Database.GetScalarQuantityFromBaseComposition(Quantity.BaseQuantityComposition * other.Quantity.BaseQuantityComposition).FundamentalUnit;
    //IScalarUnit IVector<IVectorUnit, IScalarUnit>.DotP(IVectorUnit other) => Database.GetUnitFromBaseComposition(UnitComposition * other.UnitComposition); //DotP(other);
    //new IScalarUnit DotP(IVectorUnit other) => Database.GetUnitFromBaseComposition(UnitComposition * other.UnitComposition);

    IVectorUnit IVector<IVectorUnit, IScalarUnit>.CrossP(IVectorUnit other) =>
        Database.GetVectorQuantityFromBaseComposition(Quantity.BaseQuantityComposition & other.Quantity.BaseQuantityComposition).FundamentalUnit;
    //IVectorUnit IVector<IVectorUnit, IScalarUnit>.CrossP(IVectorUnit other) => Database.GetUnitFromBaseComposition(UnitComposition & other.UnitComposition); //CrossP(other);
    //new IVectorUnit CrossP(IVectorUnit other) => Database.GetUnitFromBaseComposition(UnitComposition & other.UnitComposition);
}

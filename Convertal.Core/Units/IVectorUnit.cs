// Created by Richard Melito and licensed to you under The Clear BSD License.
using System;

namespace Convertal.Core;

public interface IVectorUnit : IUnit, IVector<IVectorUnit, IScalarUnit>
{
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
            (IVectorUnit)toConvert.Quantity.FundamentalUnit);
    }

    VectorTerm ConvertTo(decimal i, decimal j, decimal k, IVectorUnit resultingIUnit)
    {
        return ConvertTo(this, i, j, k, resultingIUnit);
    }

    VectorTerm ConvertToFundamental(decimal i, decimal j, decimal k)
    {
        return ConvertToFundamental(this, i, j, k);
    }
}

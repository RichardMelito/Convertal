// Created by Richard Melito and licensed to you under The Clear BSD License.
using System;

namespace Convertal.Core;

public abstract class VectorQuantity : Quantity, IVector<VectorQuantity, ScalarQuantity>
{
    // Must be implemented by derived types
    public override IVectorUnit FundamentalUnit => throw new NotImplementedException();

    // Must be set at construciton by derived types
    protected VectorComposition<IBaseQuantity> SettableBaseQuantityComposition { get; init; } = null!;
    public override VectorComposition<IBaseQuantity> BaseQuantityComposition => SettableBaseQuantityComposition;

    public override bool IsVector => true;

    public abstract ScalarQuantity ScalarAnalog { get; }

    protected VectorQuantity(Database database, string? name, string? symbol)
        : base(database, name, symbol)
    {
    }

    public VectorQuantity CrossP(VectorQuantity other)
    {
        var resultingComposition = BaseQuantityComposition.CrossP(other.BaseQuantityComposition);
        return (VectorQuantity)Database.GetQuantityFromBaseComposition(resultingComposition);
    }
    public ScalarQuantity DotP(VectorQuantity other)
    {
        var resultingComposition = BaseQuantityComposition.CrossP(other.BaseQuantityComposition);
        return (ScalarQuantity)Database.GetQuantityFromBaseComposition(resultingComposition);
    }

    public VectorQuantity Multiply(ScalarQuantity scalar) => scalar.Multiply(this);

    public VectorQuantity Divide(ScalarQuantity scalar)
    {
        var resultingComposition = BaseQuantityComposition.Divide(scalar.BaseQuantityComposition);
        return (VectorQuantity)Database.GetQuantityFromBaseComposition(resultingComposition);
    }

    public static VectorQuantity operator /(VectorQuantity left, ScalarQuantity right) => left.Divide(right);
    public static ScalarQuantity operator *(VectorQuantity lhs, VectorQuantity rhs) => lhs.DotP(rhs);
    public static VectorQuantity operator &(VectorQuantity lhs, VectorQuantity rhs) => lhs.CrossP(rhs);
}

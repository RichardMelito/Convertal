// Created by Richard Melito and licensed to you under The Clear BSD License.
using System;

namespace Convertal.Core;

public abstract class VectorQuantity : Quantity, IVector<VectorQuantity, ScalarQuantity>
{
    private readonly ScalarQuantity _scalarAnalog;

    // Must be implemented by derived types
    public override IVectorUnit FundamentalUnit => ScalarAnalog.FundamentalUnit.VectorAnalog!;

    public override VectorComposition<IBaseQuantity> BaseQuantityComposition { get; }

    public override bool IsVector => true;

    public virtual ScalarQuantity ScalarAnalog => _scalarAnalog;

    protected VectorQuantity(
        ScalarQuantity scalarAnalog,
        VectorComposition<IBaseQuantity>? composition,
        string? name,
        string? symbol)
        : base(scalarAnalog.Database, name, symbol)
    {
        _scalarAnalog = scalarAnalog;
        _scalarAnalog.SetVectorAnalog(this);

        BaseQuantityComposition = composition ?? new((IBaseQuantity)this);
        if (BaseQuantityComposition.ScalarAnalog != scalarAnalog.BaseQuantityComposition)
        {
            try
            {
                throw new InvalidOperationException();
            }
            finally
            {
                _scalarAnalog.SetVectorAnalog(null);
            }
        }
    }

    public VectorQuantity CrossP(VectorQuantity other)
    {
        var resultingComposition = BaseQuantityComposition.CrossP(other.BaseQuantityComposition);
        return (VectorQuantity)Database.GetQuantityFromBaseComposition(resultingComposition);
    }
    public ScalarQuantity DotP(VectorQuantity other)
    {
        var resultingComposition = BaseQuantityComposition.DotP(other.BaseQuantityComposition);
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

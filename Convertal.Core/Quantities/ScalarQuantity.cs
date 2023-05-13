// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Text;
using System.Threading.Tasks;

namespace Convertal.Core;
public abstract class ScalarQuantity : Quantity, IScalar<ScalarQuantity, VectorQuantity>
{
    // Must be implemented by derived types
    public override IScalarUnit FundamentalUnit => throw new NotImplementedException();

    public override ScalarComposition<IBaseQuantity> BaseQuantityComposition { get; }

    public override bool IsVector => false;
    public virtual VectorQuantity? VectorAnalog { get; private set; }

    protected ScalarQuantity(
        Database database,
        ScalarComposition<IBaseQuantity>? composition,
        string? name,
        string? symbol)
        : base(database, name, symbol)
    {
        BaseQuantityComposition = composition ?? new((IBaseQuantity)this);
    }

    internal void SetVectorAnalog(VectorQuantity? analog)
    {
        // analog = null means that we hit an exception in the VectorQuantity constructor
        // so we're clearing this property to clean up
        if (analog is not null && VectorAnalog is not null)
            throw new InvalidOperationException();

        VectorAnalog = analog;
    }

    public ScalarQuantity Pow(decimal power)
    {
        return (ScalarQuantity)Database.GetQuantityFromBaseComposition(BaseQuantityComposition.Pow(power));
    }

    public ScalarQuantity Multiply(ScalarQuantity other)
    {
        var resultingComposition = BaseQuantityComposition * other.BaseQuantityComposition;
        return (ScalarQuantity)Database.GetQuantityFromBaseComposition(resultingComposition);
    }
    public VectorQuantity Multiply(VectorQuantity vector)
    {
        var resultingComposition = BaseQuantityComposition * vector.BaseQuantityComposition;
        return (VectorQuantity)Database.GetQuantityFromBaseComposition(resultingComposition);
    }
    public ScalarQuantity Divide(ScalarQuantity other)
    {
        var resultingComposition = BaseQuantityComposition / other.BaseQuantityComposition;
        return (ScalarQuantity)Database.GetQuantityFromBaseComposition(resultingComposition);
    }

    public static ScalarQuantity operator *(ScalarQuantity left, ScalarQuantity right) => left.Multiply(right);
    public static ScalarQuantity operator /(ScalarQuantity left, ScalarQuantity right) => left.Divide(right);

    public static VectorQuantity operator *(ScalarQuantity scalar, VectorQuantity vector) => scalar.Multiply(vector);
    public static VectorQuantity operator *(VectorQuantity vector, ScalarQuantity scalar) => scalar.Multiply(vector);
}

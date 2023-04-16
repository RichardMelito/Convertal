// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Text;
using System.Threading.Tasks;

namespace Convertal.Core;
public abstract class ScalarQuantity : Quantity, IScalar<ScalarQuantity, VectorQuantity>
{
    // Must be implemented by derived types
    public override IScalarUnit FundamentalUnit => throw new NotImplementedException();

    // Must be set at construction by derived types
    protected ScalarComposition<IBaseQuantity> SettableBaseQuantityComposition { get; init; } = null!;
    public override ScalarComposition<IBaseQuantity> BaseQuantityComposition => SettableBaseQuantityComposition;


    public override bool IsVector => false;

    public abstract VectorQuantity? VectorAnalog { get; }

    protected ScalarQuantity(Database database, string? name, string? symbol)
        : base(database, name, symbol)
    {
    }

    public static ScalarQuantity operator *(ScalarQuantity left, ScalarQuantity right)
    {
        var resultingComposition = left.BaseQuantityComposition * right.BaseQuantityComposition;
        return (ScalarQuantity)left.Database.GetFromBaseComposition(resultingComposition);
    }
    public static VectorQuantity operator *(ScalarQuantity scalar, VectorQuantity vector)
    {
        var resultingComposition = scalar.BaseQuantityComposition * vector.BaseQuantityComposition;
        return (VectorQuantity)scalar.Database.GetFromBaseComposition(resultingComposition);
    }
    public static ScalarQuantity operator /(ScalarQuantity left, ScalarQuantity right)
    {
        var resultingComposition = left.BaseQuantityComposition / right.BaseQuantityComposition;
        return (ScalarQuantity)left.Database.GetFromBaseComposition(resultingComposition);
    }

    public ScalarQuantity Pow(decimal power)
    {
        return (ScalarQuantity)Database.GetFromBaseComposition(BaseQuantityComposition.Pow(power));
    }
}

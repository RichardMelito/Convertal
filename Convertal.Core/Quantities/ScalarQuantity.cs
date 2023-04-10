// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convertal.Core;
public abstract class ScalarQuantity : Quantity, IScalar<ScalarQuantity, VectorQuantity>
{
    public override ScalarComposition<IBaseQuantity> BaseQuantityComposition { get; }
    public override bool IsVector => false;

    public VectorQuantity? VectorAnalog { get; internal set; }

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

public abstract class VectorQuantity : Quantity, IVector<VectorQuantity, ScalarQuantity>
{
    public override VectorComposition<IBaseQuantity> BaseQuantityComposition { get; }
    public override bool IsVector => true;

    public ScalarQuantity ScalarAnalog { get; internal set; }

    protected VectorQuantity(Database database, string? name, string? symbol)
        : base(database, name, symbol)
    {

    }

    public VectorQuantity CrossP(VectorQuantity other)
    {
        var resultingComposition = BaseQuantityComposition.CrossP(other.BaseQuantityComposition);
        return (VectorQuantity)Database.GetFromBaseComposition(resultingComposition);
    }
    public ScalarQuantity DotP(VectorQuantity other)
    {
        var resultingComposition = BaseQuantityComposition.CrossP(other.BaseQuantityComposition);
        return (ScalarQuantity)Database.GetFromBaseComposition(resultingComposition);
    }
}

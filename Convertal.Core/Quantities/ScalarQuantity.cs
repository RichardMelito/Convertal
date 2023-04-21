// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convertal.Core.Extensions;

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

public sealed class ScalarEmptyQuantity : ScalarQuantity
{
    public override ScalarEmptyUnit FundamentalUnit => Database.ScalarEmptyUnit;

    internal ScalarEmptyQuantity(Database database)
        : base(database, null, null)
    {
        SettableBaseQuantityComposition = ScalarComposition<IBaseQuantity>.Empty;
        Init();
    }

    public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        toIgnore = toIgnore.UnionAppend(this);
        return Array.Empty<IMaybeNamed>().SortByTypeAndName();
    }

    protected override void DisposeBody(bool disposeDependents)
    {
        // The Database.EmptyQuantity cannot be disposed
        return;
    }

    public override EmptyQuantityProto ToProto()
    {
        return new();
    }
}

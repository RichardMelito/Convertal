// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public abstract class VectorQuantity : Quantity, IVector<VectorQuantity, ScalarQuantity>
{
    // Must be set at construciton by derived types
    protected VectorComposition<IBaseQuantity> SettableBaseQuantityComposition { get; init; } = null!;
    public override VectorComposition<IBaseQuantity> BaseQuantityComposition => SettableBaseQuantityComposition;

    public override bool IsVector => true;

    // TODO needs default generation
    public abstract ScalarQuantity ScalarAnalog { get; }

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

public sealed class VectorEmptyQuantity : VectorQuantity
{
    public override VectorEmptyUnit FundamentalUnit => Database.VectorEmptyUnit;

    internal VectorEmptyQuantity(Database database)
        : base(database, null, null)
    {
        SettableBaseQuantityComposition = VectorComposition<IBaseQuantity>.Empty;
        Init();
    }

    // TODO copy-paste of ScalarEmptyQuantity's impelmenation
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

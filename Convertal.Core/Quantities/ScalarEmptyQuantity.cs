// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public sealed class ScalarEmptyQuantity : ScalarQuantity
{
    public override ScalarEmptyUnit FundamentalUnit => Database.ScalarEmptyUnit;

    public override VectorEmptyQuantity VectorAnalog => Database.VectorEmptyQuantity;

    internal ScalarEmptyQuantity(Database database)
        : base(database, ScalarComposition<IBaseQuantity>.Empty, null, null)
    {
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

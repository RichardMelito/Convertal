﻿// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class ScalarBaseQuantity : ScalarQuantity, IBaseQuantity
{
    internal IScalarBaseUnit? SettableFundamentalUnit { get; set; }
    public override IScalarBaseUnit FundamentalUnit => SettableFundamentalUnit!;

    internal VectorBaseQuantity? SettableVectorAnalog { get; set; }
    public override VectorBaseQuantity? VectorAnalog => SettableVectorAnalog;

    internal ScalarBaseQuantity(Database database, string? name, string? symbol)
        : base(database, name, symbol)
    {
        SettableBaseQuantityComposition = new(this);
        Init();
    }

    // TODO
    public override BaseQuantityProto ToProto()
    {
        return new(Name!, Symbol, FundamentalUnit.Name!);
    }

    public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        var res = base.GetAllDependents(ref toIgnore).AsEnumerable();

        var quantsComposedOfThis = from comp_quant in Database.CompositionAndQuantitiesDictionary
                                   where comp_quant.Value is IDerivedQuantity &&
                                   comp_quant.Key.ContainsKey(this)
                                   select comp_quant.Value;

        res = res.Union(quantsComposedOfThis);
        foreach (var dependentQuantity in quantsComposedOfThis.Except(toIgnore))
            res = res.Union(dependentQuantity.GetAllDependents(ref toIgnore));

        res.ThrowIfSetContains(this);
        return res.SortByTypeAndName();
    }
}
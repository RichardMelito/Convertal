// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class VectorBaseQuantity : VectorQuantity, IBaseQuantity
{
    public override IVectorBaseUnit FundamentalUnit => (IVectorBaseUnit)base.FundamentalUnit;

    public override ScalarBaseQuantity ScalarAnalog => (ScalarBaseQuantity)base.ScalarAnalog;

    internal VectorBaseQuantity(ScalarBaseQuantity scalarAnalog, string? name, string? symbol)
        : base(scalarAnalog, null, name, symbol)
    {
        Init();
    }

    public override VectorBaseQuantityProto ToProto()
    {
        return new(Name!, Symbol, ScalarAnalog.Name!);
    }

    // TODO copy-paste of ScalarBaseQuantity's implementation
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

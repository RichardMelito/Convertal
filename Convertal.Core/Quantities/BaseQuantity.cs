// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public record BaseQuantityProto(
    string Name,
    string? Symbol,
    [property: JsonPropertyOrder(2)] string FundamentalUnit) : MaybeNamedProto(Name, Symbol);

public class BaseQuantity : Quantity, IBase, IEquatable<BaseQuantity>
{
    internal IBaseUnit? InnerFundamentalUnit { get; set; } = null;

    public override IBaseUnit FundamentalUnit => InnerFundamentalUnit!;

    public override NamedComposition<BaseQuantity> BaseQuantityComposition { get; }

    internal BaseQuantity(Database database, string name, string? symbol)
        : base(database, name, symbol)
    {
        BaseQuantityComposition = new NamedComposition<BaseQuantity>(this);
        Init();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public bool Equals(BaseQuantity? other)
    {
        return base.Equals(other);
    }

    public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        var res = base.GetAllDependents(ref toIgnore).AsEnumerable();

        var quantsComposedOfThis = from comp_quant in Database.CompositionAndQuantitiesDictionary
                                   where comp_quant.Value is DerivedQuantity &&
                                   comp_quant.Key.ContainsKey(this)
                                   select comp_quant.Value;

        res = res.Union(quantsComposedOfThis);
        foreach (var dependentQuantity in quantsComposedOfThis.Except(toIgnore))
            res = res.Union(dependentQuantity.GetAllDependents(ref toIgnore));

        res.ThrowIfSetContains(this);
        return res.SortByTypeAndName();
    }

    public override BaseQuantityProto ToProto()
    {
        return new(Name!, Symbol, FundamentalUnit.Name!);
    }
}

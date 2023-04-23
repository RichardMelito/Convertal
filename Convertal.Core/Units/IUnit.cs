// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public interface IUnit : IVectorOrScalar, IMaybeNamed, IEquatable<IUnit>, IComparable<IUnit>
{
    bool IsFundamental => Quantity.FundamentalUnit.Equals(this);

    decimal FundamentalOffset { get; }
    /*  K is fundamental
     *  C = K - 273
     *  C's FundamentalOffset = +273
     */

    Quantity Quantity { get; }

    NamedComposition<IUnit> UnitComposition { get; }

    /// <summary>
    /// Used for serialization. If <see cref="UnitComposition"/> is composed of just this instance, then this property returns null.
    /// </summary>
    NamedComposition<IUnit>? OtherUnitComposition => GetOtherUnitComposition();


    NamedComposition<IUnit>? GetOtherUnitComposition()
    {
        if (UnitComposition.Count == 1 && UnitComposition.ContainsKey(this))
            return null;

        return UnitComposition;
    }
    bool IEquatable<IUnit>.Equals(IUnit? other)
    {
        var castThis = (IMaybeNamed)this;
        var castOther = other as IMaybeNamed;
        return castThis.Equals(castOther);
    }

    static IOrderedEnumerable<IMaybeNamed> GetAllDependents(IUnit unit, ref IEnumerable<IMaybeNamed> toIgnore)
    {
        toIgnore = toIgnore.UnionAppend(unit);

        var dependentQuants =
            from quant in unit.Quantity.Database.CompositionAndQuantitiesDictionary.Values
            where quant.FundamentalUnit == unit
            select quant;

        var res = dependentQuants.Cast<IMaybeNamed>();
        foreach (var quant in dependentQuants.Except(toIgnore))
            res = res.Union(quant.GetAllDependents(ref toIgnore));

        res.ThrowIfSetContains(unit);
        return res.SortByTypeAndName();
    }

    IOrderedEnumerable<IMaybeNamed> IMaybeNamed.GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        return GetAllDependents(this, ref toIgnore);
    }

    int IComparable<IUnit>.CompareTo(IUnit? other)
    {
        return MaybeNamed.MaybeNameComparer.PerformCompare(this, other);
    }
}

// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public record EmptyUnitProto() : MaybeNamedProto(null, null);

// should maybe inherit from Unit?
public sealed class EmptyUnit : IUnit
{
    public decimal FundamentalMultiplier => 1m;
    public decimal FundamentalOffset => 0;

    public Quantity Quantity { get; }

    public NamedComposition<IUnit> UnitComposition => NamedComposition<IUnit>.Empty;

    public string? Name => null;

    public string? Symbol => null;

    public Database Database => Quantity.Database;

    internal EmptyUnit(Database database)
    {
        Quantity = database.EmptyQuantity;
    }

    public override string ToString()
    {
        return "";
    }

    public string ToStringSymbol()
    {
        return "";
    }

    public override bool Equals(object? other)
    {
        var castThis = (IMaybeNamed)this;
        var castOther = other as IMaybeNamed;
        return castThis.Equals(castOther);
    }

    public override int GetHashCode()
    {
        return ((IMaybeNamed)this).CalculateHashCode();
    }

    void IMaybeNamed.DisposeThisAndDependents(bool disposeDependents)
    {
        // The EmptyUnit cannot be disposed
        return;
    }

    public void Dispose()
    {
        // The EmptyUnit cannot be disposed
        return;
    }

    public IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        toIgnore = toIgnore.UnionAppend(this);
        return Array.Empty<IMaybeNamed>().SortByTypeAndName();
    }

    public EmptyUnitProto ToProto() => new();

    MaybeNamedProto IMaybeNamed.ToProto() => ToProto();
}

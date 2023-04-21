// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using Convertal.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Convertal.Core;

public sealed class ScalarEmptyUnit : IScalarUnit
{
    public decimal FundamentalMultiplier => 1m;

    public decimal FundamentalOffset => 0;

    public Quantity Quantity { get; }

    NamedComposition<IUnit> IUnit.UnitComposition => UnitComposition;
    public ScalarComposition<IUnit> UnitComposition => ScalarComposition<IUnit>.Empty;

    public Database Database => Quantity.Database;

    public string? Name => null;

    public string? Symbol => null;

    public VectorEmptyUnit? VectorAnalog => Database.VectorEmptyUnit;

    IVectorUnit? IScalar<IScalarUnit, IVectorUnit>.VectorAnalog => VectorAnalog;

    internal ScalarEmptyUnit(Database database)
    {
        Quantity = Database.ScalarEmptyQuantity;
    }

    public void Dispose()
    {
        // EmptyUnits cannot be disposed
        return;
    }

    void IMaybeNamed.DisposeThisAndDependents(bool disposeDependents)
    {
        // EmptyUnits cannot be disposed
        return;
    }

    public IScalarUnit Pow(decimal power) => this;

    public EmptyUnitProto ToProto() => new();

    MaybeNamedProto IMaybeNamed.ToProto() => ToProto();

    public override string ToString() => "";

    public string ToStringSymbol() => "";

    public IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        toIgnore = toIgnore.UnionAppend(this);
        return Array.Empty<IMaybeNamed>().SortByTypeAndName();
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
}

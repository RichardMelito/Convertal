// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public sealed class VectorEmptyUnit : IVectorUnit
{
    public decimal FundamentalMultiplier => 1m;

    public decimal FundamentalOffset => 0;

    public Quantity Quantity { get; }

    public VectorComposition<IUnit> UnitComposition => VectorComposition<IUnit>.Empty;

    public Database Database => Quantity.Database;

    public string? Name => null;

    public string? Symbol => null;

    public ScalarEmptyUnit ScalarAnalog => Database.ScalarEmptyUnit;

    NamedComposition<IUnit> IUnit.UnitComposition => UnitComposition;

    IScalarUnit IVector<IVectorUnit, IScalarUnit>.ScalarAnalog => ScalarAnalog;

    internal VectorEmptyUnit(Database database)
    {
        Quantity = Database.VectorEmptyQuantity;
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

    public IVectorUnit Pow(decimal power) => this;

    // TODO these are all just copy-paste of ScalarEmptyUnit's implementation
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

    public IScalarUnit DotP(IVectorUnit other) => throw new NotImplementedException();
    public IVectorUnit CrossP(IVectorUnit other) => throw new NotImplementedException();
}

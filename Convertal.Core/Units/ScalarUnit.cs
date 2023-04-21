// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using Convertal.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Convertal.Core;

public abstract class ScalarUnit : Unit, IScalarUnit
{
    public override bool IsVector => false;

    public override ScalarComposition<IUnit> UnitComposition => (ScalarComposition<IUnit>)base.UnitComposition;

    // TODO consider using generics for this sort of this
    public abstract IVectorUnit? VectorAnalog { get; }

    // for defining from a chain of operations
    internal ScalarUnit(
        Database database,
        string name,
        ScalarComposition<IUnit> composition)
        : base(database, name, composition)
    {
    }

    // TODO fix method reference
    /// <summary>
    /// Only to be called from <see cref="BaseQuantity.DefineNewBaseQuantity(string, string, Prefix?)"/>
    /// </summary>
    internal ScalarUnit(
        Database database,
        string? name,
        ScalarQuantity quantity,
        decimal fundamentalMultiplier,
        ScalarComposition<IUnit>? composition = null,
        string? symbol = null)
        : base(database, name, quantity, fundamentalMultiplier, composition, symbol)
    {
    }

    // for defining from an existing IScalarUnit
    internal ScalarUnit(
        Database database,
        string? name,
        IScalarUnit otherUnit,
        decimal multiplier,
        decimal offset,
        string? symbol)
        : base(database, name, otherUnit, multiplier, offset, symbol)
    {
    }


    // TODO fix method reference
    /// <summary>
    /// To be called only from <see cref="Database.DefineBaseUnit(UnitProto)"/>
    /// </summary>
    internal ScalarUnit(
        Database database,
        string? name,
        string? symbol,
        ScalarQuantity quantity,
        decimal fundamentalMultiplier,
        decimal fundamentalOffset,
        ScalarComposition<IUnit>? composition)
        : base(database, name, symbol, quantity, fundamentalMultiplier, fundamentalOffset, composition)
    {
    }

    // TODO
    public IScalarUnit Pow(decimal power) => throw new NotImplementedException();
}

public abstract class ScalarPrefixedUnit : PrefixedUnit, IScalarUnit
{
    public override bool IsVector => false;
    public override ScalarUnit Unit => (ScalarUnit)base.Unit;

    public abstract VectorPrefixedUnit? VectorAnalog { get; }
    IVectorUnit? IScalar<IScalarUnit, IVectorUnit>.VectorAnalog => VectorAnalog;

    protected ScalarPrefixedUnit(Database database, ScalarUnit unit, Prefix prefix)
        : base(database, unit, prefix)
    {
    }

    // TODO
    public IScalarUnit Pow(decimal power) => throw new NotImplementedException();
}

public sealed class ScalarEmptyUnit : IScalarUnit
{
    public decimal FundamentalMultiplier => 1m;

    public decimal FundamentalOffset => 0;

    public Quantity Quantity { get; }

    public ScalarComposition<IUnit> UnitComposition => ScalarComposition<IUnit>.Empty;

    public Database Database => Quantity.Database;

    public string? Name => null;

    public string? Symbol => null;

    public IVectorUnit? VectorAnalog => Database.VectorEmptyUnit;

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

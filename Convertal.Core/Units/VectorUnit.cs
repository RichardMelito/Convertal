// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public abstract class VectorUnit : Unit, IVectorUnit
{
    public override bool IsVector => true;

    public override VectorComposition<IUnit> UnitComposition => (VectorComposition<IUnit>)base.UnitComposition;

    // TODO
    public abstract IScalarUnit ScalarAnalog { get; }

    // for defining from a chain of operations
    internal VectorUnit(
        Database database,
        string name,
        VectorComposition<IUnit> composition)
        : base(database, name, composition)
    {
    }

    // TODO fix method reference
    /// <summary>
    /// Only to be called from <see cref="BaseQuantity.DefineNewBaseQuantity(string, string, Prefix?)"/>
    /// </summary>
    internal VectorUnit(
        Database database,
        string? name,
        VectorQuantity quantity,
        decimal fundamentalMultiplier,
        VectorComposition<IUnit>? composition = null,
        string? symbol = null)
        : base(database, name, quantity, fundamentalMultiplier, composition, symbol)
    {
    }

    // for defining from an existing IVectorUnit
    internal VectorUnit(
        Database database,
        string? name,
        IVectorUnit otherUnit,
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
    internal VectorUnit(
        Database database,
        string? name,
        string? symbol,
        VectorQuantity quantity,
        decimal fundamentalMultiplier,
        decimal fundamentalOffset,
        VectorComposition<IUnit>? composition)
        : base(database, name, symbol, quantity, fundamentalMultiplier, fundamentalOffset, composition)
    {
    }

    // TODO
    public IScalarUnit DotP(IVectorUnit other) => throw new NotImplementedException();
    public IVectorUnit CrossP(IVectorUnit other) => throw new NotImplementedException();
}

public abstract class VectorPrefixedUnit : PrefixedUnit, IVectorUnit
{
    public override bool IsVector => true;
    public override VectorUnit Unit => (VectorUnit)base.Unit;

    // TODO
    public abstract ScalarPrefixedUnit ScalarAnalog { get; }
    IScalarUnit IVector<IVectorUnit, IScalarUnit>.ScalarAnalog => ScalarAnalog;

    protected VectorPrefixedUnit(Database database, VectorUnit unit, Prefix prefix)
        : base(database, unit, prefix)
    {
    }

    // TODO
    public IScalarUnit DotP(IVectorUnit other) => throw new NotImplementedException();
    public IVectorUnit CrossP(IVectorUnit other) => throw new NotImplementedException();
}

public sealed class VectorEmptyUnit : IVectorUnit
{
    public decimal FundamentalMultiplier => 1m;

    public decimal FundamentalOffset => 0;

    public Quantity Quantity { get; }

    public VectorComposition<IUnit> UnitComposition => VectorComposition<IUnit>.Empty;

    public Database Database => Quantity.Database;

    public string? Name => null;

    public string? Symbol => null;

    public IScalarUnit? VectorAnalog => Database.ScalarEmptyUnit;

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
}

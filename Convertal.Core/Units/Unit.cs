// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Convertal.Core.Extensions;
using DecimalMath;

namespace Convertal.Core;

public record UnitProto(
    string? Name,
    string? Symbol,
    [property: JsonPropertyOrder(2)] string Quantity,
    [property: JsonPropertyOrder(3)] decimal FundamentalMultiplier,
    [property: JsonPropertyOrder(4), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] decimal FundamentalOffset,
    [property: JsonPropertyOrder(5)] ValueEqualityDictionary<string, decimal>? OtherUnitComposition)
    : MaybeNamedProto(Name, Symbol);

public abstract class Unit : MaybeNamed, IUnit
{
    private bool _disposed = false;

    public virtual Quantity Quantity { get; }

    public decimal FundamentalMultiplier { get; }

    // TODO what even is this? I can't remember
    public NamedComposition<IUnit>? OtherUnitComposition => ((IUnit)this).GetOtherUnitComposition();

    public virtual NamedComposition<IUnit> UnitComposition { get; private set; }

    public abstract bool IsVector { get; }

    // only to be called when defining fundamental units for new
    // quantities, and thus offset will always be 0
    protected Unit(
        Database database,
        string? name,
        Quantity quantity,
        decimal fundamentalMultiplier,
        NamedComposition<IUnit>? composition = null,
        string? symbol = null)
        : base(database, name, symbol)
    {
        Quantity = quantity;
        FundamentalMultiplier = fundamentalMultiplier;
        composition?.ThrowIfRecursive(this);
        UnitComposition = composition ?? NamedComposition<IUnit>.Make(this);
    }

    protected Unit(
        Database database,
        string? name,
        IUnit otherUnit,
        decimal multiplier,
        string? symbol)
        : base(database, name, symbol)
    {
        Quantity = otherUnit.Quantity;
        FundamentalMultiplier = otherUnit.FundamentalMultiplier * multiplier;
        UnitComposition = NamedComposition<IUnit>.Make(this);
    }

    // for defining from a chain of operations
    protected Unit(Database database, string name, NamedComposition<IUnit> composition)
        : base(database, name)
    {
        ArgumentNullException.ThrowIfNull(composition);
        // TODO: notify user that offsets will be ignored
        //var offsetQuery =
        //    from baseUnit in composition.Keys
        //    where baseUnit.FundamentalOffset != 0m
        //    select baseUnit;

        composition.ThrowIfRecursive(this);
        UnitComposition = composition;
        Quantity = Database.GetQuantityFromBaseComposition(UnitComposition);
        FundamentalMultiplier = 1m;
        foreach (var (unit, power) in UnitComposition)
        {
            var multiplier = DecimalEx.Pow(unit.FundamentalMultiplier, power);
            FundamentalMultiplier *= multiplier;
        }
    }

    // deserialization constructor
    protected Unit(Database database, string? name, string? symbol, Quantity quantity, decimal fundamentalMultiplier, NamedComposition<IUnit>? composition)
        : base(database, name, symbol)
    {
        Quantity = quantity;
        FundamentalMultiplier = fundamentalMultiplier;
        composition?.ThrowIfRecursive(this);
        UnitComposition = composition!; // May be null, which indicates that it must be set later from the SetUnitComposition method.
    }

    /// To be called only from <see cref="Database.DefineVectorBaseUnit(UnitProto)"/>,
    /// <see cref="Database.DefineScalarBaseUnit(UnitProto)"/>,
    /// <see cref="Database.DefineVectorDerivedUnit(UnitProto)"/>, and
    /// <see cref="Database.DefineScalarDerivedUnit(UnitProto)"/>.
    internal void SetUnitComposition(NamedComposition<IUnit> unitComposition)
    {
        if (!UnitComposition.IsComposedOfOne(this))
            throw new InvalidOperationException();

        if (IsVector != unitComposition.IsVector)
            throw new InvalidOperationException();

        UnitComposition = unitComposition;
    }

    // TODO
    public override UnitProto ToProto()
    {
        return new(
            Name,
            Symbol,
            Quantity.ToString(),
            FundamentalMultiplier,
            0,
            OtherUnitComposition is null ? null : new(OtherUnitComposition.CompositionAsStringDictionary));
    }
    protected override Type GetDatabaseType() => typeof(Unit);

    public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        var res = IUnit.GetAllDependents(this, ref toIgnore).AsEnumerable();

        var prefixedUnitsWithThis =
            from prefixedUnit in Database.PrefixedUnits
            where prefixedUnit.Unit == this
            select prefixedUnit;

        res = res.Union(prefixedUnitsWithThis);
        foreach (var prefixedUnit in prefixedUnitsWithThis.Except(toIgnore))
            res = res.Union(prefixedUnit.GetAllDependents(ref toIgnore));

        res.ThrowIfSetContains(this);
        return res.SortByTypeAndName();
    }

    public override string ToString()
    {
        return Name ?? UnitComposition!.ToString();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Quantity, FundamentalMultiplier);
    }

    protected override void DisposeBody(bool disposeDependents)
    {
        if (_disposed)
            return;

        if (((IUnit)this).IsFundamental && !Quantity.Disposed)
            throw new InvalidOperationException($"Cannot dispose of" +
                $" fundamental unit {this} without first disposing of " +
                $"quantity {Quantity}.");

        var allSystems = Database.GetAllMaybeNameds<MeasurementSystem>();
        foreach (var system in allSystems)
            system.RemoveUnit(this);

        _disposed = true;
        base.DisposeBody(disposeDependents);
    }

    //public abstract IVectorOrScalar ToScalar();
    //public abstract IVectorOrScalar? ToVector();
}

// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core;

public abstract class Quantity : MaybeNamed
{
    /*  There will never be multiple quantities for something in the same 
     *  way there are multiple units for a quantity. So there's F, C, K, etc. 
     *  BaseUnits for Temperature, but there's only 1 Temperature.
     *  
     *  FundamentalUnit
     *  BaseQuantityComposition
     *  
     *  MaybeName - Unnamed namespace? HasName? Temporary names?
     *      - temporaries have Unnamed namespace and fullname of their composition
     */

    private bool _disposed = false;
    private bool _initialized = false;

    public bool Disposed => _disposed;

    public abstract IUnit FundamentalUnit { get; }

    public abstract NamedComposition<BaseQuantity> BaseQuantityComposition { get; }

    protected Quantity(Database database, string? name, string? symbol)
        : base(database, name, symbol)
    {

    }

    protected override Type GetDatabaseType() => typeof(Quantity);

    public override string ToString()
    {
        return Name ?? BaseQuantityComposition.ToString();
    }

    protected void Init()
    {
        if (_initialized)
            throw new ApplicationException($"Quantity {Name ?? "{null}"} is already initialized.");

        if (Database.QuantitiesByComposition.ContainsValue(this))
            throw new ApplicationException($"Quantity {Name ?? "{null}"} is already within the dictionary.");

        Database.QuantitiesByComposition.Add(BaseQuantityComposition, this);
        _initialized = true;
    }

    public Quantity Pow(decimal power)
    {
        return Database.GetFromBaseComposition(BaseQuantityComposition.Pow(power));
    }

    public Quantity MultiplyOrDivide(Quantity lhs, Quantity rhs, bool multiplication)
    {
        var resultingComposition = NamedComposition<BaseQuantity>.MultiplyOrDivide(
            lhs.BaseQuantityComposition,
            rhs.BaseQuantityComposition,
            multiplication: multiplication);

        return Database.GetFromBaseComposition(resultingComposition);
    }

    public static Quantity operator *(Quantity lhs, Quantity rhs)
    {
        return lhs.MultiplyOrDivide(lhs, rhs, multiplication: true);
    }

    public static Quantity operator /(Quantity lhs, Quantity rhs)
    {
        return lhs.MultiplyOrDivide(lhs, rhs, multiplication: false);
    }

    public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        toIgnore = toIgnore.UnionAppend(this);

        var allUnits = Database.GetAllMaybeNameds<Unit>();
        var unitsWithThisQuantity = from unit in allUnits
                                    where unit.Quantity == this
                                    select unit;

        var res = unitsWithThisQuantity.Cast<IMaybeNamed>();
        foreach (var unit in unitsWithThisQuantity.Except(toIgnore))
            res = res.Union(unit.GetAllDependents(ref toIgnore));

        res.ThrowIfSetContains(this);
        return res.SortByTypeAndName();
    }



    protected override void DisposeBody(bool disposeDependents)
    {
        if (_disposed)
            return;

        if (!Database.QuantitiesByComposition.Remove(BaseQuantityComposition))
            throw new ApplicationException(
                $"Could not remove Quantity {Name ?? "{null}"} with composition " +
                $"{BaseQuantityComposition} from static dictionary.");

        var allSystems = Database.GetAllMaybeNameds<MeasurementSystem>();
        foreach (var system in allSystems)
            system.RemoveQuantity(this);

        _disposed = true;
        base.DisposeBody(disposeDependents);
    }
}

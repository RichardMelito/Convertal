// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core;

[JsonConverter(typeof(JsonConverters.DatabaseConverter))]
public class Database
{
    private readonly List<PrefixedUnit> _prefixedUnits = new();

    public IEnumerable<Prefix> Prefixes => GetAllMaybeNameds<Prefix>();
    public IEnumerable<BaseQuantity> BaseQuantitys => GetAllMaybeNameds<BaseQuantity>();
    public IEnumerable<DerivedQuantity> DerivedQuantitys => GetAllMaybeNameds<DerivedQuantity>();
    public IEnumerable<BaseUnit> BaseUnits => GetAllMaybeNameds<BaseUnit>();
    public IEnumerable<PrefixedBaseUnit> PrefixedBaseUnits => _prefixedUnits.Where(x => x is PrefixedBaseUnit).Cast<PrefixedBaseUnit>();
    public IEnumerable<DerivedUnit> DerivedUnits => GetAllMaybeNameds<DerivedUnit>();
    public IEnumerable<PrefixedDerivedUnit> PrefixedDerivedUnits => _prefixedUnits.Where(x => x is PrefixedDerivedUnit).Cast<PrefixedDerivedUnit>();
    public IEnumerable<MeasurementSystem> MeasurementSystems => GetAllMaybeNameds<MeasurementSystem>();

    internal Dictionary<Type, List<MaybeNamed>> MaybeNamedsByType { get; } = new();
    internal Dictionary<NamedComposition<BaseQuantity>, Quantity> QuantitiesByComposition { get; } = new();

    public EmptyQuantity EmptyQuantity { get; }

    public EmptyUnit EmptyUnit { get; }

    public IReadOnlyDictionary<NamedComposition<BaseQuantity>, Quantity> CompositionAndQuantitiesDictionary { get; }

    public ReadOnlyCollection<PrefixedUnit> PrefixedUnits { get; }

    /*  Move name lookup/storage stuff from MaybeNamed into here
     *  CRUD for named objects occurs here
     */

    public Database()
    {
        AddTypeToDictionary<MeasurementSystem>();
        AddTypeToDictionary<Prefix>();
        AddTypeToDictionary<Quantity>();
        AddTypeToDictionary<Unit>();

        CompositionAndQuantitiesDictionary = QuantitiesByComposition.AsReadOnly();
        EmptyQuantity = new(this);
        EmptyUnit = new(this);
        PrefixedUnits = _prefixedUnits.AsReadOnly();
    }

    internal void AddToPrefixedUnitsList(PrefixedUnit toAdd) => _prefixedUnits.Add(toAdd);
    internal bool RemoveFromPrefixedUnitsList(PrefixedUnit toRemove) => _prefixedUnits.Remove(toRemove);

    public Prefix DefinePrefix(string name, decimal multiplier, string? symbol = null) => new(this, name, multiplier, symbol);
    internal Prefix DefinePrefix(PrefixProto proto) => new(this, proto.Name!, proto.Multiplier, proto.Symbol);

    public PrefixedBaseUnit GetPrefixedUnit(BaseUnit unit, Prefix prefix)
    {
        return (PrefixedBaseUnit)GetPrefixedUnit((Unit)unit, prefix);
    }

    public PrefixedDerivedUnit GetPrefixedUnit(DerivedUnit unit, Prefix prefix)
    {
        return (PrefixedDerivedUnit)GetPrefixedUnit((Unit)unit, prefix);
    }

    public PrefixedUnit GetPrefixedUnit(Unit unit, Prefix prefix)
    {
        var existingPrefixedUnit =
            from prefixedUnit in PrefixedUnits
            where prefixedUnit.Unit == unit &&
            prefixedUnit.Prefix == prefix
            select prefixedUnit;

        switch (existingPrefixedUnit.Count())
        {
            case 0:
                if (unit is BaseUnit baseUnit)
                    return new PrefixedBaseUnit(unit.Database, baseUnit, prefix);

                else if (unit is DerivedUnit derivedUnit)
                    return new PrefixedDerivedUnit(unit.Database, derivedUnit, prefix);

                else
                    throw new NotImplementedException();

            case 1:
                return existingPrefixedUnit.First();

            default:
                throw new ApplicationException(existingPrefixedUnit.Count().ToString());
        }
    }

    public IEnumerable<T> GetAllMaybeNameds<T>()
        where T : MaybeNamed
    {
        var typeofT = typeof(T);
        var typeWithinDictionary = GetTypeWithinDictionary(typeofT)!;
        if (typeofT == typeWithinDictionary)
            return MaybeNamedsByType[typeWithinDictionary].Cast<T>();

        return MaybeNamedsByType[typeWithinDictionary]
            .Where(x => x is T)
            .Cast<T>();
    }

    public Type? GetTypeWithinDictionary(Type type)
    {
        var originalType = type;

        while (!MaybeNamedsByType.ContainsKey(type))
        {
            if (type.BaseType is null)
                return null;

            //if (type.BaseType is null)
            //    throw new ArgumentException($"Neither type {originalType.Name} " +
            //        $"nor any of its base types are within the name lookup dictionary.");

            type = type.BaseType;
        }

        return type;
    }

    internal void AddTypeToDictionary<T>()
        where T : MaybeNamed
    {
        AddTypeToDictionary(typeof(T));
    }

    internal void AddTypeToDictionary(Type type)
    {
        if (!MaybeNamedsByType.ContainsKey(type))
            MaybeNamedsByType.Add(type, new List<MaybeNamed>());
    }

    public T? FromString<T>(string str)
        where T : IMaybeNamed
    {
        var type = typeof(T);
        if (type.IsSubclassOf(typeof(MaybeNamed)))
        {
            if (TryGetFromName(str, type, out var res))
                return res!.ForceCast<T>();

            else
                return default;
        }

        if (type.IsSubclassOf(typeof(PrefixedUnit)))
        {
            var split = str.Split('_');
            if (split.Length != 2)
                throw new ArgumentException(str);

            var prefix = GetFromName<Prefix>(split[0]);
            var unit = GetFromName<Unit>(split[1]);
            return GetPrefixedUnit(unit, prefix).ForceCast<T>();
        }

        throw new ArgumentException(str);

        //T? res = default;
        //res = res switch
        //{
        //    MaybeNamed => (T)new Object(),
        //    _ => throw new Exception()
        //};

        //return res;
    }



    public bool TryGetFromName(
        string name,
        Type type,
        [NotNullWhen(true)] out MaybeNamed? namedObject,
        bool isSymbol = false)
    {
        var typeWithinDictionary = GetTypeWithinDictionary(type);
        if (typeWithinDictionary is null)
        {
            namedObject = null;
            return false;
        }

        var nameds = MaybeNamedsByType[typeWithinDictionary];
        MaybeNamed[] matches;
        if (isSymbol)
        {
            matches = (from named in nameds
                       where named.Symbol == name
                       select named).ToArray();
        }
        else
        {
            matches = (from named in nameds
                       where named.Name == name
                       select named).ToArray();
        }

        if (matches.Length == 1)
        {
            namedObject = matches.First();
            return true;
        }
        else if (matches.Length == 0)
        {
            namedObject = null;
            return false;
        }
        else
        {
            throw new ApplicationException();
        }
    }

    public bool TryGetFromName<T>(
        string name,
        [NotNullWhen(true)] out T? namedObject,
        bool isSymbol = false)
        where T : MaybeNamed
    {
        TryGetFromName(name, typeof(T), out var named, isSymbol);
        namedObject = named as T;
        return namedObject is not null;
    }

    public T GetFromName<T>(string name, bool isSymbol = false)
        where T : MaybeNamed
    {
        if (TryGetFromName<T>(name, out var res, isSymbol))
            return res!;

        throw new InvalidOperationException($"No instances of " +
            $"{typeof(T).Name} with {(isSymbol ? "symbol" : "name")} {name}.");
    }



    public void ThrowIfNameNotValid<T>(string name, bool isSymbol = false)
        where T : MaybeNamed
    {
        ThrowIfNameNotValid(name, GetTypeWithinDictionary(typeof(T))!, isSymbol);
    }

    public void ThrowIfNameNotValid(string name, Type type, bool isSymbol = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name must not be empty.");

        if (decimal.TryParse(name, out _))
            throw new ArgumentException("Name must not be a number.");

        if (!name.All(char.IsLetterOrDigit))
            throw new ArgumentException("Name must be composed of alphanumeric characters.");

        if (NameAlreadyRegistered(name, type, isSymbol))
        {
            throw new InvalidOperationException($"There is already a {type.Name} " +
                $"with {(isSymbol ? "symbol" : "name")} of {name}.");
        }
    }
    public bool NameIsValid<T>(string name, bool isSymbol = false)
        where T : MaybeNamed
    {
        return NameIsValid(name, GetTypeWithinDictionary(typeof(T))!, isSymbol);
    }

    private bool NameIsValid(string name, Type type, bool isSymbol)
    {
        try
        {
            ThrowIfNameNotValid(name, type, isSymbol);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool NameAlreadyRegistered<T>(string name, bool isSymbol = false)
        where T : MaybeNamed
    {
        return NameAlreadyRegistered(name, GetTypeWithinDictionary(typeof(T))!, isSymbol);
    }

    public bool NameAlreadyRegistered(string name, Type type, bool isSymbol = false)
    {
        type = GetTypeWithinDictionary(type)!;

        IEnumerable<MaybeNamed> matches;
        if (isSymbol)
        {
            matches = from named in MaybeNamedsByType[type]
                      where named.Symbol == name
                      select named;
        }
        else
        {
            matches = from named in MaybeNamedsByType[type]
                      where named.Name == name
                      select named;
        }

        return matches.Any();
    }

    public BaseUnit DefineBaseUnit(
        string name,
        IBaseUnit otherUnit,
        decimal multiplier,
        decimal offset = 0,
        string? symbol = null) => new(this, name, otherUnit, multiplier, offset, symbol);

    internal BaseUnit DefineBaseUnit(UnitProto proto)
    {
        NamedComposition<IUnit>? composition = null;
        if (proto.OtherUnitComposition is not null)
        {
            composition = new(proto.OtherUnitComposition.ToDictionary(
                kvp => ParseIUnit(kvp.Key),
                kvp => kvp.Value));
        }

        // True if this is a fundamental unit
        if (proto.Name is not null && TryGetFromName<BaseUnit>(proto.Name, out var unit))
        {
            if (proto.Symbol is not null)
                unit.ChangeSymbol(proto.Symbol);

            if (composition is not null)
                unit.UnitComposition = composition;

            return unit;
        }

        return new BaseUnit(
            this,
            proto.Name!,
            proto.Symbol,
            GetFromName<BaseQuantity>(proto.Quantity),
            proto.FundamentalMultiplier,
            proto.FundamentalOffset,
            composition);
    }

    public IUnit ParseIUnit(string toParse, bool isSymbol = false)
    {
        var split = toParse.Split('_');
        switch (split.Length)
        {
            case 2:
                {
                    var prefix = GetFromName<Prefix>(split[0], isSymbol);
                    var unit = GetFromName<Unit>(split[1], isSymbol);
                    return GetPrefixedUnit(unit, prefix);
                }

            case 1:
                return GetFromName<Unit>(split[0], isSymbol);

            default:
                throw new ArgumentException(toParse);
        }
    }

    public DerivedUnit DefineDerivedUnit(
        string name,
        IDerivedUnit otherUnit,
        decimal multiplier,
        decimal offset = 0,
        string? symbol = null)
        => new(this, name, otherUnit, multiplier, offset, symbol);

    internal DerivedUnit DefineDerivedUnit(UnitProto proto)
    {
        NamedComposition<IUnit>? composition = null;
        if (proto.OtherUnitComposition is not null)
        {
            composition = new(proto.OtherUnitComposition.ToDictionary(
                kvp => ParseIUnit(kvp.Key),
                kvp => kvp.Value));
        }

        // True if this is a fundamental unit
        if (proto.Name is not null && TryGetFromName<DerivedUnit>(proto.Name, out var unit))
        {
            if (proto.Symbol is not null)
                unit.ChangeSymbol(proto.Symbol);

            if (composition is not null && unit.UnitComposition is null)
                unit.UnitComposition = composition;

            return unit;
        }

        return new DerivedUnit(
            this,
            proto.Name!,
            proto.Symbol,
            GetFromName<DerivedQuantity>(proto.Quantity),
            proto.FundamentalMultiplier,
            proto.FundamentalOffset,
            composition);
    }

    public DerivedQuantity DefineDerivedQuantity(
        Func<Quantity> quantityOperation,
        string quantityName,
        string? quantitySymbol = null)
    {
        var resultingQuantity = quantityOperation();
        var res = resultingQuantity as DerivedQuantity;
        if (res is null)
        {
            resultingQuantity.Dispose();

            // TODO elaborate and maybe do some magic to write out the operation inputs.
            throw new InvalidOperationException("The given quantity operation did not return a derived quantity.");
        }

        res.ChangeNameAndSymbol(quantityName, quantitySymbol);
        return res;
    }

    internal DerivedQuantity DefineDerivedQuantity(DerivedQuantityProto proto)
    {
        NamedComposition<BaseQuantity> composition = new(proto.BaseQuantityComposition
            .ToDictionary(kvp => GetFromName<BaseQuantity>(kvp.Key), kvp => kvp.Value));

        DerivedQuantity res = new(this, composition, proto.FundamentalUnit);
        if (proto.Name is not null)
            res.ChangeNameAndSymbol(proto.Name, proto.Symbol);

        return res;
    }

    public BaseQuantity DefineBaseQuantity(
        string quantityName,
        string fundamentalUnitName,
        Prefix? unitPrefix = null,
        string? quantitySymbol = null,
        string? unitSymbol = null)
    {
        ThrowIfNameNotValid<Unit>(fundamentalUnitName);
        ThrowIfNameNotValid<Quantity>(quantityName);

        BaseQuantity quantity = new(this, quantityName, quantitySymbol);

        if (unitPrefix is null)
        {
            BaseUnit unit = new(this, fundamentalUnitName, quantity, 1m, unitSymbol);
            quantity.InnerFundamentalUnit = unit;
        }
        else
        {
            var fundamentalMultiplier = 1m / unitPrefix.Multiplier;
            BaseUnit unit = new(this, fundamentalUnitName, quantity, fundamentalMultiplier, unitSymbol);
            quantity.InnerFundamentalUnit = new PrefixedBaseUnit(this, unit, unitPrefix);
        }

        return quantity;
    }

    internal BaseQuantity DefineBaseQuantity(BaseQuantityProto proto)
    {
        var splitFundamentalName = proto.FundamentalUnit.Split('_');
        if (splitFundamentalName.Length == 1)
        {
            return DefineBaseQuantity(proto.Name!, splitFundamentalName[0], quantitySymbol: proto.Symbol);
        }
        else if (splitFundamentalName.Length == 2)
        {
            var prefix = GetFromName<Prefix>(splitFundamentalName[0]);
            return DefineBaseQuantity(proto.Name!, splitFundamentalName[1], prefix, quantitySymbol: proto.Symbol);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public IEnumerable<IDerivedUnit> GetAllIDerivedUnitsComposedOf(IBaseUnit baseUnit)
    {
        var allUnits = GetAllMaybeNameds<Unit>();

        IEnumerable<IDerivedUnit> unitsComposedOfGiven =
            from unit in allUnits
            where unit is DerivedUnit &&
            unit.UnitComposition.ContainsKey(baseUnit)
            select (DerivedUnit)unit;

        IEnumerable<IDerivedUnit> prefixedUnitsComposedOfGiven =
            from prefixedUnit in PrefixedUnits
            where prefixedUnit is PrefixedDerivedUnit &&
            prefixedUnit.UnitComposition.ContainsKey(baseUnit)
            select (PrefixedDerivedUnit)prefixedUnit;

        return unitsComposedOfGiven.Union(prefixedUnitsComposedOfGiven);
    }




    public Quantity GetFromBaseComposition(NamedComposition<IUnit> composition)
    {
        var resultingQuantComp = EmptyQuantity.BaseQuantityComposition;
        foreach (var (unit, power) in composition)
        {
            var quantComp = unit.Quantity.BaseQuantityComposition.Pow(power);
            resultingQuantComp *= quantComp;
        }

        return GetFromBaseComposition(resultingQuantComp);
    }

    public Quantity GetFromBaseComposition(NamedComposition<BaseQuantity> composition)
    {
        if (QuantitiesByComposition.TryGetValue(composition, out var res))
            return res;

        return new DerivedQuantity(this, composition);
    }

    public Unit DefineFromComposition(string name, NamedComposition<IUnit> composition)
    {
        var quantity = GetFromBaseComposition(composition);
        if (quantity is BaseQuantity)
            return new BaseUnit(this, name, composition);
        else
            return new DerivedUnit(this, name, composition);
    }
}

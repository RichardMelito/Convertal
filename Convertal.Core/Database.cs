// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using Convertal.Core.Extensions;

namespace Convertal.Core;

[JsonConverter(typeof(JsonConverters.DatabaseConverter))]
public class Database
{
    private readonly List<PrefixedUnit> _prefixedUnits = new();

    public IEnumerable<Prefix> Prefixes => GetAllMaybeNameds<Prefix>();
    public IEnumerable<ScalarBaseQuantity> ScalarBaseQuantitys => GetAllMaybeNameds<ScalarBaseQuantity>();
    public IEnumerable<VectorBaseQuantity> VectorBaseQuantitys => GetAllMaybeNameds<VectorBaseQuantity>();
    public IEnumerable<ScalarDerivedQuantity> ScalarDerivedQuantitys => GetAllMaybeNameds<ScalarDerivedQuantity>();
    public IEnumerable<VectorDerivedQuantity> VectorDerivedQuantitys => GetAllMaybeNameds<VectorDerivedQuantity>();
    public IEnumerable<ScalarBaseUnit> ScalarBaseUnits => GetAllMaybeNameds<ScalarBaseUnit>();
    public IEnumerable<VectorBaseUnit> VectorBaseUnits => GetAllMaybeNameds<VectorBaseUnit>();
    public IEnumerable<ScalarPrefixedBaseUnit> ScalarPrefixedBaseUnits => _prefixedUnits.Where(x => x is ScalarPrefixedBaseUnit).Cast<ScalarPrefixedBaseUnit>();
    public IEnumerable<VectorPrefixedBaseUnit> VectorPrefixedBaseUnits => _prefixedUnits.Where(x => x is VectorPrefixedBaseUnit).Cast<VectorPrefixedBaseUnit>();
    public IEnumerable<ScalarDerivedUnit> ScalarDerivedUnits => GetAllMaybeNameds<ScalarDerivedUnit>();
    public IEnumerable<VectorDerivedUnit> VectorDerivedUnits => GetAllMaybeNameds<VectorDerivedUnit>();
    public IEnumerable<ScalarPrefixedDerivedUnit> ScalarPrefixedDerivedUnits => _prefixedUnits.Where(x => x is ScalarPrefixedDerivedUnit).Cast<ScalarPrefixedDerivedUnit>();
    public IEnumerable<VectorPrefixedDerivedUnit> VectorPrefixedDerivedUnits => _prefixedUnits.Where(x => x is VectorPrefixedDerivedUnit).Cast<VectorPrefixedDerivedUnit>();
    public IEnumerable<MeasurementSystem> MeasurementSystems => GetAllMaybeNameds<MeasurementSystem>();

    internal Dictionary<Type, List<MaybeNamed>> MaybeNamedsByType { get; } = new();
    internal Dictionary<NamedComposition<IBaseQuantity>, Quantity> QuantitiesByComposition { get; } = new();

    public ScalarEmptyQuantity ScalarEmptyQuantity { get; }
    public VectorEmptyQuantity VectorEmptyQuantity { get; }

    public ScalarEmptyUnit ScalarEmptyUnit { get; }
    public VectorEmptyUnit VectorEmptyUnit { get; }

    public IReadOnlyDictionary<NamedComposition<IBaseQuantity>, Quantity> CompositionAndQuantitiesDictionary { get; }

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
        ScalarEmptyQuantity = new(this);
        VectorEmptyQuantity = new(this);
        ScalarEmptyUnit = new(this);
        VectorEmptyUnit = new(this);
        PrefixedUnits = _prefixedUnits.AsReadOnly();
    }

    internal void AddToPrefixedUnitsList(PrefixedUnit toAdd) => _prefixedUnits.Add(toAdd);
    internal bool RemoveFromPrefixedUnitsList(PrefixedUnit toRemove) => _prefixedUnits.Remove(toRemove);

    public Prefix DefinePrefix(string name, decimal multiplier, string? symbol = null) => new(this, name, multiplier, symbol);
    internal Prefix DefinePrefix(PrefixProto proto) => new(this, proto.Name!, proto.Multiplier, proto.Symbol);

    public ScalarPrefixedBaseUnit GetPrefixedUnit(ScalarBaseUnit unit, Prefix prefix)
    {
        return (ScalarPrefixedBaseUnit)GetPrefixedUnit((Unit)unit, prefix);
    }

    public ScalarPrefixedDerivedUnit GetPrefixedUnit(ScalarDerivedUnit unit, Prefix prefix)
    {
        return (ScalarPrefixedDerivedUnit)GetPrefixedUnit((Unit)unit, prefix);
    }

    public VectorPrefixedBaseUnit GetPrefixedUnit(VectorBaseUnit unit, Prefix prefix)
    {
        return (VectorPrefixedBaseUnit)GetPrefixedUnit((Unit)unit, prefix);
    }

    public VectorPrefixedDerivedUnit GetPrefixedUnit(VectorDerivedUnit unit, Prefix prefix)
    {
        return (VectorPrefixedDerivedUnit)GetPrefixedUnit((Unit)unit, prefix);
    }

    public PrefixedUnit GetPrefixedUnit(Unit unit, Prefix prefix)
    {
        var existingPrefixedUnits =
            (from prefixedUnit in PrefixedUnits
            where prefixedUnit.Unit == unit &&
            prefixedUnit.Prefix == prefix
            select prefixedUnit)
            .ToImmutableArray();

        if (existingPrefixedUnits.Length == 0)
        {
            return unit switch
            {
                ScalarBaseUnit u => new ScalarPrefixedBaseUnit(this, u, prefix),
                VectorBaseUnit u => new VectorPrefixedBaseUnit(this, u, prefix),
                ScalarDerivedUnit u => new ScalarPrefixedDerivedUnit(this, u, prefix),
                VectorDerivedUnit u => new VectorPrefixedDerivedUnit(this, u, prefix),
                _ => throw new NotImplementedException(),
            };
        }

        return existingPrefixedUnits.Single();
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

            // TODO why is this commented out?
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

    public MaybeNamed GetFromName(string name, Type type, bool isSymbol = false)
    {
        if (TryGetFromName(name, type, out var named, isSymbol))
            return named;

        throw new InvalidOperationException($"No instances of " +
            $"{type.Name} with {(isSymbol ? "symbol" : "name")} {name}.");
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

    public ScalarBaseUnit DefineScalarBaseUnit(
        string name,
        IScalarBaseUnit otherUnit,
        decimal multiplier,
        decimal offset = 0,
        string? symbol = null) => new(this, name, otherUnit, multiplier, offset, symbol);

    public VectorBaseUnit DefineVectorBaseUnit(
        string name,
        IVectorBaseUnit otherUnit,
        decimal multiplier,
        string? symbol = null) => new(this, name, otherUnit, multiplier, symbol);

    internal ScalarBaseUnit DefineScalarBaseUnit(UnitProto proto)
    {
        ScalarComposition<IUnit>? composition = null;
        if (proto.OtherUnitComposition is not null)
        {
            composition = new(proto.OtherUnitComposition.ToDictionary(
                kvp => ParseIUnit(kvp.Key),
                kvp => kvp.Value));
        }

        // True if this is a fundamental unit
        if (proto.Name is not null && TryGetFromName<ScalarBaseUnit>(proto.Name, out var unit))
        {
            if (proto.Symbol is not null)
                unit.ChangeSymbol(proto.Symbol);

            if (composition is not null)
                unit.SetUnitComposition(composition);

            return unit;
        }

        return new ScalarBaseUnit(
            this,
            proto.Name!,
            proto.Symbol,
            GetFromName<ScalarBaseQuantity>(proto.Quantity),
            proto.FundamentalMultiplier,
            proto.FundamentalOffset,
            composition);
    }

    internal VectorBaseUnit DefineVectorBaseUnit(UnitProto proto)
    {
        VectorComposition<IUnit>? composition = null;
        if (proto.OtherUnitComposition is not null)
        {
            composition = new(proto.OtherUnitComposition.ToDictionary(
                kvp => ParseIUnit(kvp.Key),
                kvp => kvp.Value));
        }

        // True if this is a fundamental unit
        if (proto.Name is not null && TryGetFromName<VectorBaseUnit>(proto.Name, out var unit))
        {
            if (proto.Symbol is not null)
                unit.ChangeSymbol(proto.Symbol);

            if (composition is not null)
                unit.SetUnitComposition(composition);

            return unit;
        }

        return new VectorBaseUnit(
            this,
            proto.Name!,
            proto.Symbol,
            GetFromName<VectorBaseQuantity>(proto.Quantity),
            proto.FundamentalMultiplier,
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

    public ScalarDerivedUnit DefineScalarDerivedUnit(
        string name,
        IScalarDerivedUnit otherUnit,
        decimal multiplier,
        decimal offset = 0,
        string? symbol = null)
        => new(this, name, otherUnit, multiplier, offset, symbol);

    public VectorDerivedUnit DefineVectorDerivedUnit(
        string name,
        IVectorDerivedUnit otherUnit,
        decimal multiplier,
        string? symbol = null)
        => new(this, name, otherUnit, multiplier, symbol);

    internal ScalarDerivedUnit DefineScalarDerivedUnit(UnitProto proto)
    {
        ScalarComposition<IUnit>? composition = null;
        if (proto.OtherUnitComposition is not null)
        {
            composition = new(proto.OtherUnitComposition.ToDictionary(
                kvp => ParseIUnit(kvp.Key),
                kvp => kvp.Value));
        }

        // True if this is a fundamental unit
        if (proto.Name is not null && TryGetFromName<ScalarDerivedUnit>(proto.Name, out var unit))
        {
            if (proto.Symbol is not null)
                unit.ChangeSymbol(proto.Symbol);

            if (composition is not null && unit.UnitComposition is null)
                unit.SetUnitComposition(composition);

            return unit;
        }

        return new ScalarDerivedUnit(
            this,
            proto.Name!,
            proto.Symbol,
            GetFromName<ScalarDerivedQuantity>(proto.Quantity),
            proto.FundamentalMultiplier,
            proto.FundamentalOffset,
            composition);
    }

    internal VectorDerivedUnit DefineVectorDerivedUnit(UnitProto proto)
    {
        VectorComposition<IUnit>? composition = null;
        if (proto.OtherUnitComposition is not null)
        {
            composition = new(proto.OtherUnitComposition.ToDictionary(
                kvp => ParseIUnit(kvp.Key),
                kvp => kvp.Value));
        }

        // True if this is a fundamental unit
        if (proto.Name is not null && TryGetFromName<VectorDerivedUnit>(proto.Name, out var unit))
        {
            if (proto.Symbol is not null)
                unit.ChangeSymbol(proto.Symbol);

            if (composition is not null && unit.UnitComposition is null)
                unit.SetUnitComposition(composition);

            return unit;
        }

        return new VectorDerivedUnit(
            this,
            proto.Name!,
            proto.Symbol,
            GetFromName<VectorDerivedQuantity>(proto.Quantity),
            proto.FundamentalMultiplier,
            composition);
    }

    // TODO why did I make these?
    public ScalarDerivedQuantity DefineScalarDerivedQuantity(
        Func<Quantity> quantityOperation,
        string quantityName,
        string? quantitySymbol = null)
    {
        var resultingQuantity = quantityOperation();
        var res = resultingQuantity as ScalarDerivedQuantity;
        if (res is null)
        {
            resultingQuantity.Dispose();

            // TODO elaborate and maybe do some magic to write out the operation inputs.
            throw new InvalidOperationException("The given quantity operation did not return a derived quantity.");
        }

        res.ChangeNameAndSymbol(quantityName, quantitySymbol);
        return res;
    }

    public VectorDerivedQuantity DefineVectorDerivedQuantity(
        Func<Quantity> quantityOperation,
        string quantityName,
        string? quantitySymbol = null)
    {
        var resultingQuantity = quantityOperation();
        var res = resultingQuantity as VectorDerivedQuantity;
        if (res is null)
        {
            resultingQuantity.Dispose();

            // TODO elaborate and maybe do some magic to write out the operation inputs.
            throw new InvalidOperationException("The given quantity operation did not return a derived quantity.");
        }

        res.ChangeNameAndSymbol(quantityName, quantitySymbol);
        return res;
    }

    internal ScalarDerivedQuantity DefineScalarDerivedQuantity(DerivedQuantityProto proto)
    {
        ScalarComposition<IBaseQuantity> composition = new(proto.BaseQuantityComposition
            .ToDictionary(kvp => (IBaseQuantity)GetFromName<Quantity>(kvp.Key), kvp => kvp.Value));

        ScalarDerivedQuantity res = new(this, composition, proto.FundamentalUnit);
        if (proto.Name is not null)
            res.ChangeNameAndSymbol(proto.Name, proto.Symbol);

        return res;
    }

    internal VectorDerivedQuantity DefineVectorDerivedQuantity(DerivedQuantityProto proto)
    {
        VectorComposition<IBaseQuantity> composition = new(proto.BaseQuantityComposition
            .ToDictionary(kvp => (IBaseQuantity)GetFromName<Quantity>(kvp.Key), kvp => kvp.Value));

        VectorDerivedQuantity res = new(this, composition, proto.FundamentalUnit);
        if (proto.Name is not null)
            res.ChangeNameAndSymbol(proto.Name, proto.Symbol);

        return res;
    }

    public ScalarBaseQuantity DefineScalarBaseQuantity(
        string quantityName,
        string fundamentalUnitName,
        Prefix? unitPrefix = null,
        string? quantitySymbol = null,
        string? unitSymbol = null)
    {
        ThrowIfNameNotValid<Unit>(fundamentalUnitName);
        ThrowIfNameNotValid<Quantity>(quantityName);

        ScalarBaseQuantity quantity = new(this, quantityName, quantitySymbol);

        if (unitPrefix is null)
        {
            ScalarBaseUnit unit = new(this, fundamentalUnitName, quantity, 1m, unitSymbol);
            quantity.SettableFundamentalUnit = unit;
        }
        else
        {
            var fundamentalMultiplier = 1m / unitPrefix.Multiplier;
            ScalarBaseUnit unit = new(this, fundamentalUnitName, quantity, fundamentalMultiplier, unitSymbol);
            quantity.SettableFundamentalUnit = new ScalarPrefixedBaseUnit(this, unit, unitPrefix);
        }

        return quantity;
    }

    public VectorBaseQuantity DefineVectorBaseQuantity(
        string quantityName,
        string fundamentalUnitName,
        Prefix? unitPrefix = null,
        string? quantitySymbol = null,
        string? unitSymbol = null)
    {
        ThrowIfNameNotValid<Unit>(fundamentalUnitName);
        ThrowIfNameNotValid<Quantity>(quantityName);

        VectorBaseQuantity quantity = new(this, quantityName, quantitySymbol);

        if (unitPrefix is null)
        {
            VectorBaseUnit unit = new(this, fundamentalUnitName, quantity, 1m, unitSymbol);
            quantity.SettableFundamentalUnit = unit;
        }
        else
        {
            var fundamentalMultiplier = 1m / unitPrefix.Multiplier;
            VectorBaseUnit unit = new(this, fundamentalUnitName, quantity, fundamentalMultiplier, unitSymbol);
            quantity.SettableFundamentalUnit = new VectorPrefixedBaseUnit(this, unit, unitPrefix);
        }

        return quantity;
    }

    internal ScalarBaseQuantity DefineScalarBaseQuantity(BaseQuantityProto proto)
    {
        var splitFundamentalName = proto.FundamentalUnit.Split('_');
        if (splitFundamentalName.Length == 1)
        {
            return DefineScalarBaseQuantity(proto.Name!, splitFundamentalName[0], quantitySymbol: proto.Symbol);
        }
        else if (splitFundamentalName.Length == 2)
        {
            var prefix = GetFromName<Prefix>(splitFundamentalName[0]);
            return DefineScalarBaseQuantity(proto.Name!, splitFundamentalName[1], prefix, quantitySymbol: proto.Symbol);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    internal VectorBaseQuantity DefineVectorBaseQuantity(BaseQuantityProto proto)
    {
        var splitFundamentalName = proto.FundamentalUnit.Split('_');
        if (splitFundamentalName.Length == 1)
        {
            return DefineVectorBaseQuantity(proto.Name!, splitFundamentalName[0], quantitySymbol: proto.Symbol);
        }
        else if (splitFundamentalName.Length == 2)
        {
            var prefix = GetFromName<Prefix>(splitFundamentalName[0]);
            return DefineVectorBaseQuantity(proto.Name!, splitFundamentalName[1], prefix, quantitySymbol: proto.Symbol);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public IEnumerable<IDerivedUnit> GetAllIDerivedUnitsComposedOf(IBaseUnit baseUnit)
    {
        IEnumerable<IDerivedUnit> unitsComposedOfGiven =
            from unit in GetAllMaybeNameds<Unit>()
            where unit is IDerivedUnit &&
            unit.UnitComposition.ContainsKey(baseUnit)
            select (IDerivedUnit)unit;

        IEnumerable<IDerivedUnit> prefixedUnitsComposedOfGiven =
            from prefixedUnit in PrefixedUnits
            where prefixedUnit is IDerivedUnit &&
            prefixedUnit.UnitComposition.ContainsKey(baseUnit)
            select (IDerivedUnit)prefixedUnit;

        return unitsComposedOfGiven.Union(prefixedUnitsComposedOfGiven);
    }


    //public IScalarUnit GetUnitFromBaseComposition(ScalarComposition<IUnit> composition)
    //{
    //    // TODO need a UnitsByComposition dictionary
    //}

    //public IVectorUnit GetUnitFromBaseComposition(VectorComposition<IUnit> composition)
    //{
    //    // TODO need a UnitsByComposition dictionary
    ////}

    //public ScalarQuantity GetQuantityFromBaseComposition(ScalarComposition<IUnit> composition)
    //{
    //    var resultingQuantComp = ScalarEmptyQuantity.BaseQuantityComposition;
    //    foreach (var (unit, power) in composition)
    //    {
    //        NamedComposition<IBaseQuantity> quantComp;
    //        if (unit is IScalarUnit s)
    //        {
    //            quantComp = s.Quantity.BaseQuantityComposition.Pow(power);
    //        }
    //        else if (unit is IVectorUnit v)
    //        {
    //            if (power != 1)
    //                throw new InvalidOperationException();

    //            quantComp = v.Quantity.BaseQuantityComposition;
    //        }
    //        else
    //        {
    //            throw new NotImplementedException();
    //        }

    //        resultingQuantComp = (resultingQuantComp * quantComp);
    //    }

    //    return GetQuantityFromBaseComposition(resultingQuantComp);
    //}

    //public VectorQuantity GetQuantityFromBaseComposition(VectorComposition<IUnit> composition)
    //{
    //    var resultingQuantComp = VectorEmptyQuantity.BaseQuantityComposition;
    //    foreach (var (unit, power) in composition)
    //    {
    //        var quantComp = unit.Quantity.BaseQuantityComposition.Pow(power);
    //        resultingQuantComp *= quantComp;
    //    }

    //    return GetQuantityFromBaseComposition(resultingQuantComp);
    //}

    public Quantity GetQuantityFromBaseComposition(NamedComposition<IUnit> composition)
    {
        var resultingQuantComp = (NamedComposition<IBaseQuantity>)ScalarEmptyQuantity.BaseQuantityComposition;
        foreach (var (unit, power) in composition)
        {
            var quantComp = unit.Quantity.BaseQuantityComposition;
            if (power != 1m)
                quantComp = ((ScalarComposition<IBaseQuantity>)quantComp).Pow(power);

            resultingQuantComp *= quantComp;
        }

        return GetQuantityFromBaseComposition(resultingQuantComp);
    }

    public ScalarQuantity GetScalarQuantityFromBaseComposition(ScalarComposition<IBaseQuantity> composition) => (ScalarQuantity)GetQuantityFromBaseComposition(composition);

    public VectorQuantity GetVectorQuantityFromBaseComposition(VectorComposition<IBaseQuantity> composition) => (VectorQuantity)GetQuantityFromBaseComposition(composition);

    public Quantity GetQuantityFromBaseComposition(NamedComposition<IBaseQuantity> composition)
    {
        if (QuantitiesByComposition.TryGetValue(composition, out var res))
            return res;

        if (composition is ScalarComposition<IBaseQuantity> s)
            return new ScalarDerivedQuantity(this, s);

        return new VectorDerivedQuantity(this, (VectorComposition<IBaseQuantity>)composition);
    }

    //public Unit DefineFromComposition(string name, NamedComposition<IUnit> composition)
    //{
    //    var quantity = GetQuantityFromBaseComposition(composition);
    //    if (quantity is BaseQuantity)
    //        return new BaseUnit(this, name, composition);
    //    else
    //        return new DerivedUnit(this, name, composition);
    //}
}

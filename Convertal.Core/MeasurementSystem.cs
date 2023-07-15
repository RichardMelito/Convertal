// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public record MeasurementSystemProto(
    string Name,
    [property: JsonPropertyOrder(2)] ValueEqualityDictionary<string, string> QuantityToUnitDictionary)
    : MaybeNamedProto(Name, null);

public class MeasurementSystem : MaybeNamed, INamed
{
    // TODO
    public static MeasurementSystem? Current { get; set; } = null;

    private readonly Dictionary<Quantity, IUnit> _quantities_units = new();

    public IReadOnlyDictionary<Quantity, IUnit> QuantityToUnitDictionary { get; }

    internal MeasurementSystem(Database database, string name)
        : base(database, name)
    {
        QuantityToUnitDictionary = _quantities_units.AsReadOnly();
    }

    public override MeasurementSystemProto ToProto()
    {
        return new(Name!, new(QuantityToUnitDictionary
            .ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value.ToString()!)));
    }

    public IScalarUnit? GetScalarUnit(ScalarQuantity quantity) => (IScalarUnit?)GetUnit(quantity);
    public IVectorUnit? GetVectorUnit(VectorQuantity quantity) => (IVectorUnit?)GetUnit(quantity);

    public IUnit? GetUnit(Quantity quantity)
    {
        if (_quantities_units.TryGetValue(quantity, out var res))
            return res;

        return null;
    }

    public void SetQuantityUnitPair(Quantity quantity, IUnit unit)
    {
        // TODO improve this so that users can have separate preferences for vectors and scalars
        var scalarUnit = (IScalarUnit)unit.ToScalar();
        var scalarQuantity = quantity.ToScalar();

        if (scalarUnit.Quantity != scalarQuantity)
        {
            throw new InvalidOperationException($"IUnit {scalarUnit} has quantity " +
                $"{scalarUnit.Quantity} instead of {scalarQuantity}.");
        }
        _quantities_units[scalarQuantity] = scalarUnit;

        var vectorUnit = unit.ToVector() as IVectorUnit;
        var vectorQuantity = quantity.ToVector();
        if (vectorUnit is not null && vectorQuantity is not null)
        {
            if (vectorUnit.Quantity != vectorQuantity)
            {
                throw new InvalidOperationException($"IUnit {vectorUnit} has quantity " +
                    $"{vectorUnit.Quantity} instead of {vectorQuantity}.");
            }
            _quantities_units[vectorQuantity] = vectorUnit;
        }
    }

    public void SetQuantityUnitPairs(params KeyValuePair<Quantity, IUnit>[] pairs)
    {
        foreach (var pair in pairs)
            SetQuantityUnitPair(pair.Key, pair.Value);
    }

    public bool RemoveUnit(IUnit unit)
    {
        if (_quantities_units.ContainsValue(unit))
            return _quantities_units.Remove(unit.Quantity);

        return false;
    }

    public bool RemoveQuantity(Quantity quantity)
    {
        return _quantities_units.Remove(quantity);
    }

    public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        // nothing should depend on a MeasurementSystem
        toIgnore = toIgnore.UnionAppend(this);
        return Array.Empty<IMaybeNamed>().SortByTypeAndName();
    }

    protected override void DisposeBody(bool disposeDependents)
    {
        // nothing should depend on a MeasurementSystem

        if (Current == this)
            Current = null;

        base.DisposeBody(disposeDependents);
    }
}

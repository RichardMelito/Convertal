// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class ScalarBaseUnit : ScalarUnit, IBaseUnit
{
    // TODO interface or concretion?
    internal IVectorBaseUnit? SettableVectorAnalog { get; set; }
    public override IVectorBaseUnit? VectorAnalog => SettableVectorAnalog;

    // for defining from a chain of operations
    internal ScalarBaseUnit(
        Database database,
        string name,
        ScalarComposition<IUnit> composition)
        : base(database, name, composition)
    {
    }

    internal ScalarBaseUnit(
        Database database,
        string? name,
        ScalarQuantity quantity,
        decimal fundamentalMultiplier,
        ScalarComposition<IUnit>? composition = null,
        string? symbol = null)
        : base(database, name, quantity, fundamentalMultiplier, composition, symbol)
    {
    }

    internal ScalarBaseUnit(
        Database database,
        string? name,
        IScalarUnit otherUnit,
        decimal multiplier,
        decimal offset,
        string? symbol)
        : base(database, name, otherUnit, multiplier, offset, symbol)
    {
    }

    internal ScalarBaseUnit(
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


    public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        var res = base.GetAllDependents(ref toIgnore).AsEnumerable();

        var unitsComposedOfThis = Database.GetAllIDerivedUnitsComposedOf(this).Cast<IMaybeNamed>();
        res = res.Union(unitsComposedOfThis);

        foreach (var unit in unitsComposedOfThis.Except(toIgnore))
            res = res.Union(unit.GetAllDependents(ref toIgnore));

        res.ThrowIfSetContains(this);
        return res.SortByTypeAndName();
    }
}

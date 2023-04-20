// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class ScalarBaseUnit : ScalarUnit, IScalarBaseUnit
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

    // TODO fix method reference
    /// <summary>
    /// Only to be called from <see cref="BaseQuantity.DefineNewBaseQuantity(string, string, Prefix?)"/>
    /// </summary>
    internal ScalarBaseUnit(
        Database database,
        string name,
        ScalarBaseQuantity quantity,
        decimal fundamentalMultiplier,
        string? symbol)
        : base(database, name, quantity, fundamentalMultiplier, symbol: symbol)
    {
    }

    // for defining from an existing IScalarBaseUnit
    internal ScalarBaseUnit(
        Database database,
        string name,
        IScalarBaseUnit otherUnit,
        decimal multiplier,
        decimal offset = 0,
        string? symbol = null)
        : base(database, name, otherUnit, multiplier, offset, symbol)
    {
    }

    // TODO fix method reference
    /// <summary>
    /// To be called only from <see cref="Database.DefineBaseUnit(UnitProto)"/>
    /// </summary>
    internal ScalarBaseUnit(
        Database database,
        string name,
        string? symbol,
        ScalarBaseQuantity quantity,
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

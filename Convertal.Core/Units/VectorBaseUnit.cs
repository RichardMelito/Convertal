// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class VectorBaseUnit : VectorUnit, IBaseUnit
{
    // TODO interface or concretion?
    // TODO default
    internal IScalarBaseUnit SettableScalarAnalog { get; set; } = null!;
    public override IScalarBaseUnit ScalarAnalog => SettableScalarAnalog;
    
    // for defining from a chain of operations
    internal VectorBaseUnit(
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
    internal VectorBaseUnit(
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
    internal VectorBaseUnit(
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
    internal VectorBaseUnit(
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

    // TODO copy-paste of ScalarBaseUnit's implementation
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

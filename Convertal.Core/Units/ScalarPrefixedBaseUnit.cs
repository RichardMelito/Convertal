// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class ScalarPrefixedBaseUnit : ScalarPrefixedUnit, IScalarBaseUnit
{
    public override ScalarBaseUnit Unit => (ScalarBaseUnit)base.Unit;

    internal VectorPrefixedBaseUnit? SettableVectorAnalog { get; set; }
    public override VectorPrefixedBaseUnit? VectorAnalog => SettableVectorAnalog;

    internal ScalarPrefixedBaseUnit(Database database, ScalarBaseUnit unit, Prefix prefix)
        : base(database, unit, prefix)
    {
    }

    public IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        var res = IUnit.GetAllDependents(this, ref toIgnore).AsEnumerable();

        var unitsComposedOfThis = Database.GetAllIDerivedUnitsComposedOf(this);
        res = res.Union(unitsComposedOfThis);
        foreach (var unit in unitsComposedOfThis.Except(toIgnore))
            res = res.Union(unit.GetAllDependents(ref toIgnore));

        res.ThrowIfSetContains(this);
        return res.SortByTypeAndName();
    }
}

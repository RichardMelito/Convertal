// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class VectorPrefixedBaseUnit : VectorPrefixedUnit, IVectorBaseUnit
{
    public override VectorBaseUnit Unit => (VectorBaseUnit)base.Unit;
    public override ScalarPrefixedBaseUnit ScalarAnalog => (ScalarPrefixedBaseUnit)base.ScalarAnalog;
    public override VectorBaseQuantity Quantity => (VectorBaseQuantity)base.Quantity;

    public VectorPrefixedBaseUnit(VectorBaseUnit unit, Prefix prefix) : base(unit, prefix)
    {

    }


    // TODO copy-paste of ScalarPrefixedBaseUnit implementation
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

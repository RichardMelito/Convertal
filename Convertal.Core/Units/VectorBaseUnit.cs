// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Collections.Generic;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

public class VectorBaseUnit : VectorUnit, IVectorBaseUnit
{
    public override VectorBaseQuantity Quantity => (VectorBaseQuantity)base.Quantity;

    public override ScalarBaseUnit ScalarAnalog => (ScalarBaseUnit)base.ScalarAnalog;

    // for defining from a chain of operations
    internal VectorBaseUnit(ScalarBaseUnit scalarAnalog)
        : base(scalarAnalog)
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

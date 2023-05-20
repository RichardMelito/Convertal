// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class VectorPrefixedDerivedUnit : VectorPrefixedUnit, IVectorDerivedUnit
{
    public override VectorDerivedUnit Unit => (VectorDerivedUnit)base.Unit;
    public override ScalarPrefixedDerivedUnit ScalarAnalog => (ScalarPrefixedDerivedUnit)base.ScalarAnalog;
    public override VectorDerivedQuantity Quantity => (VectorDerivedQuantity)base.Quantity;

    internal VectorPrefixedDerivedUnit(VectorDerivedUnit unit, Prefix prefix)
        : base(unit, prefix)
    {
    }
}

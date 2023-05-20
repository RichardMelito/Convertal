// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class ScalarPrefixedDerivedUnit : ScalarPrefixedUnit, IScalarDerivedUnit
{
    public override ScalarDerivedUnit Unit => (ScalarDerivedUnit)base.Unit;
    public override VectorPrefixedDerivedUnit? VectorAnalog => (VectorPrefixedDerivedUnit?)base.VectorAnalog;
    public override ScalarDerivedQuantity Quantity => (ScalarDerivedQuantity)base.Quantity;

    internal ScalarPrefixedDerivedUnit(ScalarDerivedUnit unit, Prefix prefix)
        : base(unit, prefix)
    {
    }
}

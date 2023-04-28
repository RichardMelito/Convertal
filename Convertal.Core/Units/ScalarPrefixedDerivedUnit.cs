// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class ScalarPrefixedDerivedUnit : ScalarPrefixedUnit, IScalarDerivedUnit
{
    public override ScalarDerivedUnit Unit => (ScalarDerivedUnit)base.Unit;

    internal VectorPrefixedDerivedUnit? SettableVectorAnalog { get; set; }
    public override VectorPrefixedDerivedUnit? VectorAnalog => SettableVectorAnalog;
    public override ScalarDerivedQuantity Quantity => (ScalarDerivedQuantity)base.Quantity;

    internal ScalarPrefixedDerivedUnit(Database database, ScalarDerivedUnit unit, Prefix prefix)
        : base(database, unit, prefix)
    {
    }
}

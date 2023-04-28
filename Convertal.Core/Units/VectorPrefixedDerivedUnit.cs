// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class VectorPrefixedDerivedUnit : VectorPrefixedUnit, IVectorDerivedUnit
{
    public override VectorDerivedUnit Unit => (VectorDerivedUnit)base.Unit;

    internal ScalarPrefixedDerivedUnit? SettableScalarAnalog { get; set; }
    public override ScalarPrefixedDerivedUnit ScalarAnalog => SettableScalarAnalog!;
    public override VectorDerivedQuantity Quantity => (VectorDerivedQuantity)base.Quantity;

    internal VectorPrefixedDerivedUnit(Database database, VectorDerivedUnit unit, Prefix prefix)
        : base(database, unit, prefix)
    {
    }
}

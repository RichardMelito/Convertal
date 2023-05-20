// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

public abstract class VectorPrefixedUnit : PrefixedUnit, IVectorUnit
{
    private ScalarPrefixedUnit? _scalarAnalog;

    public override bool IsVector => true;
    public override VectorUnit Unit => (VectorUnit)base.Unit;
    public override VectorComposition<IUnit> UnitComposition => (VectorComposition<IUnit>)base.UnitComposition;
    public override VectorQuantity Quantity => (VectorQuantity)base.Quantity;

    public virtual ScalarPrefixedUnit ScalarAnalog => _scalarAnalog ??= Prefix[Unit.ScalarAnalog];
    IScalarUnit IVector<IVectorUnit, IScalarUnit>.ScalarAnalog => ScalarAnalog;

    protected VectorPrefixedUnit(VectorUnit unit, Prefix prefix)
        : base(unit, prefix)
    {
    }

    // TODO
    public IScalarUnit DotP(IVectorUnit other) => throw new NotImplementedException();
    public IVectorUnit CrossP(IVectorUnit other) => throw new NotImplementedException();
}

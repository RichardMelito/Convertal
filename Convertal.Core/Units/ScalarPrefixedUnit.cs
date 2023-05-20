// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

public abstract class ScalarPrefixedUnit : PrefixedUnit, IScalarUnit
{
    private VectorPrefixedUnit? _vectorAnalog;

    public decimal FundamentalOffset => Unit.FundamentalOffset / Prefix.Multiplier;
    public override bool IsVector => false;
    public override ScalarUnit Unit => (ScalarUnit)base.Unit;
    public override ScalarComposition<IUnit> UnitComposition => (ScalarComposition<IUnit>)base.UnitComposition;
    public override ScalarQuantity Quantity => (ScalarQuantity)base.Quantity;

    public virtual VectorPrefixedUnit? VectorAnalog
        => _vectorAnalog ??= (Unit.VectorAnalog is null ? null : Prefix[Unit.VectorAnalog]);

    IVectorUnit? IScalar<IScalarUnit, IVectorUnit>.VectorAnalog => VectorAnalog;

    protected ScalarPrefixedUnit(ScalarUnit unit, Prefix prefix)
        : base(unit, prefix)
    {
    }

    //public override PrefixedUnitProto ToProto()
    //{
    //    return new(Name!, Symbol, Quantity.ToString(), FundamentalMultiplier, FundamentalOffset);
    //}
}

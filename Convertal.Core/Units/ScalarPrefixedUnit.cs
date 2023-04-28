// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

public abstract class ScalarPrefixedUnit : PrefixedUnit, IScalarUnit
{
    public decimal FundamentalOffset => Unit.FundamentalOffset / Prefix.Multiplier;
    public override bool IsVector => false;
    public override ScalarUnit Unit => (ScalarUnit)base.Unit;
    public override ScalarComposition<IUnit> UnitComposition => (ScalarComposition<IUnit>)base.UnitComposition;
    public override ScalarQuantity Quantity => (ScalarQuantity)base.Quantity;

    public abstract VectorPrefixedUnit? VectorAnalog { get; }
    IVectorUnit? IScalar<IScalarUnit, IVectorUnit>.VectorAnalog => VectorAnalog;

    protected ScalarPrefixedUnit(Database database, ScalarUnit unit, Prefix prefix)
        : base(database, unit, prefix)
    {
    }

    //// TODO
    //public IScalarUnit Pow(decimal power) => throw new NotImplementedException();

    public override PrefixedUnitProto ToProto()
    {
        return new(Name!, Symbol, Quantity.ToString(), FundamentalMultiplier, FundamentalOffset);
    }
}

// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Text;
using System.Threading.Tasks;

namespace Convertal.Core;

public abstract class VectorUnit : Unit, IVectorUnit
{
    public override bool IsVector => true;
    public override VectorQuantity Quantity => (VectorQuantity)base.Quantity;
    public override VectorComposition<IUnit> UnitComposition => (VectorComposition<IUnit>)base.UnitComposition;

    public virtual ScalarUnit ScalarAnalog { get; }

    IScalarUnit IVector<IVectorUnit, IScalarUnit>.ScalarAnalog => ScalarAnalog;

    internal VectorUnit(ScalarUnit scalarAnalog)
        : base(
            scalarAnalog.Database,
            scalarAnalog.Name,
            scalarAnalog.Quantity.VectorAnalog!,
            scalarAnalog.FundamentalMultiplier,
            scalarAnalog.UnitComposition.ToSimpleVectorComposition(),
            scalarAnalog.Symbol)
    {
        ScalarAnalog = scalarAnalog;
        //ScalarAnalog.SetVectorAnalog(this);
    }

    public override ScalarUnitProto ToProto() => throw new InvalidOperationException();

    protected override Type GetDatabaseType() => typeof(VectorUnit);
}

// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class VectorDerivedUnit : VectorUnit, IVectorDerivedUnit
{
    public override VectorDerivedQuantity Quantity => (VectorDerivedQuantity)base.Quantity;

    public override ScalarDerivedUnit ScalarAnalog => (ScalarDerivedUnit)base.ScalarAnalog;


    internal VectorDerivedUnit(ScalarDerivedUnit scalarAnalog)
        : base(scalarAnalog)
    {
    }

    //private static VectorComposition<IUnit> GetComposition(VectorDerivedQuantity quant)
    //{
    //    return VectorComposition<IUnit>.
    //        CreateFromExistingBaseComposition(
    //        quant.BaseQuantityComposition,
    //        baseQuantity => baseQuantity.FundamentalUnit);
    //}
}

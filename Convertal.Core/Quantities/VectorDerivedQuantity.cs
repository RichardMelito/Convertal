// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class VectorDerivedQuantity : VectorQuantity, IDerivedQuantity
{
    public override IVectorDerivedUnit FundamentalUnit => (IVectorDerivedUnit)base.FundamentalUnit;

    public override ScalarDerivedQuantity ScalarAnalog => (ScalarDerivedQuantity)base.ScalarAnalog;

    internal VectorDerivedQuantity(
        ScalarDerivedQuantity scalarAnalog,
        VectorComposition<IBaseQuantity> composition)
        : base(scalarAnalog, composition, null, null)
    {
        Init();
    }

    public override VectorDerivedQuantityProto ToProto()
    {
        return new(
            Name,
            Symbol,
            ScalarAnalog.Name,
            new(BaseQuantityComposition.CompositionAsStringDictionary));
    }
}

// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class VectorDerivedQuantity : VectorQuantity, IDerivedQuantity
{
    public override IVectorDerivedUnit FundamentalUnit { get; }

    public override ScalarDerivedQuantity ScalarAnalog => (ScalarDerivedQuantity)base.ScalarAnalog;

    /// <summary>
    /// To be called only from <see cref="Quantity.GetFromBaseComposition(NamedComposition{BaseQuantity})"/>
    /// </summary>
    internal VectorDerivedQuantity(
        ScalarDerivedQuantity scalarAnalog,
        VectorComposition<IBaseQuantity> composition,
        string? fundamentalUnitName = null)
        : base(scalarAnalog, composition, null, null)
    {
        FundamentalUnit = new VectorDerivedUnit(Database, this, fundamentalUnitName);
        Init();
    }

    public override VectorDerivedQuantityProto ToProto()
    {
        return new(
            Name,
            Symbol,
            FundamentalUnit.Name,
            ScalarAnalog.Name,
            new(BaseQuantityComposition.CompositionAsStringDictionary));
    }
}

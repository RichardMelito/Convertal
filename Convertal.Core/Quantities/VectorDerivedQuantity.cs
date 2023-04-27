// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class VectorDerivedQuantity : VectorQuantity, IDerivedQuantity
{
    public override IVectorDerivedUnit FundamentalUnit { get; }

    // TODO need default generation
    internal ScalarDerivedQuantity? SettableScalarAnalog { get; set; }
    public override ScalarDerivedQuantity? ScalarAnalog => SettableScalarAnalog;


    /// <summary>
    /// To be called only from <see cref="Quantity.GetFromBaseComposition(NamedComposition{BaseQuantity})"/>
    /// </summary>
    internal VectorDerivedQuantity(
        Database database,
        VectorComposition<IBaseQuantity> composition,
        string? fundamentalUnitName = null)
        : base(database, null, null)
    {
        SettableBaseQuantityComposition = composition;
        FundamentalUnit = new VectorDerivedUnit(database, this, fundamentalUnitName);
        Init();
    }

    // TODO
    public override DerivedQuantityProto ToProto()
    {
        return new(
            Name,
            Symbol,
            FundamentalUnit.Name,
            new(BaseQuantityComposition.CompositionAsStringDictionary));
    }
}

// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class ScalarDerivedQuantity : ScalarQuantity, IDerivedQuantity
{
    public override IScalarDerivedUnit FundamentalUnit { get; }

    internal VectorDerivedQuantity? SettableVectorAnalog { get; set; }
    public override VectorDerivedQuantity? VectorAnalog => SettableVectorAnalog;

    /// <summary>
    /// To be called only from <see cref="Quantity.GetFromBaseComposition(NamedComposition{BaseQuantity})"/>
    /// </summary>
    internal ScalarDerivedQuantity(
        Database database,
        ScalarComposition<IBaseQuantity> composition,
        string? fundamentalUnitName = null)
        : base(database, null, null)
    {
        SettableBaseQuantityComposition = composition;
        FundamentalUnit = new ScalarDerivedUnit(database, this, fundamentalUnitName);
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

// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class ScalarDerivedQuantity : ScalarQuantity, IDerivedQuantity
{
    public override IScalarDerivedUnit FundamentalUnit { get; }

    public override VectorDerivedQuantity? VectorAnalog => (VectorDerivedQuantity?)base.VectorAnalog;

    /// <summary>
    /// To be called only from <see cref="Quantity.GetFromBaseComposition(NamedComposition{BaseQuantity})"/>
    /// </summary>
    internal ScalarDerivedQuantity(
        Database database,
        ScalarComposition<IBaseQuantity> composition,
        string? fundamentalUnitName = null)
        : base(database, composition, null, null)
    {
        FundamentalUnit = new ScalarDerivedUnit(database, this, fundamentalUnitName);
        Init();
    }

    public override ScalarDerivedQuantityProto ToProto()
    {
        return new(
            Name,
            Symbol,
            FundamentalUnit.Name,
            VectorAnalog?.Name,
            new(BaseQuantityComposition.CompositionAsStringDictionary));
    }
}

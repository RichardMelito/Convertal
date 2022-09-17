// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Text.Json.Serialization;

namespace ConvertAllTheThings.Core;

public record DerivedQuantityProto(
    string? Name,
    string? Symbol,
    [property: JsonPropertyOrder(2)] string? FundamentalUnit,
    [property: JsonPropertyOrder(3)] ValueEqualityDictionary<string, decimal> BaseQuantityComposition)
    : MaybeNamedProto(Name, Symbol);

public class DerivedQuantity : Quantity, IDerived
{
    public override IUnit FundamentalUnit { get; }

    public override NamedComposition<BaseQuantity> BaseQuantityComposition { get; }

    /// <summary>
    /// To be called only from <see cref="Quantity.GetFromBaseComposition(NamedComposition{BaseQuantity})"/>
    /// </summary>
    /// <param name="composition"></param>
    internal DerivedQuantity(Database database, NamedComposition<BaseQuantity> composition, string? fundamentalUnitName = null)
        : base(database, null, null)
    {
        BaseQuantityComposition = composition;
        FundamentalUnit = new DerivedUnit(database, this, fundamentalUnitName);
        Init();
    }

    public override DerivedQuantityProto ToProto()
    {
        return new(
            Name,
            Symbol,
            FundamentalUnit.Name,
            new(BaseQuantityComposition.CompositionAsStringDictionary));
    }
}

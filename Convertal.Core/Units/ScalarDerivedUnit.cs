// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class ScalarDerivedUnit : ScalarUnit, IDerivedUnit
{
    internal IVectorDerivedUnit? SettableVectorAnalog { get; set; }
    public override IVectorDerivedUnit? VectorAnalog => SettableVectorAnalog;

    // for defining from a chain of operations
    internal ScalarDerivedUnit(
        Database database,
        string name,
        ScalarComposition<IUnit> composition)
        : base(database, name, composition)
    {
    }

    // TODO fix method reference
    /// <summary>
    /// To be called only from <see cref="DerivedQuantity.DerivedQuantity(NamedComposition{BaseQuantity})"/>
    /// </summary>
    internal ScalarDerivedUnit(
        Database database,
        ScalarDerivedQuantity quantity,
        string? name = null)
        : base(database, name, quantity, 1m, GetComposition(quantity))
    {
    }
     
    // for defining from an existing IScalarDerivedUnit
    internal ScalarDerivedUnit(
        Database database,
        string name,
        IScalarDerivedUnit otherUnit,
        decimal multiplier,
        decimal offset = 0,
        string? symbol = null)
        : base(database, name, otherUnit, multiplier, offset, symbol: symbol)
    {
    }

    // TODO fix method reference
    /// <summary>
    /// To be called only from <see cref="Database.DefineDerivedUnit(UnitProto)"/>
    /// </summary>
    internal ScalarDerivedUnit(
        Database database,
        string name,
        string? symbol,
        ScalarDerivedQuantity quantity,
        decimal fundamentalMultiplier,
        decimal fundamentalOffset,
        ScalarComposition<IUnit>? composition)
        : base(database, name, symbol, quantity, fundamentalMultiplier, fundamentalOffset, composition)
    {
    }

    private static ScalarComposition<IUnit> GetComposition(ScalarDerivedQuantity quant)
    {
        return ScalarComposition<IUnit>.
            CreateFromExistingBaseComposition(
            quant.BaseQuantityComposition,
            baseQuantity => baseQuantity.FundamentalUnit);
    }
}

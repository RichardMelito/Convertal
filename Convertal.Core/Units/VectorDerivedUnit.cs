// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class VectorDerivedUnit : VectorUnit, IDerivedUnit
{
    // TODO interface or concretion?
    // TODO default
    internal IScalarDerivedUnit? SettableScalarAnalog { get; set; }
    public override IScalarDerivedUnit ScalarAnalog => SettableScalarAnalog!;



    // for defining from a chain of operations
    internal VectorDerivedUnit(
        Database database,
        string name,
        VectorComposition<IUnit> composition)
        : base(database, name, composition)
    {
    }

    // TODO fix method reference
    /// <summary>
    /// To be called only from <see cref="DerivedQuantity.DerivedQuantity(NamedComposition{BaseQuantity})"/>
    /// </summary>
    internal VectorDerivedUnit(
        Database database,
        VectorDerivedQuantity quantity,
        string? name = null)
        : base(database, name, quantity, 1m, GetComposition(quantity))
    {
    }

    // for defining from an existing IVectorDerivedUnit
    internal VectorDerivedUnit(
        Database database,
        string name,
        IVectorDerivedUnit otherUnit,
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
    internal VectorDerivedUnit(
        Database database,
        string name,
        string? symbol,
        VectorDerivedQuantity quantity,
        decimal fundamentalMultiplier,
        decimal fundamentalOffset,
        VectorComposition<IUnit>? composition)
        : base(database, name, symbol, quantity, fundamentalMultiplier, fundamentalOffset, composition)
    {
    }

    private static VectorComposition<IUnit> GetComposition(VectorDerivedQuantity quant)
    {
        return VectorComposition<IUnit>.
            CreateFromExistingBaseComposition(
            quant.BaseQuantityComposition,
            baseQuantity => baseQuantity.FundamentalUnit);
    }
}

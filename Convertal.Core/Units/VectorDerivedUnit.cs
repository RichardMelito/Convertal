// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core;

public class VectorDerivedUnit : VectorUnit, IVectorDerivedUnit
{
    public override VectorDerivedQuantity Quantity => (VectorDerivedQuantity)base.Quantity;

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
        string? symbol = null)
        : base(database, name, otherUnit, multiplier, symbol: symbol)
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
        VectorComposition<IUnit>? composition)
        : base(database, name, symbol, quantity, fundamentalMultiplier, composition)
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

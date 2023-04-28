// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

public abstract class ScalarUnit : Unit, IScalarUnit
{
    public decimal FundamentalOffset { get; }
    public override bool IsVector => false;

    public override ScalarQuantity Quantity => (ScalarQuantity)base.Quantity;

    public override ScalarComposition<IUnit> UnitComposition => (ScalarComposition<IUnit>)base.UnitComposition;

    // TODO consider using generics for this sort of this
    public abstract IVectorUnit? VectorAnalog { get; }

    // for defining from a chain of operations
    internal ScalarUnit(
        Database database,
        string name,
        ScalarComposition<IUnit> composition)
        : base(database, name, composition)
    {
        FundamentalOffset = 0;
    }

    // TODO fix method reference
    /// <summary>
    /// Only to be called from <see cref="BaseQuantity.DefineNewBaseQuantity(string, string, Prefix?)"/>
    /// </summary>
    internal ScalarUnit(
        Database database,
        string? name,
        ScalarQuantity quantity,
        decimal fundamentalMultiplier,
        ScalarComposition<IUnit>? composition = null,
        string? symbol = null)
        : base(database, name, quantity, fundamentalMultiplier, composition, symbol)
    {
        FundamentalOffset = 0;
    }

    // for defining from an existing IScalarUnit
    internal ScalarUnit(
        Database database,
        string? name,
        IScalarUnit otherUnit,
        decimal multiplier,
        decimal offset,
        string? symbol)
        : base(database, name, otherUnit, multiplier, symbol)
    {
        FundamentalOffset = (otherUnit.FundamentalOffset / multiplier) + offset;
    }


    // TODO fix method reference
    /// <summary>
    /// To be called only from <see cref="Database.DefineBaseUnit(UnitProto)"/>
    /// </summary>
    internal ScalarUnit(
        Database database,
        string? name,
        string? symbol,
        ScalarQuantity quantity,
        decimal fundamentalMultiplier,
        decimal fundamentalOffset,
        ScalarComposition<IUnit>? composition)
        : base(database, name, symbol, quantity, fundamentalMultiplier, composition)
    {
        FundamentalOffset = fundamentalOffset;
    }

    public override UnitProto ToProto()
    {
        return new(
            Name,
            Symbol,
            Quantity.ToString(),
            FundamentalMultiplier,
            FundamentalOffset,
            OtherUnitComposition is null ? null : new(OtherUnitComposition.CompositionAsStringDictionary));
    }

    //// TODO
    //public IScalarUnit Pow(decimal power) => throw new NotImplementedException();
}

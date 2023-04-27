// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Text;
using System.Threading.Tasks;

namespace Convertal.Core;

public abstract class VectorUnit : Unit, IVectorUnit
{
    public override bool IsVector => true;

    public override VectorComposition<IUnit> UnitComposition => (VectorComposition<IUnit>)base.UnitComposition;

    // TODO
    public abstract IScalarUnit ScalarAnalog { get; }

    // for defining from a chain of operations
    internal VectorUnit(
        Database database,
        string name,
        VectorComposition<IUnit> composition)
        : base(database, name, composition)
    {
    }

    // TODO fix method reference
    /// <summary>
    /// Only to be called from <see cref="BaseQuantity.DefineNewBaseQuantity(string, string, Prefix?)"/>
    /// </summary>
    internal VectorUnit(
        Database database,
        string? name,
        VectorQuantity quantity,
        decimal fundamentalMultiplier,
        VectorComposition<IUnit>? composition = null,
        string? symbol = null)
        : base(database, name, quantity, fundamentalMultiplier, composition, symbol)
    {
    }

    // for defining from an existing IVectorUnit
    internal VectorUnit(
        Database database,
        string? name,
        IVectorUnit otherUnit,
        decimal multiplier,
        decimal offset,
        string? symbol)
        : base(database, name, otherUnit, multiplier, offset, symbol)
    {
    }

    // TODO fix method reference
    /// <summary>
    /// To be called only from <see cref="Database.DefineBaseUnit(UnitProto)"/>
    /// </summary>
    internal VectorUnit(
        Database database,
        string? name,
        string? symbol,
        VectorQuantity quantity,
        decimal fundamentalMultiplier,
        decimal fundamentalOffset,
        VectorComposition<IUnit>? composition)
        : base(database, name, symbol, quantity, fundamentalMultiplier, fundamentalOffset, composition)
    {
    }

    //// TODO
    //public IScalarUnit DotP(IVectorUnit other) => throw new NotImplementedException();
    //public IVectorUnit CrossP(IVectorUnit other) => throw new NotImplementedException();
}

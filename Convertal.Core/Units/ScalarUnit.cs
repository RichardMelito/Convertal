// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Text.Json.Serialization;

namespace Convertal.Core;

public record ScalarUnitProto(
    string? Name,
    string? Symbol,
    [property: JsonPropertyOrder(2)] string Quantity,
    [property: JsonPropertyOrder(3)] bool HasVectorAnalog,
    [property: JsonPropertyOrder(4)] decimal FundamentalMultiplier,
    [property: JsonPropertyOrder(5), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] decimal FundamentalOffset,
    [property: JsonPropertyOrder(6)] ValueEqualityDictionary<string, decimal>? OtherUnitComposition)
    : MaybeNamedProto(Name, Symbol);

public abstract class ScalarUnit : Unit, IScalarUnit
{
    private VectorUnit? _vectorAnalog;

    public decimal FundamentalOffset { get; }
    public override bool IsVector => false;

    public override ScalarQuantity Quantity => (ScalarQuantity)base.Quantity;

    public override ScalarComposition<IUnit> UnitComposition => (ScalarComposition<IUnit>)base.UnitComposition;

    public virtual VectorUnit? VectorAnalog => _vectorAnalog ??= (Quantity.VectorAnalog is null ? null : MakeVectorAnalog());

    IVectorUnit? IScalar<IScalarUnit, IVectorUnit>.VectorAnalog => VectorAnalog;

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
    /// To be called only from <see cref="Database.DefineBaseUnit(ScalarUnitProto)"/>
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

    protected abstract VectorUnit MakeVectorAnalog();

    //internal void SetVectorAnalog(VectorUnit analog)
    //{
    //    if (VectorAnalog is not null)
    //        throw new InvalidOperationException();

    //    if (analog.Quantity.ScalarAnalog != Quantity)
    //        throw new InvalidOperationException();

    //    VectorAnalog = analog;
    //}

    protected override Type GetDatabaseType() => typeof(ScalarUnit);

    public override ScalarUnitProto ToProto()
    {
        return new(
            Name,
            Symbol,
            Quantity.ToString(),
            _vectorAnalog is not null,
            FundamentalMultiplier,
            FundamentalOffset,
            OtherUnitComposition is null ? null : new(OtherUnitComposition.CompositionAsStringDictionary));
    }
}

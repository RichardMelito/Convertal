// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;

namespace Convertal.Core;

public abstract class ScalarUnit : IUnit, IScalar<ScalarUnit, VectorUnit>
{
    public abstract decimal FundamentalMultiplier { get; }
    public abstract decimal FundamentalOffset { get; }
    public abstract Quantity Quantity { get; }
    public abstract ScalarComposition<IUnit> UnitComposition { get; }
    public abstract Database Database { get; }
    public abstract string? Name { get; }
    public abstract string? Symbol { get; }

    NamedComposition<IUnit> IUnit.UnitComposition => UnitComposition;

    public abstract void Dispose();
    public abstract MaybeNamedProto ToProto();
    public abstract string ToStringSymbol();

    public ScalarUnit Pow(decimal power) => throw new NotImplementedException();
    public static ScalarUnit operator *(ScalarUnit left, ScalarUnit right) => throw new NotImplementedException();
    public static VectorUnit operator *(ScalarUnit scalar, VectorUnit vector) => throw new NotImplementedException();
    public static ScalarUnit operator /(ScalarUnit left, ScalarUnit right) => throw new NotImplementedException();
}


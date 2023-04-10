// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convertal.Core;

public abstract class VectorUnit : IUnit, IVector<VectorUnit, ScalarUnit>
{
    public abstract decimal FundamentalMultiplier { get; }
    public abstract decimal FundamentalOffset { get; }
    public abstract Quantity Quantity { get; }
    public abstract NamedComposition<IUnit> UnitComposition { get; }
    public abstract Database Database { get; }
    public abstract string? Name { get; }
    public abstract string? Symbol { get; }

    public VectorUnit CrossP(VectorUnit other) => throw new NotImplementedException();
    public abstract void Dispose();
    public ScalarUnit DotP(VectorUnit other) => throw new NotImplementedException();
    public abstract MaybeNamedProto ToProto();
    public abstract string ToStringSymbol();
}


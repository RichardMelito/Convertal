// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using DecimalMath;

namespace Convertal.Core;

public record VectorTerm : Term, IVector<VectorTerm, ScalarTerm>
{
    private decimal? _magnitude;
    public override bool IsVector => true;

    public override decimal Magnitude => _magnitude ??= DecimalEx.Sqrt(I*I + J*J + K*K);

    public decimal I { get; init; }
    public decimal J { get; init; }
    public decimal K { get; init; }

    public override IVectorUnit Unit => (IVectorUnit)base.Unit;
    public override VectorQuantity Quantity => (VectorQuantity)base.Quantity;

    protected override string AmountString => $"[{I}_i, {J}_j, {K}_k]";

    // TODO ensure that unit's analogs always have same fundamental multiplier
    public ScalarTerm? ScalarAnalog => Unit.ScalarAnalog?.ToTerm(Magnitude);

    public VectorTerm(decimal magnitude, IVectorUnit unit)
        : base(unit)
    {
        I = J = K = DecimalEx.Sqrt(magnitude / 3);
    }

    public VectorTerm(decimal i, decimal j, decimal k, IVectorUnit unit)
        : base(unit)
    {
        I = i;
        J = j;
        K = k;
    }

    public override VectorTerm ConvertUnitToPreferredSystem(MeasurementSystem? input = null)
        => (VectorTerm)base.ConvertUnitToPreferredSystem(input);

    public override VectorTerm ConvertUnitToFundamental() => (VectorTerm)base.ConvertUnitToFundamental();
    public override VectorTerm ConvertUnitTo(IUnit resultingIUnit)
        => (VectorTerm)base.ConvertUnitTo(resultingIUnit);
    public ScalarTerm DotP(VectorTerm other) => throw new NotImplementedException();
    public VectorTerm CrossP(VectorTerm other) => throw new NotImplementedException();
}

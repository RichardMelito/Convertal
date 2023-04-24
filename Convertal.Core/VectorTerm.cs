// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using Convertal.Core.Extensions;
using DecimalMath;

namespace Convertal.Core;

public record VectorTerm : Term, IVector<VectorTerm, ScalarTerm>
{
    private readonly decimal? _magnitude;
    private readonly decimal _i;
    private readonly decimal _j;
    private readonly decimal _k;

    public override bool IsVector => true;

    public override decimal Magnitude => _magnitude!.Value;

    public decimal I
    {
        get => _i;
        init
        {
            _i = value;
            // These checks for recalculation are for the case where a caller uses the "with" keyword
            // to make a mutated copy of a Term. Based on my understanding, the copy will have the original
            // Term's _magnitude, so it will need to be recalculated.
            if (_magnitude.HasValue)
                _magnitude = CalculateMagnitude();
        }
    }
    public decimal J
    {
        get => _j;
        init
        {
            _j = value;
            if (_magnitude.HasValue)
                _magnitude = CalculateMagnitude();
        }
    }
    public decimal K
    {
        get => _k;
        init
        {
            _k = value;
            if (_magnitude.HasValue)
                _magnitude = CalculateMagnitude();
        }
    }

    public override IVectorUnit Unit => (IVectorUnit)base.Unit;
    public override VectorQuantity Quantity => (VectorQuantity)base.Quantity;

    protected override string AmountString => $"[{I}_i, {J}_j, {K}_k]";

    // TODO ensure that unit's analogs always have same fundamental multiplier
    public ScalarTerm? ScalarAnalog => Unit.ScalarAnalog?.ToTerm(Magnitude);

    public VectorTerm(decimal magnitude, IVectorUnit unit)
        : base(unit)
    {
        I = J = K = DecimalEx.Sqrt(magnitude / 3);
        _magnitude = magnitude;
    }

    public VectorTerm(decimal i, decimal j, decimal k, IVectorUnit unit)
        : base(unit)
    {
        I = i;
        J = j;
        K = k;
        _magnitude = CalculateMagnitude();
    }

    private decimal CalculateMagnitude() => DecimalEx.Sqrt(I * I + J * J + K * K);


    public VectorTerm ConvertUnitToPreferredSystem(MeasurementSystem? input = null)
    {
        var system = input ?? MeasurementSystem.Current;
        var resultingUnit = system?.GetVectorUnit(Quantity) ?? Quantity.FundamentalUnit;
        return ConvertUnitTo(resultingUnit);
    }

    public VectorTerm ConvertUnitToFundamental()
    {
        return Unit.ConvertToFundamental(I, J, K);
    }

    public VectorTerm ConvertUnitTo(IVectorUnit resultingIUnit)
    {
        return Unit.ConvertTo(I, J, K, resultingIUnit);
    }

    public ScalarTerm DotP(VectorTerm other)
    {
        var fund = ConvertUnitToFundamental();
        var otherFund = other.ConvertUnitToFundamental();

        // It looks like we can't actually use the interface operators much....
        var resQuantity = Quantity.DotP(other.Quantity);
        return new(
            fund.I * otherFund.I + fund.J * otherFund.J + fund.K * otherFund.K,
            resQuantity.FundamentalUnit);
    }
    public VectorTerm CrossP(VectorTerm other)
    {
        var fund = ConvertUnitToFundamental();
        var otherFund = other.ConvertUnitToFundamental();

        var resQuantity = Quantity.CrossP(other.Quantity);
        return new(
            fund.J * otherFund.K - fund.K * otherFund.J,
            fund.K * otherFund.I - fund.I * otherFund.K,
            fund.I * otherFund.J - fund.J * otherFund.I,
            resQuantity.FundamentalUnit);
    }


    public VectorTerm Multiply(ScalarTerm scalar) => scalar.Multiply(this);

    public VectorTerm Divide(ScalarTerm scalar)
    {
        var fund = ConvertUnitToFundamental();
        var scalarFund = scalar.ConvertUnitToFundamental();
        var resQuantity = Quantity.Divide(scalar);
        return new(
            fund.I / scalarFund.Magnitude,
            fund.J / scalarFund.Magnitude,
            fund.K / scalarFund.Magnitude,
            resQuantity.FundamentalUnit);
    }

    public override VectorTerm Multiply(decimal multiplier) => new(
        I * multiplier,
        J * multiplier,
        K * multiplier,
        Unit);

    public override VectorTerm Divide(decimal divisor) => new(
        I / divisor,
        J / divisor,
        K / divisor,
        Unit);


    public static VectorTerm operator *(VectorTerm term, decimal multiplier) => term.Multiply(multiplier);
    public static VectorTerm operator *(decimal multiplier, VectorTerm term) => term.Multiply(multiplier);
    public static VectorTerm operator /(VectorTerm term, decimal divisor) => term.Divide(divisor);

    public static VectorTerm operator +(VectorTerm lhs, VectorTerm rhs)
    {
        var convertedRhs = rhs.ConvertUnitTo(lhs.Unit);
        return new(
            lhs.I + convertedRhs.I,
            lhs.J + convertedRhs.J,
            lhs.K + convertedRhs.K,
            lhs.Unit);
    }

    public static VectorTerm operator -(VectorTerm lhs, VectorTerm rhs)
    {
        var convertedRhs = rhs.ConvertUnitTo(lhs.Unit);
        return new(
            lhs.I - convertedRhs.I,
            lhs.J - convertedRhs.J,
            lhs.K - convertedRhs.K,
            lhs.Unit);
    }
}

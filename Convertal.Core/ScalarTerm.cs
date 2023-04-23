// Created by Richard Melito and licensed to you under The Clear BSD License.

using DecimalMath;

namespace Convertal.Core;

public record ScalarTerm : Term, IScalar<ScalarTerm, VectorTerm>
{
    public override bool IsVector => false;

    public override decimal Magnitude { get; }
    public override IScalarUnit Unit => (IScalarUnit)base.Unit;
    public override ScalarQuantity Quantity => (ScalarQuantity)base.Quantity;

    public VectorTerm? VectorAnalog => Unit.VectorAnalog?.ToTerm(Magnitude);

    public ScalarTerm(decimal magnitude, IScalarUnit unit): base(unit) => Magnitude = magnitude;


    public ScalarTerm Pow(decimal power)
    {
        var fundamental = ConvertUnitToFundamental();
        var resMagnitude = DecimalEx.Pow(fundamental.Magnitude, power);
        var resQuantity = fundamental.Quantity.Pow(power);
        return new(resMagnitude, resQuantity.FundamentalUnit);
    }


    public ScalarTerm ConvertUnitToPreferredSystem(MeasurementSystem? input = null)
    {
        var system = input ?? MeasurementSystem.Current;
        var resultingUnit = system?.GetScalarUnit(Quantity) ?? Quantity.FundamentalUnit;
        return ConvertUnitTo(resultingUnit);
    }

    public ScalarTerm ConvertUnitToFundamental()
    {
        return Unit.ConvertToFundamental(Magnitude);
    }

    public ScalarTerm ConvertUnitTo(IScalarUnit resultingIUnit)
    {
        return Unit.ConvertTo(Magnitude, resultingIUnit);
    }
}

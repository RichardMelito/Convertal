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

    public override ScalarTerm ConvertUnitToPreferredSystem(MeasurementSystem? input = null)
        => (ScalarTerm)base.ConvertUnitToPreferredSystem(input);

    public override ScalarTerm ConvertUnitToFundamental() => (ScalarTerm)base.ConvertUnitToFundamental();
    public override ScalarTerm ConvertUnitTo(IUnit resultingIUnit)
        => (ScalarTerm)base.ConvertUnitTo(resultingIUnit);
}

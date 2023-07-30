// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core.Defaults;

public class VectorDerivedQuantities
{
    public readonly VectorDerivedQuantity Area;
    public readonly VectorDerivedQuantity Velocity;
    public readonly VectorDerivedQuantity Acceleration;
    public readonly VectorDerivedQuantity Force;
    public readonly VectorDerivedQuantity Torque;
    public readonly VectorDerivedQuantity AngularVelocity;
    public readonly VectorDerivedQuantity AngularAcceleration;

    VectorDerivedQuantities()
    {
        Frequency = (DerivedQuantity)Time.Pow(-1m);
        Area = (DerivedQuantity)(Length * Length);
        Volume = (DerivedQuantity)(Area * Length);
        VolumeFlowRate = (DerivedQuantity)(Volume / Time);
        Speed = (DerivedQuantity)(Length / Time);
        Acceleration = (DerivedQuantity)(Speed / Time);
        Force = (DerivedQuantity)(Mass * Acceleration);
        Pressure = (DerivedQuantity)(Force / Area);
        EnergyAndTorque = (DerivedQuantity)(Force * Length);
        Power = (DerivedQuantity)(EnergyAndTorque / Time);
        //Torque = (DerivedQuantity)(Force * Length);
        AngularVelocity = (DerivedQuantity)(Angle / Time);
        AngularAcceleration = (DerivedQuantity)(AngularVelocity / Time);
        WaveNumber = (DerivedQuantity)(Length.Pow(-1m));
        Density = (DerivedQuantity)(Mass / Volume);
        SurfaceDensity = (DerivedQuantity)(Mass / Area);
        ElectricCharge = (DerivedQuantity)(ElectricCurrent * Time);
        Voltage = (DerivedQuantity)(EnergyAndTorque / ElectricCharge);
        ElectricCapacitance = (DerivedQuantity)(ElectricCharge / Voltage);
        ElectricResistance = (DerivedQuantity)(Voltage / ElectricCurrent);
        ElectricConductance = (DerivedQuantity)(ElectricResistance.Pow(-1m));
        MagneticFlux = (DerivedQuantity)(Voltage * Time);
        MagneticFluxDensity = (DerivedQuantity)(MagneticFlux / Area);
        Inductance = (DerivedQuantity)(MagneticFlux / ElectricCurrent);
        LuminousFlux = (DerivedQuantity)(LuminousIntensity * SolidAngle);
        Illuminance = (DerivedQuantity)(LuminousFlux / Area);
        SpecificVolume = (DerivedQuantity)(Density.Pow(-1m));
        ElectricCurrentDensity = (DerivedQuantity)(ElectricCurrent / Area);
        MagneticFieldStrength = (DerivedQuantity)(ElectricCurrent / Length);
        Luminance = (DerivedQuantity)(LuminousIntensity / Area);
        DynamicViscosity = (DerivedQuantity)(Pressure * Time);
        KinematicViscosity = (DerivedQuantity)(DynamicViscosity / Density);
        PowerFlux = (DerivedQuantity)(Power / Area);
        HeatCapacity = (DerivedQuantity)(EnergyAndTorque / Temperature);
        SpecificHeatCapacity = (DerivedQuantity)(HeatCapacity / Mass);
        SpecificEnergy = (DerivedQuantity)(EnergyAndTorque / Mass);
        ThermalConductivity = (DerivedQuantity)(Power / (Length * Temperature));
        //EnergyDensity = (DerivedQuantity)(Energy / Volume);
        ElectricFieldStrength = (DerivedQuantity)(Voltage / Length);
        ElectricChargeDensity = (DerivedQuantity)(ElectricCharge / Volume);
        SurfaceChargeDensity = (DerivedQuantity)(ElectricCharge / Area);
        Permittivity = (DerivedQuantity)(ElectricCapacitance / Length);
        Permeability = (DerivedQuantity)(Inductance / Length);

        foreach (var field in typeof(DerivedQuantities).GetFields())
        {
            var quantity = (DerivedQuantity)field.GetValue(field.Name)!;
            quantity.ChangeName(field.Name);
        }

        Frequency.ChangeSymbol("f");
        Area.ChangeSymbol("A");
        Volume.ChangeSymbol("V");
        VolumeFlowRate.ChangeSymbol("Vdot");
        Speed.ChangeSymbol("v");
        Acceleration.ChangeSymbol("a");
        Force.ChangeSymbol("F");
        Pressure.ChangeSymbol("p");
        EnergyAndTorque.ChangeSymbol("E");
        Power.ChangeSymbol("P");
        //Torque.ChangeSymbol("τ");
        AngularVelocity.ChangeSymbol("ω");
        AngularAcceleration.ChangeSymbol("α");
        WaveNumber.ChangeSymbol("σ");
        Density.ChangeSymbol("ρ");
        ElectricCharge.ChangeSymbol("q");
        Voltage.ChangeSymbol("ℰ");
        ElectricCapacitance.ChangeSymbol("C");
        ElectricResistance.ChangeSymbol("R");
        MagneticFlux.ChangeSymbol("Φ");
        MagneticFluxDensity.ChangeSymbol("B");
        Inductance.ChangeSymbol("L");
        LuminousFlux.ChangeSymbol("Φv");
        Illuminance.ChangeSymbol("Ev");
        ElectricCurrentDensity.ChangeSymbol("j");
        MagneticFieldStrength.ChangeSymbol("H");
        Luminance.ChangeSymbol("Lv");
        DynamicViscosity.ChangeSymbol("μ");
        KinematicViscosity.ChangeSymbol("ν");
        SpecificHeatCapacity.ChangeSymbol("cp");
        SpecificEnergy.ChangeSymbol("e");
        ThermalConductivity.ChangeSymbol("k");
        Permittivity.ChangeSymbol("ε");
    }
}

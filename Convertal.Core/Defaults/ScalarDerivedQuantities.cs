// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core.Defaults;

public class ScalarDerivedQuantities
{
    public readonly ScalarDerivedQuantity Frequency;
    public readonly ScalarDerivedQuantity Area;
    public readonly ScalarDerivedQuantity Volume;
    public readonly ScalarDerivedQuantity VolumeFlowRate;
    public readonly ScalarDerivedQuantity Speed;
    public readonly ScalarDerivedQuantity Acceleration;
    public readonly ScalarDerivedQuantity Force;
    public readonly ScalarDerivedQuantity Pressure;
    public readonly ScalarDerivedQuantity Energy;
    public readonly ScalarDerivedQuantity Power;
    public readonly ScalarDerivedQuantity AngularVelocity;
    public readonly ScalarDerivedQuantity AngularAcceleration;
    public readonly ScalarDerivedQuantity WaveNumber;
    public readonly ScalarDerivedQuantity Density;
    public readonly ScalarDerivedQuantity SurfaceDensity;
    public readonly ScalarDerivedQuantity ElectricCharge;
    public readonly ScalarDerivedQuantity Voltage;
    public readonly ScalarDerivedQuantity ElectricCapacitance;
    public readonly ScalarDerivedQuantity ElectricResistance;
    public readonly ScalarDerivedQuantity ElectricConductance;
    public readonly ScalarDerivedQuantity MagneticFlux;
    public readonly ScalarDerivedQuantity MagneticFluxDensity;
    public readonly ScalarDerivedQuantity Inductance;
    public readonly ScalarDerivedQuantity LuminousFlux;
    public readonly ScalarDerivedQuantity Illuminance;
    public readonly ScalarDerivedQuantity SpecificVolume;
    public readonly ScalarDerivedQuantity ElectricCurrentDensity;
    public readonly ScalarDerivedQuantity MagneticFieldStrength;
    public readonly ScalarDerivedQuantity Luminance;
    public readonly ScalarDerivedQuantity DynamicViscosity;
    public readonly ScalarDerivedQuantity KinematicViscosity;
    public readonly ScalarDerivedQuantity PowerFlux;
    public readonly ScalarDerivedQuantity HeatCapacity;
    public readonly ScalarDerivedQuantity SpecificHeatCapacity;
    public readonly ScalarDerivedQuantity SpecificEnergy;
    public readonly ScalarDerivedQuantity ThermalConductivity;
    public readonly ScalarDerivedQuantity EnergyDensity;
    public readonly ScalarDerivedQuantity ElectricFieldStrength;
    public readonly ScalarDerivedQuantity ElectricChargeDensity;
    public readonly ScalarDerivedQuantity SurfaceChargeDensity;
    public readonly ScalarDerivedQuantity Permittivity;
    public readonly ScalarDerivedQuantity Permeability;

    internal ScalarDerivedQuantities(DefaultDatabaseWrapper defaultDatabase)
    {
        var sbqs = defaultDatabase.ScalarBaseQuantities;
        Frequency = sbqs.Time.Pow(-1m).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Frequency), "f");
        Area = (sbqs.Length * sbqs.Length).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Area), "A");
        Volume = (Area * sbqs.Length).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Volume), "V");
        VolumeFlowRate = (Volume / sbqs.Time).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(VolumeFlowRate), "Vdot");
        Speed = (sbqs.Length / sbqs.Time).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Speed));
        Acceleration = (Speed / sbqs.Time);
        Force = (sbqs.Mass * Acceleration);
        Pressure = (Force / Area);
        EnergyAndTorque = (Force * sbqs.Length);
        Power = (EnergyAndTorque / sbqs.Time);
        //Torque = (Force * Length);
        AngularVelocity = (sbqs.Angle / sbqs.Time);
        AngularAcceleration = (AngularVelocity / sbqs.Time);
        WaveNumber = (sbqs.Length.Pow(-1m));
        Density = (sbqs.Mass / Volume);
        SurfaceDensity = (sbqs.Mass / Area);
        ElectricCharge = (ElectricCurrent * sbqs.Time);
        Voltage = (EnergyAndTorque / ElectricCharge);
        ElectricCapacitance = (ElectricCharge / Voltage);
        ElectricResistance = (Voltage / ElectricCurrent);
        ElectricConductance = (ElectricResistance.Pow(-1m));
        MagneticFlux = (Voltage * sbqs.Time);
        MagneticFluxDensity = (MagneticFlux / Area);
        Inductance = (MagneticFlux / ElectricCurrent);
        LuminousFlux = (LuminousIntensity * SolidAngle);
        Illuminance = (LuminousFlux / Area);
        SpecificVolume = (Density.Pow(-1m));
        ElectricCurrentDensity = (ElectricCurrent / Area);
        MagneticFieldStrength = (ElectricCurrent / sbqs.Length);
        Luminance = (LuminousIntensity / Area);
        DynamicViscosity = (Pressure * sbqs.Time);
        KinematicViscosity = (DynamicViscosity / Density);
        PowerFlux = (Power / sbqs.Area);
        HeatCapacity = (EnergyAndTorque / sbqs.Temperature);
        SpecificHeatCapacity = (HeatCapacity / sbqs.Mass);
        SpecificEnergy = (EnergyAndTorque / sbqs.Mass);
        ThermalConductivity = (Power / (sbqs.Length * sbqs.Temperature));
        //EnergyDensity = (Energy / Volume);
        ElectricFieldStrength = (Voltage / sbqs.Length);
        ElectricChargeDensity = (ElectricCharge / Volume);
        SurfaceChargeDensity = (ElectricCharge / Area);
        Permittivity = (ElectricCapacitance / sbqs.Length);
        Permeability = (Inductance / sbqs.Length);

        foreach (var field in typeof(DerivedQuantities).GetFields())
        {
            var quantity = field.GetValue(field.Name)!;
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

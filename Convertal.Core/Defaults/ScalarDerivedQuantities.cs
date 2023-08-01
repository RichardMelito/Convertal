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
    public readonly ScalarDerivedQuantity AngularSpeed;
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
        Acceleration = (Speed / sbqs.Time).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Acceleration), "a");
        Force = (sbqs.Mass * Acceleration).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Force), "F");
        Pressure = (Force / Area).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Pressure), "p");
        Energy = (Force * sbqs.Length).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Energy), "E");
        Power = (Energy / sbqs.Time).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Power), "P");
        AngularSpeed = (sbqs.Angle / sbqs.Time).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(AngularSpeed), "ω");
        AngularAcceleration = (AngularSpeed / sbqs.Time).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(AngularAcceleration), "α");
        WaveNumber = sbqs.Length.Pow(-1m).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(WaveNumber), "σ");
        Density = (sbqs.Mass / Volume).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Density), "ρ");
        SurfaceDensity = (sbqs.Mass / Area).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(SurfaceDensity));
        ElectricCharge = (sbqs.ElectricCurrent * sbqs.Time).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(ElectricCharge), "q");
        Voltage = (Energy / ElectricCharge).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Voltage), "ℰ");
        ElectricCapacitance = (ElectricCharge / Voltage).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(ElectricCapacitance), "C");
        ElectricResistance = (Voltage / sbqs.ElectricCurrent).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(ElectricResistance), "R");
        ElectricConductance = ElectricResistance.Pow(-1m).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(ElectricConductance));
        MagneticFlux = (Voltage * sbqs.Time).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(MagneticFlux), "Φ");
        MagneticFluxDensity = (MagneticFlux / Area).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(MagneticFluxDensity), "B");
        Inductance = (MagneticFlux / sbqs.ElectricCurrent).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Inductance), "L");
        LuminousFlux = (sbqs.LuminousIntensity * sbqs.SolidAngle).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(LuminousFlux), "Φv");
        Illuminance = (LuminousFlux / Area).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Illuminance), "Ev");
        SpecificVolume = Density.Pow(-1m).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(SpecificVolume));
        ElectricCurrentDensity = (sbqs.ElectricCurrent / Area).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(ElectricCurrentDensity), "j");
        MagneticFieldStrength = (sbqs.ElectricCurrent / sbqs.Length).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(MagneticFieldStrength), "H");
        Luminance = (sbqs.LuminousIntensity / Area).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Luminance), "Lv");
        DynamicViscosity = (Pressure * sbqs.Time).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(DynamicViscosity), "μ");
        KinematicViscosity = (DynamicViscosity / Density).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(KinematicViscosity), "ν");
        PowerFlux = (Power / Area).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(PowerFlux));
        HeatCapacity = (Energy / sbqs.Temperature).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(HeatCapacity));
        SpecificHeatCapacity = (HeatCapacity / sbqs.Mass).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(SpecificHeatCapacity), "cp");
        SpecificEnergy = (Energy / sbqs.Mass).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(SpecificEnergy), "e");
        ThermalConductivity = (Power / (sbqs.Length * sbqs.Temperature)).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(ThermalConductivity), "k");
        EnergyDensity = (Energy / Volume).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(EnergyDensity));
        ElectricFieldStrength = (Voltage / sbqs.Length).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(ElectricFieldStrength));
        ElectricChargeDensity = (ElectricCharge / Volume).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(ElectricChargeDensity));
        SurfaceChargeDensity = (ElectricCharge / Area).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(SurfaceChargeDensity));
        Permittivity = (ElectricCapacitance / sbqs.Length).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Permittivity), "ε");
        Permeability = (Inductance / sbqs.Length).CastAndChangeNameAndSymbol<ScalarDerivedQuantity>(nameof(Permeability));
    }
}

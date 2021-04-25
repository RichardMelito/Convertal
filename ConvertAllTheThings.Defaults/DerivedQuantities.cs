using System;
using ConvertAllTheThings.Core;
using static ConvertAllTheThings.Core.DerivedQuantity;
using static ConvertAllTheThings.Defaults.BaseQuantities;

using System.Reflection;

namespace ConvertAllTheThings.Defaults
{
    public static class DerivedQuantities
    {
        public static readonly DerivedQuantity Frequency;
        public static readonly DerivedQuantity Area;
        public static readonly DerivedQuantity Volume;
        public static readonly DerivedQuantity VolumeFlowRate;
        public static readonly DerivedQuantity Speed;
        public static readonly DerivedQuantity Acceleration;
        public static readonly DerivedQuantity Force;
        public static readonly DerivedQuantity Pressure;
        public static readonly DerivedQuantity EnergyAndTorque; // the same until vector stuff is implemented
        public static readonly DerivedQuantity Power;
        //public static readonly DerivedQuantity Torque;
        public static readonly DerivedQuantity AngularVelocity;
        public static readonly DerivedQuantity AngularAcceleration;
        public static readonly DerivedQuantity WaveNumber;
        public static readonly DerivedQuantity Density;
        public static readonly DerivedQuantity SurfaceDensity;
        public static readonly DerivedQuantity ElectricCharge;
        public static readonly DerivedQuantity Voltage;
        public static readonly DerivedQuantity ElectricCapacitance;
        public static readonly DerivedQuantity ElectricResistance;
        public static readonly DerivedQuantity ElectricConductance;
        public static readonly DerivedQuantity MagneticFlux;
        public static readonly DerivedQuantity MagneticFluxDensity;
        public static readonly DerivedQuantity Inductance;
        public static readonly DerivedQuantity LuminousFlux;
        public static readonly DerivedQuantity Illuminance;
        public static readonly DerivedQuantity SpecificVolume;
        public static readonly DerivedQuantity ElectricCurrentDensity;
        public static readonly DerivedQuantity MagneticFieldStrength;
        public static readonly DerivedQuantity Luminance;
        public static readonly DerivedQuantity DynamicViscosity;
        public static readonly DerivedQuantity KinematicViscosity;
        public static readonly DerivedQuantity PowerFlux;
        public static readonly DerivedQuantity HeatCapacity;
        public static readonly DerivedQuantity SpecificHeatCapacity;
        public static readonly DerivedQuantity SpecificEnergy;
        public static readonly DerivedQuantity ThermalConductivity;
        //public static readonly DerivedQuantity EnergyDensity;
        public static readonly DerivedQuantity ElectricFieldStrength;
        public static readonly DerivedQuantity ElectricChargeDensity;
        public static readonly DerivedQuantity SurfaceChargeDensity;
        public static readonly DerivedQuantity Permittivity;
        public static readonly DerivedQuantity Permeability;

        static DerivedQuantities()
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
}

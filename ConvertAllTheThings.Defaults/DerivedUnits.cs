using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core;
using static ConvertAllTheThings.Core.Unit;
using static DecimalMath.DecimalEx;
using static ConvertAllTheThings.Defaults.DerivedQuantities;
using static ConvertAllTheThings.Defaults.BaseUnits;

namespace ConvertAllTheThings.Defaults
{
    public static class DerivedUnits
    {
        #region Fundamental units
        public static readonly DerivedUnit Hertz = (DerivedUnit)Frequency.FundamentalUnit;
        public static readonly DerivedUnit SquareMeter = (DerivedUnit)Area.FundamentalUnit;
        public static readonly DerivedUnit CubicMeter = (DerivedUnit)Volume.FundamentalUnit;
        public static readonly DerivedUnit CubicMeterPerSecond = (DerivedUnit)VolumeFlowRate.FundamentalUnit;
        public static readonly DerivedUnit MeterPerSecond = (DerivedUnit)Speed.FundamentalUnit;
        public static readonly DerivedUnit MeterPerSecondSquared = (DerivedUnit)Acceleration.FundamentalUnit;
        public static readonly DerivedUnit Newton = (DerivedUnit)Force.FundamentalUnit;
        public static readonly DerivedUnit Pascal = (DerivedUnit)Pressure.FundamentalUnit;
        public static readonly DerivedUnit Joule = (DerivedUnit)EnergyAndTorque.FundamentalUnit;
        public static readonly DerivedUnit Watt = (DerivedUnit)Power.FundamentalUnit;
        public static readonly DerivedUnit RadianPerSecond = (DerivedUnit)AngularVelocity.FundamentalUnit;
        public static readonly DerivedUnit RadianPerSecondSquared = (DerivedUnit)AngularAcceleration.FundamentalUnit;
        public static readonly DerivedUnit InverseMeter = (DerivedUnit)WaveNumber.FundamentalUnit;
        public static readonly DerivedUnit KilogramPerCubicMeter = (DerivedUnit)Density.FundamentalUnit;
        public static readonly DerivedUnit KilogramPerSquareMeter = (DerivedUnit)SurfaceDensity.FundamentalUnit;
        public static readonly DerivedUnit Coulomb = (DerivedUnit)ElectricCharge.FundamentalUnit;
        public static readonly DerivedUnit Volt = (DerivedUnit)Voltage.FundamentalUnit;
        public static readonly DerivedUnit Farad = (DerivedUnit)ElectricCapacitance.FundamentalUnit;
        public static readonly DerivedUnit Ohm = (DerivedUnit)ElectricResistance.FundamentalUnit;
        public static readonly DerivedUnit Siemen = (DerivedUnit)ElectricConductance.FundamentalUnit;
        public static readonly DerivedUnit Weber = (DerivedUnit)MagneticFlux.FundamentalUnit;
        public static readonly DerivedUnit Tesla = (DerivedUnit)MagneticFluxDensity.FundamentalUnit;
        public static readonly DerivedUnit Henry = (DerivedUnit)Inductance.FundamentalUnit;
        public static readonly DerivedUnit Lumen = (DerivedUnit)LuminousFlux.FundamentalUnit;
        public static readonly DerivedUnit Lux = (DerivedUnit)Illuminance.FundamentalUnit;
        public static readonly DerivedUnit CubicMeterPerKilogram = (DerivedUnit)SpecificVolume.FundamentalUnit;
        public static readonly DerivedUnit AmperePerSquareMeter = (DerivedUnit)ElectricCurrentDensity.FundamentalUnit;
        public static readonly DerivedUnit AmperePerMeter = (DerivedUnit)MagneticFieldStrength.FundamentalUnit;
        public static readonly DerivedUnit CandelaPerSquareMeter = (DerivedUnit)Luminance.FundamentalUnit;
        public static readonly DerivedUnit PascalSecond = (DerivedUnit)DynamicViscosity.FundamentalUnit;
        public static readonly DerivedUnit MeterSquaredPerSecond = (DerivedUnit)KinematicViscosity.FundamentalUnit;
        public static readonly DerivedUnit WattPerSquareMeter = (DerivedUnit)PowerFlux.FundamentalUnit;
        public static readonly DerivedUnit JoulePerKelvin = (DerivedUnit)HeatCapacity.FundamentalUnit;
        public static readonly DerivedUnit JoulePerKilogramKelvin = (DerivedUnit)SpecificHeatCapacity.FundamentalUnit;
        public static readonly DerivedUnit JoulePerCubicMeter = (DerivedUnit)SpecificEnergy.FundamentalUnit;
        public static readonly DerivedUnit WattPerMeterKelvin = (DerivedUnit)ThermalConductivity.FundamentalUnit;
        public static readonly DerivedUnit VoltPerMeter = (DerivedUnit)ElectricFieldStrength.FundamentalUnit;
        public static readonly DerivedUnit CoulombPerCubicMeter = (DerivedUnit)ElectricChargeDensity.FundamentalUnit;
        public static readonly DerivedUnit CoulombPerSquareMeter = (DerivedUnit)SurfaceChargeDensity.FundamentalUnit;
        public static readonly DerivedUnit FaradPerMeter = (DerivedUnit)Permittivity.FundamentalUnit;
        public static readonly DerivedUnit HenryPerMeter = (DerivedUnit)Permeability.FundamentalUnit;
        #endregion

        #region Area
        public static readonly DerivedUnit SquareMile;
        public static readonly DerivedUnit SquareYard;
        #endregion

        static DerivedUnits()
        {
            #region Area
            SquareMile = (DerivedUnit)DefineFromComposition(nameof(SquareMile), Mile.UnitComposition.Pow(2m));
            SquareYard = (DerivedUnit)DefineFromComposition(nameof(SquareYard), Multiply(Yard, Yard));
            #endregion

            foreach (var field in typeof(DerivedUnits).GetFields())
            {
                var unit = (DerivedUnit)field.GetValue(field.Name)!;
                if (unit.MaybeName is not null)
                    continue;

                unit.ChangeName(field.Name);
            }
        }
    }
}

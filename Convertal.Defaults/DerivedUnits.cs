// Created by Richard Melito and licensed to you under The Clear BSD License.

using Convertal.Core;
using static Convertal.Defaults.BaseUnits;
using static Convertal.Defaults.DerivedQuantities;
using static Convertal.Defaults.Prefixes;
using static DecimalMath.DecimalEx;

namespace Convertal.Defaults;

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
    public static readonly DerivedUnit Are;
    public static readonly DerivedUnit Acre;
    public static readonly DerivedUnit Arpent;
    public static readonly DerivedUnit Barn;
    public static readonly DerivedUnit CircularInch;
    public static readonly DerivedUnit SquareFoot;
    public static readonly DerivedUnit SquareInch;
    public static readonly DerivedUnit SquareMile;
    public static readonly DerivedUnit SquareYard;
    public static readonly DerivedUnit SquareRod;
    public static readonly DerivedUnit SquareMil;
    public static readonly DerivedUnit SquareKiloMeter;
    public static readonly DerivedUnit Kanal;
    public static readonly DerivedUnit Rood;
    public static readonly DerivedUnit Township;
    public static readonly DerivedUnit Yardland;
    #endregion

    #region Volume
    public static readonly DerivedUnit AcreFoot;
    public static readonly DerivedUnit AcreInch;
    public static readonly DerivedUnit AmericanStandardBarrel;
    public static readonly DerivedUnit BarrelOfOil;
    public static readonly DerivedUnit BoardFoot;
    public static readonly DerivedUnit Bushel;
    public static readonly DerivedUnit Butt;
    public static readonly DerivedUnit Cord;
    public static readonly DerivedUnit CubicCentimeter;
    public static readonly DerivedUnit CubicDecimeter;
    public static readonly DerivedUnit Liter;
    public static readonly DerivedUnit CubicFoot;
    public static readonly DerivedUnit CubicInch;
    public static readonly DerivedUnit CubicMile;
    public static readonly DerivedUnit CubicYard;
    public static readonly DerivedUnit Cup;
    public static readonly DerivedUnit Dram;
    public static readonly DerivedUnit FluidOunce;
    public static readonly DerivedUnit Gallon;
    public static readonly DerivedUnit Gill;
    public static readonly DerivedUnit Hogshead;
    public static readonly DerivedUnit Jigger;
    public static readonly DerivedUnit Minim;
    public static readonly DerivedUnit Peck;
    public static readonly DerivedUnit Pint;
    public static readonly DerivedUnit Quart;
    public static readonly DerivedUnit Tablespoon;
    public static readonly DerivedUnit Teaspoon;
    #endregion

    static DerivedUnits()
    {
        #region Area
        SquareFoot = Make(nameof(SquareFoot), Foot, 2m);
        SquareInch = Make(nameof(SquareInch), Inch, 2m);
        SquareMile = Make(nameof(SquareMile), Mile, 2m);
        SquareYard = Make(nameof(SquareYard), Yard, 2m);
        SquareRod = Make(nameof(SquareRod), Rod, 2m);
        SquareMil = Make(nameof(SquareMil), Mil, 2m);
        SquareKiloMeter = Make(nameof(SquareKiloMeter), Kilo[Meter], 2m);

        Are = new(nameof(Are), SquareMeter, 100m);
        Acre = new(nameof(Acre), SquareMile, 1m / 640m);
        Arpent = new(nameof(Arpent), SquareFoot, 36800m);
        Barn = new(nameof(Barn), SquareMeter, 1e-28m);
        CircularInch = new(nameof(CircularInch), SquareMil, PiQuarter);
        Kanal = new(nameof(Kanal), SquareYard, 650m);
        Rood = new(nameof(Rood), SquareRod, 40m);
        Township = new(nameof(Township), SquareMile, 36m);
        Yardland = new(nameof(Yardland), Acre, 30m);
        #endregion

        #region Volume
        Liter = new(nameof(Liter), CubicMeter, 1e-3m);

        AcreFoot = Make(nameof(AcreFoot), Acre.UC * Foot.UC);
        AcreInch = Make(nameof(AcreInch), Acre.UC * Inch.UC);
        AmericanStandardBarrel = new(nameof(AmericanStandardBarrel), Liter, 200m);
        BoardFoot = Make(nameof(BoardFoot), Foot.UC * Foot.UC * Inch.UC);
        //Bushel = new(nameof(Bushel), Gallon, 8m);
        //Butt = new(nameof(Butt), Hogshead, 2m);
        //Cup = new(nameof(Cup), FluidOunce, 8m);
        //Cord = new(nameof(Cord), CubicFoot, 128m);
        CubicCentimeter = Make(nameof(CubicCentimeter), Centi[Meter], 3m);
        CubicDecimeter = Make(nameof(CubicDecimeter), Deci[Meter], 3m);
        //BarrelOfOil = new(nameof(BarrelOfOil), Gallon, 42m);
        #endregion

        return;
        foreach (var field in typeof(DerivedUnits).GetFields())
        {
            var unit = (DerivedUnit)field.GetValue(field.Name)!;
            if (unit.Name is not null)
                continue;

            unit.ChangeName(field.Name);
        }
    }



    private static DerivedUnit Make(string name, NamedComposition<IUnit> composition)
    {
        return (DerivedUnit)DefineFromComposition(name, composition);
    }

    private static DerivedUnit Make(string name, IUnit unit, decimal power)
    {
        return (DerivedUnit)DefineFromComposition(name, unit.UnitComposition.Pow(power));
    }
}

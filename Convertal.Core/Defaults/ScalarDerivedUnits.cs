// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Diagnostics.Metrics;

namespace Convertal.Core.Defaults;

public class ScalarDerivedUnits
{
    #region Fundamental units
    public readonly ScalarDerivedUnit Hertz;
    public readonly ScalarDerivedUnit SquareMeter;
    public readonly ScalarDerivedUnit CubicMeter;
    public readonly ScalarDerivedUnit CubicMeterPerSecond;
    public readonly ScalarDerivedUnit MeterPerSecond;
    public readonly ScalarDerivedUnit MeterPerSecondSquared;
    public readonly ScalarDerivedUnit Newton;
    public readonly ScalarDerivedUnit Pascal;
    public readonly ScalarDerivedUnit Joule;
    public readonly ScalarDerivedUnit Watt;
    public readonly ScalarDerivedUnit RadianPerSecond;
    public readonly ScalarDerivedUnit RadianPerSecondSquared;
    public readonly ScalarDerivedUnit InverseMeter;
    public readonly ScalarDerivedUnit KilogramPerCubicMeter;
    public readonly ScalarDerivedUnit KilogramPerSquareMeter;
    public readonly ScalarDerivedUnit Coulomb;
    public readonly ScalarDerivedUnit Volt;
    public readonly ScalarDerivedUnit Farad;
    public readonly ScalarDerivedUnit Ohm;
    public readonly ScalarDerivedUnit Siemen;
    public readonly ScalarDerivedUnit Weber;
    public readonly ScalarDerivedUnit Tesla;
    public readonly ScalarDerivedUnit Henry;
    public readonly ScalarDerivedUnit Lumen;
    public readonly ScalarDerivedUnit Lux;
    public readonly ScalarDerivedUnit CubicMeterPerKilogram;
    public readonly ScalarDerivedUnit AmperePerSquareMeter;
    public readonly ScalarDerivedUnit AmperePerMeter;
    public readonly ScalarDerivedUnit CandelaPerSquareMeter;
    public readonly ScalarDerivedUnit PascalSecond;
    public readonly ScalarDerivedUnit MeterSquaredPerSecond;
    public readonly ScalarDerivedUnit WattPerSquareMeter;
    public readonly ScalarDerivedUnit JoulePerKelvin;
    public readonly ScalarDerivedUnit JoulePerKilogramKelvin;
    public readonly ScalarDerivedUnit JoulePerCubicMeter;
    public readonly ScalarDerivedUnit WattPerMeterKelvin;
    public readonly ScalarDerivedUnit VoltPerMeter;
    public readonly ScalarDerivedUnit CoulombPerCubicMeter;
    public readonly ScalarDerivedUnit CoulombPerSquareMeter;
    public readonly ScalarDerivedUnit FaradPerMeter;
    public readonly ScalarDerivedUnit HenryPerMeter;
    #endregion

    #region Area
    public readonly ScalarDerivedUnit Acre;
    public readonly ScalarDerivedUnit Arpent;
    public readonly ScalarDerivedUnit Barn;
    public readonly ScalarDerivedUnit CircularInch;
    public readonly ScalarDerivedUnit SquareFoot;
    public readonly ScalarDerivedUnit SquareInch;
    public readonly ScalarDerivedUnit SquareMile;
    public readonly ScalarDerivedUnit SquareYard;
    public readonly ScalarDerivedUnit SquareRod;
    public readonly ScalarDerivedUnit SquareMil;
    public readonly ScalarDerivedUnit SquareKiloMeter;
    public readonly ScalarDerivedUnit Kanal;
    public readonly ScalarDerivedUnit Rood;
    public readonly ScalarDerivedUnit Township;
    public readonly ScalarDerivedUnit Yardland;
    #endregion

    #region Volume
    public readonly ScalarDerivedUnit AcreFoot;
    public readonly ScalarDerivedUnit AcreInch;
    public readonly ScalarDerivedUnit AmericanStandardBarrel;
    public readonly ScalarDerivedUnit BarrelOfOil;
    public readonly ScalarDerivedUnit BoardFoot;
    public readonly ScalarDerivedUnit Bushel;
    public readonly ScalarDerivedUnit Butt;
    public readonly ScalarDerivedUnit Cord;
    public readonly ScalarDerivedUnit CubicCentimeter;
    public readonly ScalarDerivedUnit CubicDecimeter;
    public readonly ScalarDerivedUnit Liter;
    public readonly ScalarDerivedUnit CubicFoot;
    public readonly ScalarDerivedUnit CubicInch;
    public readonly ScalarDerivedUnit CubicMile;
    public readonly ScalarDerivedUnit CubicYard;
    public readonly ScalarDerivedUnit Cup;
    public readonly ScalarDerivedUnit Dram;
    public readonly ScalarDerivedUnit FluidOunce;
    public readonly ScalarDerivedUnit Gallon;
    public readonly ScalarDerivedUnit Gill;
    public readonly ScalarDerivedUnit Hogshead;
    public readonly ScalarDerivedUnit Jigger;
    public readonly ScalarDerivedUnit Minim;
    public readonly ScalarDerivedUnit Peck;
    public readonly ScalarDerivedUnit Pint;
    public readonly ScalarDerivedUnit Quart;
    public readonly ScalarDerivedUnit Tablespoon;
    public readonly ScalarDerivedUnit Teaspoon;
    #endregion

    internal ScalarDerivedUnits(DefaultDatabaseWrapper defaultDatabase)
    {
        var sdqs = defaultDatabase.ScalarDerivedQuantities;

        #region Fundamental units
        Hertz = (ScalarDerivedUnit)sdqs.Frequency.FundamentalUnit;
        SquareMeter = (ScalarDerivedUnit)sdqs.Area.FundamentalUnit;
        CubicMeter = (ScalarDerivedUnit)sdqs.Volume.FundamentalUnit;
        CubicMeterPerSecond = (ScalarDerivedUnit)sdqs.VolumeFlowRate.FundamentalUnit;
        MeterPerSecond = (ScalarDerivedUnit)sdqs.Speed.FundamentalUnit;
        MeterPerSecondSquared = (ScalarDerivedUnit)sdqs.Acceleration.FundamentalUnit;
        Newton = (ScalarDerivedUnit)sdqs.Force.FundamentalUnit;
        Pascal = (ScalarDerivedUnit)sdqs.Pressure.FundamentalUnit;
        Joule = (ScalarDerivedUnit)sdqs.Energy.FundamentalUnit;
        Watt = (ScalarDerivedUnit)sdqs.Power.FundamentalUnit;
        RadianPerSecond = (ScalarDerivedUnit)sdqs.AngularSpeed.FundamentalUnit;
        RadianPerSecondSquared = (ScalarDerivedUnit)sdqs.AngularAcceleration.FundamentalUnit;
        InverseMeter = (ScalarDerivedUnit)sdqs.WaveNumber.FundamentalUnit;
        KilogramPerCubicMeter = (ScalarDerivedUnit)sdqs.Density.FundamentalUnit;
        KilogramPerSquareMeter = (ScalarDerivedUnit)sdqs.SurfaceDensity.FundamentalUnit;
        Coulomb = (ScalarDerivedUnit)sdqs.ElectricCharge.FundamentalUnit;
        Volt = (ScalarDerivedUnit)sdqs.Voltage.FundamentalUnit;
        Farad = (ScalarDerivedUnit)sdqs.ElectricCapacitance.FundamentalUnit;
        Ohm = (ScalarDerivedUnit)sdqs.ElectricResistance.FundamentalUnit;
        Siemen = (ScalarDerivedUnit)sdqs.ElectricConductance.FundamentalUnit;
        Weber = (ScalarDerivedUnit)sdqs.MagneticFlux.FundamentalUnit;
        Tesla = (ScalarDerivedUnit)sdqs.MagneticFluxDensity.FundamentalUnit;
        Henry = (ScalarDerivedUnit)sdqs.Inductance.FundamentalUnit;
        Lumen = (ScalarDerivedUnit)sdqs.LuminousFlux.FundamentalUnit;
        Lux = (ScalarDerivedUnit)sdqs.Illuminance.FundamentalUnit;
        CubicMeterPerKilogram = (ScalarDerivedUnit)sdqs.SpecificVolume.FundamentalUnit;
        AmperePerSquareMeter = (ScalarDerivedUnit)sdqs.ElectricCurrentDensity.FundamentalUnit;
        AmperePerMeter = (ScalarDerivedUnit)sdqs.MagneticFieldStrength.FundamentalUnit;
        CandelaPerSquareMeter = (ScalarDerivedUnit)sdqs.Luminance.FundamentalUnit;
        PascalSecond = (ScalarDerivedUnit)sdqs.DynamicViscosity.FundamentalUnit;
        MeterSquaredPerSecond = (ScalarDerivedUnit)sdqs.KinematicViscosity.FundamentalUnit;
        WattPerSquareMeter = (ScalarDerivedUnit)sdqs.PowerFlux.FundamentalUnit;
        JoulePerKelvin = (ScalarDerivedUnit)sdqs.HeatCapacity.FundamentalUnit;
        JoulePerKilogramKelvin = (ScalarDerivedUnit)sdqs.SpecificHeatCapacity.FundamentalUnit;
        JoulePerCubicMeter = (ScalarDerivedUnit)sdqs.SpecificEnergy.FundamentalUnit;
        WattPerMeterKelvin = (ScalarDerivedUnit)sdqs.ThermalConductivity.FundamentalUnit;
        VoltPerMeter = (ScalarDerivedUnit)sdqs.ElectricFieldStrength.FundamentalUnit;
        CoulombPerCubicMeter = (ScalarDerivedUnit)sdqs.ElectricChargeDensity.FundamentalUnit;
        CoulombPerSquareMeter = (ScalarDerivedUnit)sdqs.SurfaceChargeDensity.FundamentalUnit;
        FaradPerMeter = (ScalarDerivedUnit)sdqs.Permittivity.FundamentalUnit;
        HenryPerMeter = (ScalarDerivedUnit)sdqs.Permeability.FundamentalUnit;
        #endregion

        #region Area
        Acre = 
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

        //    #region Volume
        //    Liter = new(nameof(Liter), CubicMeter, 1e-3m);

        //    AcreFoot = Make(nameof(AcreFoot), Acre.UC * Foot.UC);
        //    AcreInch = Make(nameof(AcreInch), Acre.UC * Inch.UC);
        //    AmericanStandardBarrel = new(nameof(AmericanStandardBarrel), Liter, 200m);
        //    BoardFoot = Make(nameof(BoardFoot), Foot.UC * Foot.UC * Inch.UC);
        //    //Bushel = new(nameof(Bushel), Gallon, 8m);
        //    //Butt = new(nameof(Butt), Hogshead, 2m);
        //    //Cup = new(nameof(Cup), FluidOunce, 8m);
        //    //Cord = new(nameof(Cord), CubicFoot, 128m);
        //    CubicCentimeter = Make(nameof(CubicCentimeter), Centi[Meter], 3m);
        //    CubicDecimeter = Make(nameof(CubicDecimeter), Deci[Meter], 3m);
        //    //BarrelOfOil = new(nameof(BarrelOfOil), Gallon, 42m);
        //    #endregion

        //    return;
        //    foreach (var field in typeof(DerivedUnits).GetFields())
        //    {
        //        var unit = (ScalarDerivedUnit)sdqs.field.GetValue(field.Name)!;
        //        if (unit.Name is not null)
        //            continue;
        //        unit.ChangeName(field.Name);
        //    }
    }



    //private ScalarDerivedUnit Make(string name, NamedComposition<IUnit> composition)
    //{
    //    return (ScalarDerivedUnit)sdqs.DefineFromComposition(name, composition);
    //}

    //private ScalarDerivedUnit Make(string name, IUnit unit, decimal power)
    //{
    //    return (ScalarDerivedUnit)sdqs.DefineFromComposition(name, unit.UnitComposition.Pow(power));
    //}
}

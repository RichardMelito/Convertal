// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using DecimalMath;

namespace Convertal.Core.Defaults;

public class ScalarDerivedUnits
{
    #region Fundamental units
    public readonly DerivedUnit Hertz = (DerivedUnit)Frequency.FundamentalUnit;
    public readonly DerivedUnit SquareMeter = (DerivedUnit)Area.FundamentalUnit;
    public readonly DerivedUnit CubicMeter = (DerivedUnit)Volume.FundamentalUnit;
    public readonly DerivedUnit CubicMeterPerSecond = (DerivedUnit)VolumeFlowRate.FundamentalUnit;
    public readonly DerivedUnit MeterPerSecond = (DerivedUnit)Speed.FundamentalUnit;
    public readonly DerivedUnit MeterPerSecondSquared = (DerivedUnit)Acceleration.FundamentalUnit;
    public readonly DerivedUnit Newton = (DerivedUnit)Force.FundamentalUnit;
    public readonly DerivedUnit Pascal = (DerivedUnit)Pressure.FundamentalUnit;
    public readonly DerivedUnit Joule = (DerivedUnit)EnergyAndTorque.FundamentalUnit;
    public readonly DerivedUnit Watt = (DerivedUnit)Power.FundamentalUnit;
    public readonly DerivedUnit RadianPerSecond = (DerivedUnit)AngularVelocity.FundamentalUnit;
    public readonly DerivedUnit RadianPerSecondSquared = (DerivedUnit)AngularAcceleration.FundamentalUnit;
    public readonly DerivedUnit InverseMeter = (DerivedUnit)WaveNumber.FundamentalUnit;
    public readonly DerivedUnit KilogramPerCubicMeter = (DerivedUnit)Density.FundamentalUnit;
    public readonly DerivedUnit KilogramPerSquareMeter = (DerivedUnit)SurfaceDensity.FundamentalUnit;
    public readonly DerivedUnit Coulomb = (DerivedUnit)ElectricCharge.FundamentalUnit;
    public readonly DerivedUnit Volt = (DerivedUnit)Voltage.FundamentalUnit;
    public readonly DerivedUnit Farad = (DerivedUnit)ElectricCapacitance.FundamentalUnit;
    public readonly DerivedUnit Ohm = (DerivedUnit)ElectricResistance.FundamentalUnit;
    public readonly DerivedUnit Siemen = (DerivedUnit)ElectricConductance.FundamentalUnit;
    public readonly DerivedUnit Weber = (DerivedUnit)MagneticFlux.FundamentalUnit;
    public readonly DerivedUnit Tesla = (DerivedUnit)MagneticFluxDensity.FundamentalUnit;
    public readonly DerivedUnit Henry = (DerivedUnit)Inductance.FundamentalUnit;
    public readonly DerivedUnit Lumen = (DerivedUnit)LuminousFlux.FundamentalUnit;
    public readonly DerivedUnit Lux = (DerivedUnit)Illuminance.FundamentalUnit;
    public readonly DerivedUnit CubicMeterPerKilogram = (DerivedUnit)SpecificVolume.FundamentalUnit;
    public readonly DerivedUnit AmperePerSquareMeter = (DerivedUnit)ElectricCurrentDensity.FundamentalUnit;
    public readonly DerivedUnit AmperePerMeter = (DerivedUnit)MagneticFieldStrength.FundamentalUnit;
    public readonly DerivedUnit CandelaPerSquareMeter = (DerivedUnit)Luminance.FundamentalUnit;
    public readonly DerivedUnit PascalSecond = (DerivedUnit)DynamicViscosity.FundamentalUnit;
    public readonly DerivedUnit MeterSquaredPerSecond = (DerivedUnit)KinematicViscosity.FundamentalUnit;
    public readonly DerivedUnit WattPerSquareMeter = (DerivedUnit)PowerFlux.FundamentalUnit;
    public readonly DerivedUnit JoulePerKelvin = (DerivedUnit)HeatCapacity.FundamentalUnit;
    public readonly DerivedUnit JoulePerKilogramKelvin = (DerivedUnit)SpecificHeatCapacity.FundamentalUnit;
    public readonly DerivedUnit JoulePerCubicMeter = (DerivedUnit)SpecificEnergy.FundamentalUnit;
    public readonly DerivedUnit WattPerMeterKelvin = (DerivedUnit)ThermalConductivity.FundamentalUnit;
    public readonly DerivedUnit VoltPerMeter = (DerivedUnit)ElectricFieldStrength.FundamentalUnit;
    public readonly DerivedUnit CoulombPerCubicMeter = (DerivedUnit)ElectricChargeDensity.FundamentalUnit;
    public readonly DerivedUnit CoulombPerSquareMeter = (DerivedUnit)SurfaceChargeDensity.FundamentalUnit;
    public readonly DerivedUnit FaradPerMeter = (DerivedUnit)Permittivity.FundamentalUnit;
    public readonly DerivedUnit HenryPerMeter = (DerivedUnit)Permeability.FundamentalUnit;
    #endregion

    #region Area
    public readonly DerivedUnit Are;
    public readonly DerivedUnit Acre;
    public readonly DerivedUnit Arpent;
    public readonly DerivedUnit Barn;
    public readonly DerivedUnit CircularInch;
    public readonly DerivedUnit SquareFoot;
    public readonly DerivedUnit SquareInch;
    public readonly DerivedUnit SquareMile;
    public readonly DerivedUnit SquareYard;
    public readonly DerivedUnit SquareRod;
    public readonly DerivedUnit SquareMil;
    public readonly DerivedUnit SquareKiloMeter;
    public readonly DerivedUnit Kanal;
    public readonly DerivedUnit Rood;
    public readonly DerivedUnit Township;
    public readonly DerivedUnit Yardland;
    #endregion

    #region Volume
    public readonly DerivedUnit AcreFoot;
    public readonly DerivedUnit AcreInch;
    public readonly DerivedUnit AmericanStandardBarrel;
    public readonly DerivedUnit BarrelOfOil;
    public readonly DerivedUnit BoardFoot;
    public readonly DerivedUnit Bushel;
    public readonly DerivedUnit Butt;
    public readonly DerivedUnit Cord;
    public readonly DerivedUnit CubicCentimeter;
    public readonly DerivedUnit CubicDecimeter;
    public readonly DerivedUnit Liter;
    public readonly DerivedUnit CubicFoot;
    public readonly DerivedUnit CubicInch;
    public readonly DerivedUnit CubicMile;
    public readonly DerivedUnit CubicYard;
    public readonly DerivedUnit Cup;
    public readonly DerivedUnit Dram;
    public readonly DerivedUnit FluidOunce;
    public readonly DerivedUnit Gallon;
    public readonly DerivedUnit Gill;
    public readonly DerivedUnit Hogshead;
    public readonly DerivedUnit Jigger;
    public readonly DerivedUnit Minim;
    public readonly DerivedUnit Peck;
    public readonly DerivedUnit Pint;
    public readonly DerivedUnit Quart;
    public readonly DerivedUnit Tablespoon;
    public readonly DerivedUnit Teaspoon;
    #endregion

    ScalarDerivedUnits()
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



    private DerivedUnit Make(string name, NamedComposition<IUnit> composition)
    {
        return (DerivedUnit)DefineFromComposition(name, composition);
    }

    private DerivedUnit Make(string name, IUnit unit, decimal power)
    {
        return (DerivedUnit)DefineFromComposition(name, unit.UnitComposition.Pow(power));
    }
}
public class VectorDerivedUnits
{
    #region Fundamental units
    public readonly DerivedUnit Hertz = (DerivedUnit)Frequency.FundamentalUnit;
    public readonly DerivedUnit SquareMeter = (DerivedUnit)Area.FundamentalUnit;
    public readonly DerivedUnit CubicMeter = (DerivedUnit)Volume.FundamentalUnit;
    public readonly DerivedUnit CubicMeterPerSecond = (DerivedUnit)VolumeFlowRate.FundamentalUnit;
    public readonly DerivedUnit MeterPerSecond = (DerivedUnit)Speed.FundamentalUnit;
    public readonly DerivedUnit MeterPerSecondSquared = (DerivedUnit)Acceleration.FundamentalUnit;
    public readonly DerivedUnit Newton = (DerivedUnit)Force.FundamentalUnit;
    public readonly DerivedUnit Pascal = (DerivedUnit)Pressure.FundamentalUnit;
    public readonly DerivedUnit Joule = (DerivedUnit)EnergyAndTorque.FundamentalUnit;
    public readonly DerivedUnit Watt = (DerivedUnit)Power.FundamentalUnit;
    public readonly DerivedUnit RadianPerSecond = (DerivedUnit)AngularVelocity.FundamentalUnit;
    public readonly DerivedUnit RadianPerSecondSquared = (DerivedUnit)AngularAcceleration.FundamentalUnit;
    public readonly DerivedUnit InverseMeter = (DerivedUnit)WaveNumber.FundamentalUnit;
    public readonly DerivedUnit KilogramPerCubicMeter = (DerivedUnit)Density.FundamentalUnit;
    public readonly DerivedUnit KilogramPerSquareMeter = (DerivedUnit)SurfaceDensity.FundamentalUnit;
    public readonly DerivedUnit Coulomb = (DerivedUnit)ElectricCharge.FundamentalUnit;
    public readonly DerivedUnit Volt = (DerivedUnit)Voltage.FundamentalUnit;
    public readonly DerivedUnit Farad = (DerivedUnit)ElectricCapacitance.FundamentalUnit;
    public readonly DerivedUnit Ohm = (DerivedUnit)ElectricResistance.FundamentalUnit;
    public readonly DerivedUnit Siemen = (DerivedUnit)ElectricConductance.FundamentalUnit;
    public readonly DerivedUnit Weber = (DerivedUnit)MagneticFlux.FundamentalUnit;
    public readonly DerivedUnit Tesla = (DerivedUnit)MagneticFluxDensity.FundamentalUnit;
    public readonly DerivedUnit Henry = (DerivedUnit)Inductance.FundamentalUnit;
    public readonly DerivedUnit Lumen = (DerivedUnit)LuminousFlux.FundamentalUnit;
    public readonly DerivedUnit Lux = (DerivedUnit)Illuminance.FundamentalUnit;
    public readonly DerivedUnit CubicMeterPerKilogram = (DerivedUnit)SpecificVolume.FundamentalUnit;
    public readonly DerivedUnit AmperePerSquareMeter = (DerivedUnit)ElectricCurrentDensity.FundamentalUnit;
    public readonly DerivedUnit AmperePerMeter = (DerivedUnit)MagneticFieldStrength.FundamentalUnit;
    public readonly DerivedUnit CandelaPerSquareMeter = (DerivedUnit)Luminance.FundamentalUnit;
    public readonly DerivedUnit PascalSecond = (DerivedUnit)DynamicViscosity.FundamentalUnit;
    public readonly DerivedUnit MeterSquaredPerSecond = (DerivedUnit)KinematicViscosity.FundamentalUnit;
    public readonly DerivedUnit WattPerSquareMeter = (DerivedUnit)PowerFlux.FundamentalUnit;
    public readonly DerivedUnit JoulePerKelvin = (DerivedUnit)HeatCapacity.FundamentalUnit;
    public readonly DerivedUnit JoulePerKilogramKelvin = (DerivedUnit)SpecificHeatCapacity.FundamentalUnit;
    public readonly DerivedUnit JoulePerCubicMeter = (DerivedUnit)SpecificEnergy.FundamentalUnit;
    public readonly DerivedUnit WattPerMeterKelvin = (DerivedUnit)ThermalConductivity.FundamentalUnit;
    public readonly DerivedUnit VoltPerMeter = (DerivedUnit)ElectricFieldStrength.FundamentalUnit;
    public readonly DerivedUnit CoulombPerCubicMeter = (DerivedUnit)ElectricChargeDensity.FundamentalUnit;
    public readonly DerivedUnit CoulombPerSquareMeter = (DerivedUnit)SurfaceChargeDensity.FundamentalUnit;
    public readonly DerivedUnit FaradPerMeter = (DerivedUnit)Permittivity.FundamentalUnit;
    public readonly DerivedUnit HenryPerMeter = (DerivedUnit)Permeability.FundamentalUnit;
    #endregion

    #region Area
    public readonly DerivedUnit Are;
    public readonly DerivedUnit Acre;
    public readonly DerivedUnit Arpent;
    public readonly DerivedUnit Barn;
    public readonly DerivedUnit CircularInch;
    public readonly DerivedUnit SquareFoot;
    public readonly DerivedUnit SquareInch;
    public readonly DerivedUnit SquareMile;
    public readonly DerivedUnit SquareYard;
    public readonly DerivedUnit SquareRod;
    public readonly DerivedUnit SquareMil;
    public readonly DerivedUnit SquareKiloMeter;
    public readonly DerivedUnit Kanal;
    public readonly DerivedUnit Rood;
    public readonly DerivedUnit Township;
    public readonly DerivedUnit Yardland;
    #endregion

    #region Volume
    public readonly DerivedUnit AcreFoot;
    public readonly DerivedUnit AcreInch;
    public readonly DerivedUnit AmericanStandardBarrel;
    public readonly DerivedUnit BarrelOfOil;
    public readonly DerivedUnit BoardFoot;
    public readonly DerivedUnit Bushel;
    public readonly DerivedUnit Butt;
    public readonly DerivedUnit Cord;
    public readonly DerivedUnit CubicCentimeter;
    public readonly DerivedUnit CubicDecimeter;
    public readonly DerivedUnit Liter;
    public readonly DerivedUnit CubicFoot;
    public readonly DerivedUnit CubicInch;
    public readonly DerivedUnit CubicMile;
    public readonly DerivedUnit CubicYard;
    public readonly DerivedUnit Cup;
    public readonly DerivedUnit Dram;
    public readonly DerivedUnit FluidOunce;
    public readonly DerivedUnit Gallon;
    public readonly DerivedUnit Gill;
    public readonly DerivedUnit Hogshead;
    public readonly DerivedUnit Jigger;
    public readonly DerivedUnit Minim;
    public readonly DerivedUnit Peck;
    public readonly DerivedUnit Pint;
    public readonly DerivedUnit Quart;
    public readonly DerivedUnit Tablespoon;
    public readonly DerivedUnit Teaspoon;
    #endregion

    VectorDerivedUnits()
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



    private DerivedUnit Make(string name, NamedComposition<IUnit> composition)
    {
        return (DerivedUnit)DefineFromComposition(name, composition);
    }

    private DerivedUnit Make(string name, IUnit unit, decimal power)
    {
        return (DerivedUnit)DefineFromComposition(name, unit.UnitComposition.Pow(power));
    }
}

public class DefaultPrefixes
{
    public readonly Prefix Yotta;
    public readonly Prefix Zetta;
    public readonly Prefix Exa;
    public readonly Prefix Peta;
    public readonly Prefix Tera;
    public readonly Prefix Giga;
    public readonly Prefix Mega;
    public readonly Prefix Kilo;
    public readonly Prefix Hecto;
    public readonly Prefix Deca;

    public readonly Prefix Deci;
    public readonly Prefix Centi;
    public readonly Prefix Milli;
    public readonly Prefix Micro;
    public readonly Prefix Nano;
    public readonly Prefix Pico;
    public readonly Prefix Femto;
    public readonly Prefix Atto;
    public readonly Prefix Zepto;
    public readonly Prefix Yocto;

    public readonly Prefix Kibi;
    public readonly Prefix Mebi;
    public readonly Prefix Gibi;
    public readonly Prefix Tebi;
    public readonly Prefix Pebi;
    public readonly Prefix Exbi;
    public readonly Prefix Zebi;
    public readonly Prefix Yobi;

    internal DefaultPrefixes(DefaultDatabaseWrapper defaultDatabase)
    {
        Yotta = defaultDatabase.Database.GetOrDefinePrefix("yotta", 1e24m, "Y");
        Zetta = defaultDatabase.Database.GetOrDefinePrefix("zetta", 1e21m, "Z");
        Exa = defaultDatabase.Database.GetOrDefinePrefix("exa", 1e18m, "E");
        Peta = defaultDatabase.Database.GetOrDefinePrefix("peta", 1e15m, "P");
        Tera = defaultDatabase.Database.GetOrDefinePrefix("tera", 1e12m, "T");
        Giga = defaultDatabase.Database.GetOrDefinePrefix("giga", 1e9m, "G");
        Mega = defaultDatabase.Database.GetOrDefinePrefix("mega", 1e6m, "M");
        Kilo = defaultDatabase.Database.GetOrDefinePrefix("kilo", 1e3m, "k");
        Hecto = defaultDatabase.Database.GetOrDefinePrefix("hecto", 1e2m, "h");
        Deca = defaultDatabase.Database.GetOrDefinePrefix("deca", 1e1m, "da");

        Deci = defaultDatabase.Database.GetOrDefinePrefix("deci", 1e-1m, "d");
        Centi = defaultDatabase.Database.GetOrDefinePrefix("centi", 1e-2m, "c");
        Milli = defaultDatabase.Database.GetOrDefinePrefix("milli", 1e-3m, "m");
        Micro = defaultDatabase.Database.GetOrDefinePrefix("micro", 1e-6m, "μ");
        Nano = defaultDatabase.Database.GetOrDefinePrefix("nano", 1e-9m, "n");
        Pico = defaultDatabase.Database.GetOrDefinePrefix("pico", 1e-12m, "p");
        Femto = defaultDatabase.Database.GetOrDefinePrefix("femto", 1e-15m, "f");
        Atto = defaultDatabase.Database.GetOrDefinePrefix("atto", 1e-18m, "a");
        Zepto = defaultDatabase.Database.GetOrDefinePrefix("zepto", 1e-21m, "z");
        Yocto = defaultDatabase.Database.GetOrDefinePrefix("yocto", 1e-24m, "y");

        Kibi = defaultDatabase.Database.GetOrDefinePrefix("kibi", DecimalEx.Pow(1024m, 1m), "Ki");
        Mebi = defaultDatabase.Database.GetOrDefinePrefix("mebi", DecimalEx.Pow(1024m, 2m), "Mi");
        Gibi = defaultDatabase.Database.GetOrDefinePrefix("gibi", DecimalEx.Pow(1024m, 3m), "Gi");
        Tebi = defaultDatabase.Database.GetOrDefinePrefix("tebi", DecimalEx.Pow(1024m, 4m), "Ti");
        Pebi = defaultDatabase.Database.GetOrDefinePrefix("pebi", DecimalEx.Pow(1024m, 5m), "Pi");
        Exbi = defaultDatabase.Database.GetOrDefinePrefix("exbi", DecimalEx.Pow(1024m, 6m), "Ei");
        Zebi = defaultDatabase.Database.GetOrDefinePrefix("zebi", DecimalEx.Pow(1024m, 7m), "Zi");
        Yobi = defaultDatabase.Database.GetOrDefinePrefix("yobi", DecimalEx.Pow(1024m, 8m), "Yi");
    }
}

public class MeasurementSystems
{
    // TODO
}

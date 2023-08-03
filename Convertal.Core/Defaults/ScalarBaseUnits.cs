// Created by Richard Melito and licensed to you under The Clear BSD License.

using static DecimalMath.DecimalEx;

namespace Convertal.Core.Defaults;

public class ScalarBaseUnits
{
    #region Fundamental units
    public readonly ScalarPrefixedBaseUnit Kilogram;
    public readonly ScalarBaseUnit Radian;
    public readonly ScalarBaseUnit Steradian;
    public readonly ScalarBaseUnit Ampere;
    public readonly ScalarBaseUnit Bit;
    public readonly ScalarBaseUnit Meter;
    public readonly ScalarBaseUnit Gram;
    public readonly ScalarBaseUnit Kelvin;
    public readonly ScalarBaseUnit Second;
    public readonly ScalarBaseUnit Candela;
    #endregion

    #region Angle
    public readonly ScalarBaseUnit Degree;
    public readonly ScalarBaseUnit Gradian;
    public readonly ScalarBaseUnit ArcMinute;
    public readonly ScalarBaseUnit ArcSecond;
    public readonly ScalarBaseUnit Revolution;
    public readonly ScalarBaseUnit Quadrant;
    public readonly ScalarBaseUnit Sign;
    #endregion

    #region SolidAngle
    public readonly ScalarBaseUnit Hemisphere;
    public readonly ScalarBaseUnit SquareDegree;
    public readonly ScalarBaseUnit Sphere;
    #endregion

    #region ElectricCurrent
    public readonly ScalarBaseUnit Abampere;
    public readonly ScalarBaseUnit Biot;
    #endregion

    #region Information
    public readonly ScalarBaseUnit Byte;
    public readonly ScalarBaseUnit Nibble;
    public readonly ScalarBaseUnit Crumb;
    public readonly ScalarBaseUnit Word;
    #endregion

    #region Length
    public readonly ScalarBaseUnit Inch;
    public readonly ScalarBaseUnit Foot;
    public readonly ScalarBaseUnit Yard;
    public readonly ScalarBaseUnit Mile;
    public readonly ScalarBaseUnit NauticalMile;
    public readonly ScalarBaseUnit Cable;
    public readonly ScalarBaseUnit CableUsSurvey;
    public readonly ScalarBaseUnit Caliber;
    public readonly ScalarBaseUnit ChainGunters;
    public readonly ScalarBaseUnit ChainEngineers;
    public readonly ScalarBaseUnit Cubit;
    public readonly ScalarBaseUnit FootUsSurvey;
    public readonly ScalarBaseUnit Fathom;
    public readonly ScalarBaseUnit Furlong;
    public readonly ScalarBaseUnit Hand;
    public readonly ScalarBaseUnit LandLeague;
    public readonly ScalarBaseUnit Pica;
    public readonly ScalarBaseUnit Point;
    public readonly ScalarBaseUnit Rod;
    public readonly ScalarBaseUnit Angstrom;
    public readonly ScalarBaseUnit AstronomicalUnit;
    public readonly ScalarBaseUnit Parsec;
    public readonly ScalarBaseUnit LightYear;
    public readonly ScalarBaseUnit Mil;
    #endregion

    #region Mass
    public readonly ScalarBaseUnit Carat;
    public readonly ScalarBaseUnit PoundMass;
    public readonly ScalarBaseUnit Ounce;
    public readonly ScalarBaseUnit Dram;
    public readonly ScalarBaseUnit Stone;
    public readonly ScalarBaseUnit Slug;
    public readonly ScalarBaseUnit ShortTon;
    public readonly ScalarBaseUnit LongTon;
    public readonly ScalarBaseUnit Tonne;
    public readonly ScalarBaseUnit AtomicMass;
    public readonly ScalarBaseUnit Scruple;
    #endregion

    #region Temperature
    public readonly ScalarBaseUnit Celsius;
    public readonly ScalarBaseUnit Rankine;
    public readonly ScalarBaseUnit Fahrenheit;
    #endregion

    #region Time
    public readonly ScalarBaseUnit Minute;
    public readonly ScalarBaseUnit Hour;
    public readonly ScalarBaseUnit Day;
    public readonly ScalarBaseUnit Week;
    public readonly ScalarBaseUnit Year;
    #endregion

    #region LuminousIntensity
    public readonly ScalarBaseUnit CandlePower;
    #endregion

    internal ScalarBaseUnits(DefaultDatabaseWrapper defaultDatabase)
    {
        var sbqs = defaultDatabase.ScalarBaseQuantities;
        var db = defaultDatabase.Database;

        #region Fundamental units
        Kilogram = (ScalarPrefixedBaseUnit)sbqs.Mass.FundamentalUnit;
        Radian = (ScalarBaseUnit)sbqs.Angle.FundamentalUnit;
        Steradian = (ScalarBaseUnit)sbqs.SolidAngle.FundamentalUnit;
        Ampere = (ScalarBaseUnit)sbqs.ElectricCurrent.FundamentalUnit;
        Bit = (ScalarBaseUnit)sbqs.Information.FundamentalUnit;
        Meter = (ScalarBaseUnit)sbqs.Length.FundamentalUnit;
        Gram = Kilogram.Unit;
        Kelvin = (ScalarBaseUnit)sbqs.Temperature.FundamentalUnit;
        Second = (ScalarBaseUnit)sbqs.Time.FundamentalUnit;
        Candela = (ScalarBaseUnit)sbqs.LuminousIntensity.FundamentalUnit;
        #endregion

        #region Angle
        Degree = db.GetOrDefineScalarBaseUnit(nameof(Degree), Radian, Pi / 180m, "°");
        Gradian = db.GetOrDefineScalarBaseUnit(nameof(Gradian), Radian, Pi / 200m, "grad");
        ArcMinute = db.GetOrDefineScalarBaseUnit(nameof(ArcMinute), Degree, 1m / 60m, "arcmin");
        ArcSecond = db.GetOrDefineScalarBaseUnit(nameof(ArcSecond), ArcMinute, 1m / 60m, "arcs");
        Revolution = db.GetOrDefineScalarBaseUnit(nameof(Revolution), Radian, TwoPi, "rev");
        Quadrant = db.GetOrDefineScalarBaseUnit(nameof(Quadrant), Radian, PiHalf);
        Sign = db.GetOrDefineScalarBaseUnit(nameof(Sign), Degree, 30m);
        #endregion

        #region SolidAngle
        Hemisphere = db.GetOrDefineScalarBaseUnit(nameof(Hemisphere), Steradian, TwoPi);
        SquareDegree = db.GetOrDefineScalarBaseUnit(nameof(SquareDegree), Steradian, Pow(Pi / 180m, 2m));
        Sphere = db.GetOrDefineScalarBaseUnit(nameof(Sphere), Hemisphere, 2m);
        #endregion

        #region ElectricCurrent
        Abampere = db.GetOrDefineScalarBaseUnit(nameof(Abampere), Ampere, 10m, "abA");
        Biot = db.GetOrDefineScalarBaseUnit(nameof(Biot), Abampere, 1m);
        #endregion

        #region Information
        Byte = db.GetOrDefineScalarBaseUnit(nameof(Byte), Bit, 8m);
        Nibble = db.GetOrDefineScalarBaseUnit(nameof(Nibble), Bit, 4m);
        Crumb = db.GetOrDefineScalarBaseUnit(nameof(Crumb), Bit, 2m);
        Word = db.GetOrDefineScalarBaseUnit(nameof(Word), Bit, 16m);
        #endregion

        #region Length
        Inch = db.GetOrDefineScalarBaseUnit(nameof(Inch), Meter, 0.0254m, "in");
        Foot = db.GetOrDefineScalarBaseUnit(nameof(Foot), Inch, 12m, "ft");
        Yard = db.GetOrDefineScalarBaseUnit(nameof(Yard), Foot, 3m, "yd");
        Mile = db.GetOrDefineScalarBaseUnit(nameof(Mile), Foot, 5280m, "mi");
        NauticalMile = db.GetOrDefineScalarBaseUnit(nameof(NauticalMile), Meter, 1852m, "nmi");
        Cable = db.GetOrDefineScalarBaseUnit(nameof(Cable), NauticalMile, 0.1m);
        CableUsSurvey = db.GetOrDefineScalarBaseUnit(nameof(CableUsSurvey), Yard, 240m);
        Caliber = db.GetOrDefineScalarBaseUnit(nameof(Caliber), Inch, 0.01m);
        ChainGunters = db.GetOrDefineScalarBaseUnit(nameof(ChainGunters), Yard, 22m);
        ChainEngineers = db.GetOrDefineScalarBaseUnit(nameof(ChainEngineers), Foot, 100m);
        Cubit = db.GetOrDefineScalarBaseUnit(nameof(Cubit), Foot, 1.5m);
        FootUsSurvey = db.GetOrDefineScalarBaseUnit(nameof(FootUsSurvey), Foot, 1.000002m);
        Fathom = db.GetOrDefineScalarBaseUnit(nameof(Fathom), Yard, 2m);
        Furlong = db.GetOrDefineScalarBaseUnit(nameof(Furlong), Foot, 660m, "fur");
        Hand = db.GetOrDefineScalarBaseUnit(nameof(Hand), Inch, 4m, "hh");
        LandLeague = db.GetOrDefineScalarBaseUnit(nameof(LandLeague), Mile, 3m);
        Pica = db.GetOrDefineScalarBaseUnit(nameof(Pica), Inch, 0.166m);
        Point = db.GetOrDefineScalarBaseUnit(nameof(Point), Pica, 1m / 12m);
        Rod = db.GetOrDefineScalarBaseUnit(nameof(Rod), Yard, 5.5m);
        Angstrom = db.GetOrDefineScalarBaseUnit(nameof(Angstrom), Meter, 1e-10m, "Å");
        AstronomicalUnit = db.GetOrDefineScalarBaseUnit(nameof(AstronomicalUnit), Meter, 1.495978e11m, "AU");
        Parsec = db.GetOrDefineScalarBaseUnit(nameof(Parsec), AstronomicalUnit, 206265m, "pc");
        LightYear = db.GetOrDefineScalarBaseUnit(nameof(LightYear), Parsec, 0.306601m, "ly");
        Mil = db.GetOrDefineScalarBaseUnit(nameof(Mil), Inch, 0.001m, "mil");
        #endregion

        #region Mass
        Carat = db.GetOrDefineScalarBaseUnit(nameof(Carat), Gram, 0.2m, "ct");
        PoundMass = db.GetOrDefineScalarBaseUnit(nameof(PoundMass), Kilogram, 0.4536m, "lbm");
        Ounce = db.GetOrDefineScalarBaseUnit(nameof(Ounce), PoundMass, 1m / 16m, "oz");
        Dram = db.GetOrDefineScalarBaseUnit(nameof(Dram), Ounce, 1m / 16m);
        Stone = db.GetOrDefineScalarBaseUnit(nameof(Stone), PoundMass, 14m, "st");
        Slug = db.GetOrDefineScalarBaseUnit(nameof(Slug), PoundMass, 32.174m);
        ShortTon = db.GetOrDefineScalarBaseUnit(nameof(ShortTon), PoundMass, 2000m, "tn");
        LongTon = db.GetOrDefineScalarBaseUnit(nameof(LongTon), PoundMass, 2240m, "LT");
        Tonne = db.GetOrDefineScalarBaseUnit(nameof(Tonne), Kilogram, 1000m, "t");
        AtomicMass = db.GetOrDefineScalarBaseUnit(nameof(AtomicMass), Kilogram, 1.66054e-27m, "Da");
        Scruple = db.GetOrDefineScalarBaseUnit(nameof(Scruple), Ounce, 1m / 24m);
        #endregion
        #region Temperature
        Celsius = db.GetOrDefineScalarBaseUnit(nameof(Celsius), Kelvin, 1m, "°C", 273.15m);
        Rankine = db.GetOrDefineScalarBaseUnit(nameof(Rankine), Kelvin, 4m / 9m, "°R");
        Fahrenheit = db.GetOrDefineScalarBaseUnit(nameof(Fahrenheit), Rankine, 1m, "°F", 459.67m);
        #endregion
        #region Time
        Minute = db.GetOrDefineScalarBaseUnit(nameof(Minute), Second, 60m, "min");
        Hour = db.GetOrDefineScalarBaseUnit(nameof(Hour), Minute, 60m, "h");
        Day = db.GetOrDefineScalarBaseUnit(nameof(Day), Hour, 24m, "d");
        Week = db.GetOrDefineScalarBaseUnit(nameof(Week), Day, 7m);
        Year = db.GetOrDefineScalarBaseUnit(nameof(Year), Day, 365.25m, "y");
        #endregion
        #region LuminousIntensity
        CandlePower = db.GetOrDefineScalarBaseUnit(nameof(CandlePower), Candela, 0.981m, "cp");
        #endregion
    }
}

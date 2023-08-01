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
    public readonly ScalarBaseUnit Carat = new(nameof(Carat), Gram, 0.2m);
    public readonly ScalarBaseUnit PoundMass = new(nameof(PoundMass), Kilogram, 0.4536m);
    public readonly ScalarBaseUnit Ounce = new(nameof(Ounce), PoundMass, 1m / 16m);
    public readonly ScalarBaseUnit Dram = new(nameof(Dram), Ounce, 1m / 16m);
    public readonly ScalarBaseUnit Stone = new(nameof(Stone), PoundMass, 14m);
    public readonly ScalarBaseUnit Slug = new(nameof(Slug), PoundMass, 32.174m);
    public readonly ScalarBaseUnit ShortTon = new(nameof(ShortTon), PoundMass, 2000m);
    public readonly ScalarBaseUnit LongTon = new(nameof(LongTon), PoundMass, 2240m);
    public readonly ScalarBaseUnit Tonne = new(nameof(Tonne), Kilogram, 1000m);
    public readonly ScalarBaseUnit AtomicMass = new(nameof(AtomicMass), Kilogram, 1.66054e-27m);
    public readonly ScalarBaseUnit Scruple = new(nameof(Scruple), Ounce, 1m / 24m);
    #endregion

    #region Temperature
    public readonly ScalarBaseUnit Celsius = new(nameof(Celsius), Kelvin, 1m, 273.15m);
    public readonly ScalarBaseUnit Rankine = new(nameof(Rankine), Kelvin, 4m / 9m);
    public readonly ScalarBaseUnit Fahrenheit = new(nameof(Fahrenheit), Rankine, 1m, 459.67m);
    #endregion

    #region Time
    public readonly ScalarBaseUnit Minute = new(nameof(Minute), Second, 60m);
    public readonly ScalarBaseUnit Hour = new(nameof(Hour), Minute, 60m);
    public readonly ScalarBaseUnit Day = new(nameof(Day), Hour, 24m);
    public readonly ScalarBaseUnit Week = new(nameof(Week), Day, 7m);
    public readonly ScalarBaseUnit Month = new(nameof(Month), Minute, 4330m);
    public readonly ScalarBaseUnit Year = new(nameof(Year), Month, 12m);
    #endregion

    #region LuminousIntensity
    public readonly ScalarBaseUnit CandlePower = new(nameof(CandlePower), Candela, 0.981m);
    public readonly ScalarBaseUnit Hefnerkerze = new(nameof(Hefnerkerze), Candela, 0.90337m);
    public readonly ScalarBaseUnit Voille = new(nameof(Voille), Candela, 20.17m);
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
        Degree = db.GetOrDefineScalarBaseUnit(nameof(Degree), Radian, Pi / 180m, symbol: "°");
        Gradian = db.GetOrDefineScalarBaseUnit(nameof(Gradian), Radian, Pi / 200m, symbol: "grad");
        ArcMinute = db.GetOrDefineScalarBaseUnit(nameof(ArcMinute), Degree, 1m / 60m, symbol: "arcmin");
        ArcSecond = db.GetOrDefineScalarBaseUnit(nameof(ArcSecond), ArcMinute, 1m / 60m, symbol: "arcs");
        Revolution = db.GetOrDefineScalarBaseUnit(nameof(Revolution), Radian, TwoPi, symbol: "rev");
        Quadrant = db.GetOrDefineScalarBaseUnit(nameof(Quadrant), Radian, PiHalf);
        Sign = db.GetOrDefineScalarBaseUnit(nameof(Sign), Degree, 30m);
        #endregion

        #region SolidAngle
        Hemisphere = db.GetOrDefineScalarBaseUnit(nameof(Hemisphere), Steradian, TwoPi);
        SquareDegree = db.GetOrDefineScalarBaseUnit(nameof(SquareDegree), Steradian, Pow(Pi / 180m, 2m));
        Sphere = db.GetOrDefineScalarBaseUnit(nameof(Sphere), Hemisphere, 2m);
        #endregion

        #region ElectricCurrent
        Abampere = db.GetOrDefineScalarBaseUnit(nameof(Abampere), Ampere, 10m, symbol: "abA");
        Biot = db.GetOrDefineScalarBaseUnit(nameof(Biot), Abampere, 1m);
        #endregion

        #region Information
        Byte = db.GetOrDefineScalarBaseUnit(nameof(Byte), Bit, 8m);
        Nibble = db.GetOrDefineScalarBaseUnit(nameof(Nibble), Bit, 4m);
        Crumb = db.GetOrDefineScalarBaseUnit(nameof(Crumb), Bit, 2m);
        Word = db.GetOrDefineScalarBaseUnit(nameof(Word), Bit, 16m);
        #endregion

        #region Length
        Inch = db.GetOrDefineScalarBaseUnit(nameof(Inch), Meter, 0.0254m, symbol: "in");
        Foot = db.GetOrDefineScalarBaseUnit(nameof(Foot), Inch, 12m, symbol: "ft");
        Yard = db.GetOrDefineScalarBaseUnit(nameof(Yard), Foot, 3m, symbol: "yd");
        Mile = db.GetOrDefineScalarBaseUnit(nameof(Mile), Foot, 5280m, symbol: "mi");
        NauticalMile = db.GetOrDefineScalarBaseUnit(nameof(NauticalMile), Meter, 1852m, symbol: "nmi");
        Cable = db.GetOrDefineScalarBaseUnit(nameof(Cable), NauticalMile, 0.1m);
        CableUsSurvey = db.GetOrDefineScalarBaseUnit(nameof(CableUsSurvey), Yard, 240m);
        Caliber = db.GetOrDefineScalarBaseUnit(nameof(Caliber), Inch, 0.01m);
        ChainGunters = db.GetOrDefineScalarBaseUnit(nameof(ChainGunters), Yard, 22m);
        ChainEngineers = db.GetOrDefineScalarBaseUnit(nameof(ChainEngineers), Foot, 100m);
        Cubit = db.GetOrDefineScalarBaseUnit(nameof(Cubit), Foot, 1.5m);
        FootUsSurvey = db.GetOrDefineScalarBaseUnit(nameof(FootUsSurvey), Foot, 1.000002m);
        Fathom = db.GetOrDefineScalarBaseUnit(nameof(Fathom), Yard, 2m);
        Furlong = db.GetOrDefineScalarBaseUnit(nameof(Furlong), Foot, 660m, symbol: "fur");
        Hand = db.GetOrDefineScalarBaseUnit(nameof(Hand), Inch, 4m, symbol: "hh");
        LandLeague = db.GetOrDefineScalarBaseUnit(nameof(LandLeague), Mile, 3m);
        Pica = db.GetOrDefineScalarBaseUnit(nameof(Pica), Inch, 0.166m);
        Point = db.GetOrDefineScalarBaseUnit(nameof(Point), Pica, 1m / 12m);
        Rod = db.GetOrDefineScalarBaseUnit(nameof(Rod), Yard, 5.5m);
        Angstrom = db.GetOrDefineScalarBaseUnit(nameof(Angstrom), Meter, 1e-10m, symbol: "Å");
        AstronomicalUnit = db.GetOrDefineScalarBaseUnit(nameof(AstronomicalUnit), Meter, 1.495978e11m, symbol: "AU");
        Parsec = db.GetOrDefineScalarBaseUnit(nameof(Parsec), AstronomicalUnit, 206265m, symbol: "pc");
        LightYear = db.GetOrDefineScalarBaseUnit(nameof(LightYear), Parsec, 0.306601m, symbol: "ly");
        Mil = db.GetOrDefineScalarBaseUnit(nameof(Mil), Inch, 0.001m);
        #endregion
    }
}

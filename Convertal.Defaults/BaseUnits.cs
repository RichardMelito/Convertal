// Created by Richard Melito and licensed to you under The Clear BSD License.

using Convertal.Core;
using static Convertal.Defaults.BaseQuantities;
using static DecimalMath.DecimalEx;

namespace Convertal.Defaults;

public static class BaseUnits
{
    #region Fundamental units
    public static readonly BaseUnit Radian = (BaseUnit)Angle.FundamentalUnit;
    public static readonly BaseUnit Steradian = (BaseUnit)SolidAngle.FundamentalUnit;
    public static readonly BaseUnit Ampere = (BaseUnit)ElectricCurrent.FundamentalUnit;
    public static readonly BaseUnit Bit = (BaseUnit)Information.FundamentalUnit;
    public static readonly BaseUnit Meter = (BaseUnit)Length.FundamentalUnit;
    public static readonly PrefixedBaseUnit Kilogram = (PrefixedBaseUnit)Mass.FundamentalUnit;
    public static readonly BaseUnit Gram = Kilogram.Unit;
    public static readonly BaseUnit Kelvin = (BaseUnit)Temperature.FundamentalUnit;
    public static readonly BaseUnit Second = (BaseUnit)Time.FundamentalUnit;
    public static readonly BaseUnit Candela = (BaseUnit)LuminousIntensity.FundamentalUnit;
    #endregion

    #region Angle
    public static readonly BaseUnit Degree = new(nameof(Degree), Radian, Pi / 180m);
    public static readonly BaseUnit Gradian = new(nameof(Gradian), Radian, Pi / 200m);
    public static readonly BaseUnit ArcMinute = new(nameof(ArcMinute), Degree, 1m / 60m);
    public static readonly BaseUnit ArcSecond = new(nameof(ArcSecond), ArcMinute, 1m / 60m);
    public static readonly BaseUnit Revolution = new(nameof(Revolution), Radian, TwoPi);
    public static readonly BaseUnit Quadrant = new(nameof(Quadrant), Radian, PiHalf);
    public static readonly BaseUnit Sign = new(nameof(Sign), Degree, 30m);
    #endregion

    #region SolidAngle
    public static readonly BaseUnit Hemisphere = new(nameof(Hemisphere), Steradian, TwoPi);
    public static readonly BaseUnit SquareDegree = new(nameof(SquareDegree), Steradian, Pow(Pi / 180m, 2m));
    public static readonly BaseUnit Sphere = new(nameof(Sphere), Hemisphere, 2m);
    #endregion

    #region ElectricCurrent
    public static readonly BaseUnit Abampere = new(nameof(Abampere), Ampere, 10m);
    public static readonly BaseUnit Biot = new(nameof(Biot), Abampere, 1m);
    #endregion

    #region Information
    public static readonly BaseUnit Byte = new(nameof(Byte), Bit, 8m);
    public static readonly BaseUnit Nibble = new(nameof(Nibble), Bit, 4m);
    public static readonly BaseUnit Crumb = new(nameof(Crumb), Bit, 2m);
    public static readonly BaseUnit Word = new(nameof(Word), Bit, 16m);
    #endregion

    #region Length
    public static readonly BaseUnit Inch = new(nameof(Inch), Meter, 0.0254m);
    public static readonly BaseUnit Foot = new(nameof(Foot), Inch, 12m);
    public static readonly BaseUnit Yard = new(nameof(Yard), Foot, 3m);
    public static readonly BaseUnit Mile = new(nameof(Mile), Foot, 5280m);
    public static readonly BaseUnit NauticalMile = new(nameof(NauticalMile), Meter, 1852m);
    public static readonly BaseUnit Cable = new(nameof(Cable), NauticalMile, 0.1m);
    public static readonly BaseUnit CableUsSurvey = new(nameof(CableUsSurvey), Yard, 240m);
    public static readonly BaseUnit Caliber = new(nameof(Caliber), Inch, 0.01m);
    public static readonly BaseUnit ChainGunters = new(nameof(ChainGunters), Yard, 22m);
    public static readonly BaseUnit ChainEngineers = new(nameof(ChainEngineers), Foot, 100m);
    public static readonly BaseUnit Cubit = new(nameof(Cubit), Foot, 1.5m);
    public static readonly BaseUnit FootUsSurvey = new(nameof(FootUsSurvey), Foot, 1.000002m);
    public static readonly BaseUnit Fathom = new(nameof(Fathom), Yard, 2m);
    public static readonly BaseUnit Furlong = new(nameof(Furlong), Foot, 660m);
    public static readonly BaseUnit Hand = new(nameof(Hand), Inch, 4m);
    public static readonly BaseUnit LandLeague = new(nameof(LandLeague), Mile, 3m);
    public static readonly BaseUnit Pica = new(nameof(Pica), Inch, 0.166m);
    public static readonly BaseUnit Point = new(nameof(Point), Pica, 1m / 12m);
    public static readonly BaseUnit Pixel = new(nameof(Pixel), Pica, 1m / 16m);
    public static readonly BaseUnit Rod = new(nameof(Rod), Yard, 5.5m);
    public static readonly BaseUnit Angstrom = new(nameof(Angstrom), Meter, 1e-10m);
    public static readonly BaseUnit AstronomicalUnit = new(nameof(AstronomicalUnit), Meter, 1.495978e11m);
    public static readonly BaseUnit Parsec = new(nameof(Parsec), AstronomicalUnit, 206265m);
    public static readonly BaseUnit LightYear = new(nameof(LightYear), Parsec, 0.306601m);
    public static readonly BaseUnit Mil = new(nameof(Mil), Inch, 0.001m);
    #endregion

    #region Mass
    public static readonly BaseUnit Carat = new(nameof(Carat), Gram, 0.2m);
    public static readonly BaseUnit PoundMass = new(nameof(PoundMass), Kilogram, 0.4536m);
    public static readonly BaseUnit Ounce = new(nameof(Ounce), PoundMass, 1m / 16m);
    public static readonly BaseUnit Dram = new(nameof(Dram), Ounce, 1m / 16m);
    public static readonly BaseUnit Stone = new(nameof(Stone), PoundMass, 14m);
    public static readonly BaseUnit Slug = new(nameof(Slug), PoundMass, 32.174m);
    public static readonly BaseUnit ShortTon = new(nameof(ShortTon), PoundMass, 2000m);
    public static readonly BaseUnit LongTon = new(nameof(LongTon), PoundMass, 2240m);
    public static readonly BaseUnit Tonne = new(nameof(Tonne), Kilogram, 1000m);
    public static readonly BaseUnit AtomicMass = new(nameof(AtomicMass), Kilogram, 1.66054e-27m);
    public static readonly BaseUnit Scruple = new(nameof(Scruple), Ounce, 1m / 24m);
    #endregion

    #region Temperature
    public static readonly BaseUnit Celsius = new(nameof(Celsius), Kelvin, 1m, 273.15m);
    public static readonly BaseUnit Rankine = new(nameof(Rankine), Kelvin, 4m / 9m);
    public static readonly BaseUnit Fahrenheit = new(nameof(Fahrenheit), Rankine, 1m, 459.67m);
    #endregion

    #region Time
    public static readonly BaseUnit Minute = new(nameof(Minute), Second, 60m);
    public static readonly BaseUnit Hour = new(nameof(Hour), Minute, 60m);
    public static readonly BaseUnit Day = new(nameof(Day), Hour, 24m);
    public static readonly BaseUnit Week = new(nameof(Week), Day, 7m);
    public static readonly BaseUnit Month = new(nameof(Month), Minute, 4330m);
    public static readonly BaseUnit Year = new(nameof(Year), Month, 12m);
    #endregion

    #region LuminousIntensity
    public static readonly BaseUnit CandlePower = new(nameof(CandlePower), Candela, 0.981m);
    public static readonly BaseUnit Hefnerkerze = new(nameof(Hefnerkerze), Candela, 0.90337m);
    public static readonly BaseUnit Voille = new(nameof(Voille), Candela, 20.17m);
    #endregion

    static BaseUnits()
    {
        #region Angle
        //Degree.ChangeSymbol("°");
        Gradian.ChangeSymbol("grad");
        ArcMinute.ChangeSymbol("arcmin");
        ArcSecond.ChangeSymbol("arcs");
        Revolution.ChangeSymbol("rev");
        #endregion

        #region ElectricCurrent
        Abampere.ChangeSymbol("abA");
        #endregion

        #region Length
        Inch.ChangeSymbol("in");
        Foot.ChangeSymbol("ft");
        Yard.ChangeSymbol("yd");
        NauticalMile.ChangeSymbol("nmi");
        Furlong.ChangeSymbol("fur");
        Hand.ChangeSymbol("hh");
        LightYear.ChangeSymbol("ly");
        Parsec.ChangeSymbol("pc");
        Pixel.ChangeSymbol("px");
        //Angstrom.ChangeSymbol("Å");
        AstronomicalUnit.ChangeSymbol("AU");
        #endregion
    }
}

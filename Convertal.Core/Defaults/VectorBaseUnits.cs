// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core.Defaults;

public class VectorBaseUnits
{
    #region Fundamental units
    public readonly VectorBaseUnit Radian;
    public readonly VectorBaseUnit Meter;
    #endregion

    #region Angle
    public readonly VectorBaseUnit Degree;
    public readonly VectorBaseUnit Gradian;
    public readonly VectorBaseUnit ArcMinute;
    public readonly VectorBaseUnit ArcSecond;
    public readonly VectorBaseUnit Revolution;
    public readonly VectorBaseUnit Quadrant;
    public readonly VectorBaseUnit Sign;
    #endregion

    #region Length
    public readonly VectorBaseUnit Inch;
    public readonly VectorBaseUnit Foot;
    public readonly VectorBaseUnit Yard;
    public readonly VectorBaseUnit Mile;
    public readonly VectorBaseUnit NauticalMile;
    public readonly VectorBaseUnit Cable;
    public readonly VectorBaseUnit CableUsSurvey;
    public readonly VectorBaseUnit Caliber;
    public readonly VectorBaseUnit ChainGunters;
    public readonly VectorBaseUnit ChainEngineers;
    public readonly VectorBaseUnit Cubit;
    public readonly VectorBaseUnit FootUsSurvey;
    public readonly VectorBaseUnit Fathom;
    public readonly VectorBaseUnit Furlong;
    public readonly VectorBaseUnit Hand;
    public readonly VectorBaseUnit LandLeague;
    public readonly VectorBaseUnit Pica;
    public readonly VectorBaseUnit Point;
    public readonly VectorBaseUnit Rod;
    public readonly VectorBaseUnit Angstrom;
    public readonly VectorBaseUnit AstronomicalUnit;
    public readonly VectorBaseUnit Parsec;
    public readonly VectorBaseUnit LightYear;
    public readonly VectorBaseUnit Mil;
    #endregion

    internal VectorBaseUnits(DefaultDatabaseWrapper defaultDatabase)
    {
        var sbus = defaultDatabase.ScalarBaseUnits;

        #region Fundamental units
        Radian = sbus.Radian.VectorAnalog!;
        Meter = sbus.Meter.VectorAnalog!;
        #endregion

        #region Angle
        Degree = sbus.Degree.VectorAnalog!;
        Gradian = sbus.Gradian.VectorAnalog!;
        ArcMinute = sbus.ArcMinute.VectorAnalog!;
        ArcSecond = sbus.ArcSecond.VectorAnalog!;
        Revolution = sbus.Revolution.VectorAnalog!;
        Quadrant = sbus.Quadrant.VectorAnalog!;
        Sign = sbus.Sign.VectorAnalog!;
        #endregion

        #region Length
        Inch = sbus.Inch.VectorAnalog!;
        Foot = sbus.Foot.VectorAnalog!;
        Yard = sbus.Yard.VectorAnalog!;
        Mile = sbus.Mile.VectorAnalog!;
        NauticalMile = sbus.NauticalMile.VectorAnalog!;
        Cable = sbus.Cable.VectorAnalog!;
        CableUsSurvey = sbus.CableUsSurvey.VectorAnalog!;
        Caliber = sbus.Caliber.VectorAnalog!;
        ChainGunters = sbus.ChainGunters.VectorAnalog!;
        ChainEngineers = sbus.ChainEngineers.VectorAnalog!;
        Cubit = sbus.Cubit.VectorAnalog!;
        FootUsSurvey = sbus.FootUsSurvey.VectorAnalog!;
        Fathom = sbus.Fathom.VectorAnalog!;
        Furlong = sbus.Furlong.VectorAnalog!;
        Hand = sbus.Hand.VectorAnalog!;
        LandLeague = sbus.LandLeague.VectorAnalog!;
        Pica = sbus.Pica.VectorAnalog!;
        Point = sbus.Point.VectorAnalog!;
        Rod = sbus.Rod.VectorAnalog!;
        Angstrom = sbus.Angstrom.VectorAnalog!;
        AstronomicalUnit = sbus.AstronomicalUnit.VectorAnalog!;
        Parsec = sbus.Parsec.VectorAnalog!;
        LightYear = sbus.LightYear.VectorAnalog!;
        Mil = sbus.Mil.VectorAnalog!;
        #endregion
    }
}

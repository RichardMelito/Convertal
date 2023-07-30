// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace Convertal.Core.Defaults;

public class ScalarBaseQuantities
{
    public readonly ScalarBaseQuantity Angle;
    public readonly ScalarBaseQuantity SolidAngle;
    public readonly ScalarBaseQuantity ElectricCurrent;
    public readonly ScalarBaseQuantity Information;
    public readonly ScalarBaseQuantity Length;
    public readonly ScalarBaseQuantity Mass;
    public readonly ScalarBaseQuantity Temperature;
    public readonly ScalarBaseQuantity Time;
    public readonly ScalarBaseQuantity LuminousIntensity;

    internal ScalarBaseQuantities(DefaultDatabaseWrapper defaultDatabase)
    {
        var database = defaultDatabase.Database;
        var prefixes = defaultDatabase.Prefixes;

        Angle = database.GetOrDefineScalarBaseQuantity(nameof(Angle), "Radian", unitSymbol: "rad");
        SolidAngle = database.GetOrDefineScalarBaseQuantity(nameof(SolidAngle), "Steradian", unitSymbol: "sr");
        // TODO improve name system to allow spaces or underscores
        ElectricCurrent = database.GetOrDefineScalarBaseQuantity(nameof(ElectricCurrent), "Ampere", quantitySymbol: "I", unitSymbol: "A");
        Information = database.GetOrDefineScalarBaseQuantity(nameof(Information), "Bit", unitSymbol: "b");
        Length = database.GetOrDefineScalarBaseQuantity(nameof(Length), "Meter", quantitySymbol: "l", unitSymbol: "m");
        Mass = database.GetOrDefineScalarBaseQuantity(nameof(Mass), "Gram", unitPrefix: prefixes.Kilo, quantitySymbol: "M", unitSymbol: "g");
        Temperature = database.GetOrDefineScalarBaseQuantity(nameof(Temperature), "Kelvin", quantitySymbol: "Θ", unitSymbol: "K");
        Time = database.GetOrDefineScalarBaseQuantity(nameof(Time), "Second", quantitySymbol: "T", unitSymbol: "s");
        LuminousIntensity = database.GetOrDefineScalarBaseQuantity(nameof(LuminousIntensity), "Candela", quantitySymbol: "J", unitSymbol: "cd");
    }
}

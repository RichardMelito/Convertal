// Created by Richard Melito and licensed to you under The Clear BSD License.

using Convertal.Core;

namespace Convertal.Defaults;

public static class BaseQuantities
{
    public static readonly BaseQuantity Angle;
    public static readonly BaseQuantity SolidAngle;
    public static readonly BaseQuantity ElectricCurrent;
    public static readonly BaseQuantity Information;
    public static readonly BaseQuantity Length;
    public static readonly BaseQuantity Mass;
    public static readonly BaseQuantity Temperature;
    public static readonly BaseQuantity Time;
    public static readonly BaseQuantity LuminousIntensity;

    static BaseQuantities()
    {
        Angle = DefineNewBaseQuantity(nameof(Angle), "Radian", unitSymbol: "rad");
        SolidAngle = DefineNewBaseQuantity(nameof(SolidAngle), "Steradian", unitSymbol: "sr");
        // TODO improve name system to allow spaces or underscores
        ElectricCurrent = DefineNewBaseQuantity(nameof(ElectricCurrent), "Ampere", quantitySymbol: "I", unitSymbol: "A");
        Information = DefineNewBaseQuantity(nameof(Information), "Bit", unitSymbol: "b");
        Length = DefineNewBaseQuantity(nameof(Length), "Meter", quantitySymbol: "l", unitSymbol: "m");
        Mass = DefineNewBaseQuantity(nameof(Mass), "Gram", unitPrefix: Prefixes.Kilo, quantitySymbol: "M", unitSymbol: "g");
        Temperature = DefineNewBaseQuantity(nameof(Temperature), "Kelvin", quantitySymbol: "Θ", unitSymbol: "K");
        Time = DefineNewBaseQuantity(nameof(Time), "Second", quantitySymbol: "T", unitSymbol: "s");
        LuminousIntensity = DefineNewBaseQuantity(nameof(LuminousIntensity), "Candela", quantitySymbol: "J", unitSymbol: "cd");
    }
}

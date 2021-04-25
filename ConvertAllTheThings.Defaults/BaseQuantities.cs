using System;
using ConvertAllTheThings.Core;
using static ConvertAllTheThings.Core.BaseQuantity;

namespace ConvertAllTheThings.Defaults
{
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
            Angle = DefineNewBaseQuantity(nameof(Angle), "radian", unitSymbol: "rad");
            SolidAngle = DefineNewBaseQuantity(nameof(SolidAngle), "steradian", unitSymbol: "sr");
            // TODO improve name system to allow spaces or underscores
            ElectricCurrent = DefineNewBaseQuantity(nameof(ElectricCurrent), "ampere", quantitySymbol: "I", unitSymbol: "A");
            Information = DefineNewBaseQuantity(nameof(Information), "bit", unitSymbol: "b");
            Length = DefineNewBaseQuantity(nameof(Length), "meter", quantitySymbol: "l", unitSymbol: "m");
            Mass = DefineNewBaseQuantity(nameof(Mass), "gram", unitPrefix: Prefixes.Kilo, quantitySymbol: "M", unitSymbol: "g");
            Temperature = DefineNewBaseQuantity(nameof(Temperature), "kelvin", quantitySymbol: "Θ", unitSymbol: "K");
            Time = DefineNewBaseQuantity(nameof(Time), "second", quantitySymbol: "T", unitSymbol: "s");
            LuminousIntensity = DefineNewBaseQuantity(nameof(LuminousIntensity), "candela", quantitySymbol: "J", unitSymbol: "cd");
        }
    }
}

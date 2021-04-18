using System;
using ConvertAllTheThings.Core;
using static ConvertAllTheThings.Core.BaseQuantity;

namespace ConvertAllTheThings.Defaults
{
    public static class BaseQuantities
    {
        public static readonly BaseQuantity Angle;
        public static readonly BaseQuantity ElectricCharge;
        public static readonly BaseQuantity Information;
        public static readonly BaseQuantity Length;
        public static readonly BaseQuantity Mass;
        public static readonly BaseQuantity Temperature;
        public static readonly BaseQuantity Time;

        static BaseQuantities()
        {
            Angle = DefineNewBaseQuantity("angle", "radian", unitSymbol: "rad");
            // TODO improve name system to allow spaces or underscores
            ElectricCharge = DefineNewBaseQuantity("electriccharge", "coulomb", quantitySymbol: "q", unitSymbol: "C");
            Information = DefineNewBaseQuantity("information", "bit", unitSymbol: "b");
            Length = DefineNewBaseQuantity("time", "meter", quantitySymbol: "L", unitSymbol: "m");
            Mass = DefineNewBaseQuantity("mass", "gram", unitPrefix: Prefixes.Kilo, quantitySymbol: "M", unitSymbol: "g");
            Temperature = DefineNewBaseQuantity("temperature", "kelvin", quantitySymbol: "Θ", unitSymbol: "K");
            Time = DefineNewBaseQuantity("time", "second", quantitySymbol: "T", unitSymbol: "s");
        }
    }
}

// Created by Richard Melito and licensed to you under The Clear BSD License.

using Convertal.Core;
using DecimalMath;

namespace Convertal.Defaults;

public static class Prefixes
{
    public static readonly Prefix Yotta;
    public static readonly Prefix Zetta;
    public static readonly Prefix Exa;
    public static readonly Prefix Peta;
    public static readonly Prefix Tera;
    public static readonly Prefix Giga;
    public static readonly Prefix Mega;
    public static readonly Prefix Kilo;
    public static readonly Prefix Hecto;
    public static readonly Prefix Deca;

    public static readonly Prefix Deci;
    public static readonly Prefix Centi;
    public static readonly Prefix Milli;
    public static readonly Prefix Micro;
    public static readonly Prefix Nano;
    public static readonly Prefix Pico;
    public static readonly Prefix Femto;
    public static readonly Prefix Atto;
    public static readonly Prefix Zepto;
    public static readonly Prefix Yocto;

    public static readonly Prefix Kibi;
    public static readonly Prefix Mebi;
    public static readonly Prefix Gibi;
    public static readonly Prefix Tebi;
    public static readonly Prefix Pebi;
    public static readonly Prefix Exbi;

    static Prefixes()
    {
        Yotta = new("yotta", 1e24m, "Y");
        Zetta = new("zetta", 1e21m, "Z");
        Exa = new("exa", 1e18m, "E");
        Peta = new("peta", 1e15m, "P");
        Tera = new("tera", 1e12m, "T");
        Giga = new("giga", 1e9m, "G");
        Mega = new("mega", 1e6m, "M");
        Kilo = new("kilo", 1e3m, "k");
        Hecto = new("hecto", 1e2m, "h");
        Deca = new("deca", 1e1m, "da");

        Deci = new("deci", 1e-1m, "d");
        Centi = new("centi", 1e-2m, "c");
        Milli = new("milli", 1e-3m, "m");
        Micro = new("micro", 1e-6m, "μ");
        Nano = new("nano", 1e-9m, "n");
        Pico = new("pico", 1e-12m, "p");
        Femto = new("femto", 1e-15m, "f");
        Atto = new("atto", 1e-18m, "a");
        Zepto = new("zepto", 1e-21m, "z");
        Yocto = new("yocto", 1e-24m, "y");

        Kibi = new("kibi", DecimalEx.Pow(2m, 10m), "Ki");
        Mebi = new("mebi", DecimalEx.Pow(2m, 20m), "Mi");
        Gibi = new("gibi", DecimalEx.Pow(2m, 30m), "Gi");
        Tebi = new("tebi", DecimalEx.Pow(2m, 40m), "Ti");
        Pebi = new("pebi", DecimalEx.Pow(2m, 50m), "Pi");
        Exbi = new("exbi", DecimalEx.Pow(2m, 60m), "Ei");
    }
}

using System.ComponentModel;

namespace ICSGameLauncher.Common.Enums;

public enum PegiAge
{
    [Description("PEGI 3+")]
    Pegi3 = 3,
    [Description("PEGI 7+")]
    Pegi7 = 7,
    [Description("PEGI 12+")]
    Pegi12 = 12,
    [Description("PEGI 16+")]
    Pegi16 = 16,
    [Description("PEGI 18+")]
    Pegi18 = 18,
    [Description("PEGI! : Parental Guidance Recommended")]
    PegiPg = 0
}
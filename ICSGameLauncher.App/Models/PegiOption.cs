using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.App.Models;

public record PegiOption(PegiAge Value, string Description)
{
    public override string ToString() => Description;
}
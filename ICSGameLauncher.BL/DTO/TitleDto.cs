using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.BL.DTO;

public sealed record TitleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PegiAge PegiRating { get; set; }
}
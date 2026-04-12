namespace ICSGameLauncher.BL.DTO;

public sealed record StudioDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
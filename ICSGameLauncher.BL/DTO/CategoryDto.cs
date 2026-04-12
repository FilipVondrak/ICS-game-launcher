namespace ICSGameLauncher.BL.DTO;

public sealed record CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
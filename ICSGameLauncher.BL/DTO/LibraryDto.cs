namespace ICSGameLauncher.BL.DTO;

public sealed record LibraryDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Description { get; set; }
    public int TitleCount { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.Data.Models;

public class Library
{
    [Key] public int Id { get; set; }

    [Required] public required int UserId { get; set; }

    [Required] public required User User { get; set; }

    public List<Title> Titles { get; } = [];

    public string? Description { get; set; }

    [Required] public required int TitleCount { get; set; }
}
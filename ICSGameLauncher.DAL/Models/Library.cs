using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.DAL.Models;

public class Library : IEntity
{
    [Key] public int Id { get; set; }

    [Required] public required int UserId { get; set; }

    [Required] public required User User { get; set; }

    public ICollection<Title> Titles { get; } = [];

    [StringLength(255)]
    public string? Description { get; set; }

    [Required] public required int TitleCount { get; set; }
}
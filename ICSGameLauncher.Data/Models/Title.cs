using System.ComponentModel.DataAnnotations;
using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.Data.Models;

public class Title
{
    [Key] public int Id { get; set; }

    [Required] public required string Name { get; set; }

    [Required] public required string Description { get; set; }

    [Required] public required PegiAge PegiRating { get; set; }

    [Required] public List<Studio> Studios { get; } = [];

    [Required] public List<Category> Categories { get; } = [];

    [Required] public List<Library> Libraries { get; } = [];
}
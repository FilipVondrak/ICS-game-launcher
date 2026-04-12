using System.ComponentModel.DataAnnotations;
using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.DAL.Models;

public sealed class TitleEntity : IEntity
{
    [Key] public int Id { get; set; }

    [StringLength(255)]
    [Required] public required string Name { get; set; }

    [StringLength(255)]
    [Required] public required string Description { get; set; }

    [Required] public required PegiAge PegiRating { get; set; }

    [Required] public ICollection<StudioEntity> Studios { get; } = [];

    [Required] public ICollection<CategoryEntity> Categories { get; } = [];

    [Required] public ICollection<LibraryEntity> Libraries { get; } = [];
}
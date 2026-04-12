using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.DAL.Models;

public sealed class StudioEntity : IEntity
{
    [Key] public int Id { get; set; }

    [StringLength(255)]
    [Required] public required string Name { get; set; }

    [Required] public ICollection<TitleEntity> Titles { get; } = [];
}
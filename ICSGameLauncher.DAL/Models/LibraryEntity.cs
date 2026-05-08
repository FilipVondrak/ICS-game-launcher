using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.DAL.Models;

public sealed class LibraryEntity : IEntity
{
    [Key] public Guid Id { get; set; }

    [Required] public required Guid UserId { get; set; }

    [Required] public required UserEntity User { get; set; }

    public ICollection<TitleEntity> Titles { get; } = [];

    [StringLength(255)]
    public string? Description { get; set; }

    [Required] public required int TitleCount { get; set; }
}
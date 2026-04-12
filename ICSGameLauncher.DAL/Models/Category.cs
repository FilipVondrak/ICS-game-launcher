using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.DAL.Models;

public class Category : IEntity
{
    [Key] public int Id { get; set; }

    [StringLength(255)]
    [Required] public required string Name { get; set; }

    [Required] public ICollection<Title> Titles { get; } = [];
}
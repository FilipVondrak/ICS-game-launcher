using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.Data.Models;

public class Category
{
    [Key] public int Id { get; set; }

    [StringLength(255)]
    [Required] public required string Name { get; set; }

    [Required] public ICollection<Title> Titles { get; } = [];
}
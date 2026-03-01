using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.Data.Models;

public class Category
{
    [Key] public int Id { get; set; }

    [Required] public required string Name { get; set; }

    [Required] public List<Title> Titles { get; } = [];
}
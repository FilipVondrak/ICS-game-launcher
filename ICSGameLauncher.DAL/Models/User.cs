using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.DAL.Models;

public class User : IEntity
{
    [Key] public int Id { get; set; }

    [StringLength(255)]
    [Required] public required string Username { get; set; }

    [StringLength(255)]
    [Required] public required string Name { get; set; }

    [StringLength(255)]
    [Required] public required string Surname { get; set; }

    [StringLength(255)]
    [Required] public required string Email { get; set; }

    [Required] public ICollection<Library> Libraries { get; } = [];
}
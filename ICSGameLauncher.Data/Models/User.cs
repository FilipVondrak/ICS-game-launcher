using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.Data.Models;

public class User
{
    [Key] public int Id { get; set; }

    [Required] public required string Username { get; set; }

    [Required] public required string Name { get; set; }

    [Required] public required string Surname { get; set; }

    [Required] public required string Email { get; set; }

    public Library? Library { get; set; }
}
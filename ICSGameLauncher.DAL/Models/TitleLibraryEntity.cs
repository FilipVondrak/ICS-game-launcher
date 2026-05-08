using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.DAL.Models;

public sealed class TitleLibraryEntity
{
    [Required] public required Guid TitleId { get; set; }
    [Required] public required TitleEntity Title { get; set; }

    [Required] public required Guid LibraryId { get; set; }
    [Required] public required LibraryEntity Library { get; set; }

    [Required] public DateTime LastPlayed { get; set; }

}
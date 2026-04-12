using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.DAL.Models;

public sealed class TitleLibraryEntity
{
    [Required] public required int TitleId { get; set; }
    [Required] public required TitleEntity Title { get; set; }

    [Required] public required int LibraryId { get; set; }
    [Required] public required LibraryEntity Library { get; set; }

    [Required] public DateTime LastPlayed { get; set; }

}
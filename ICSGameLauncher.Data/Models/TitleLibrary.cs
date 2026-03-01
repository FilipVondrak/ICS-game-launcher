using System.ComponentModel.DataAnnotations;

namespace ICSGameLauncher.Data.Models;

public class TitleLibrary
{
    [Required] public required int TitleId { get; set; }
    [Required] public required Title Title { get; set; }

    [Required] public required int LibraryId { get; set; }
    [Required] public required Library Library { get; set; }

    [Required] public DateTime LastPlayed { get; set; }

}
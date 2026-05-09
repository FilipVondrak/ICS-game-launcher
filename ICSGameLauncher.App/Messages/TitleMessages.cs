using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.App.Messages;

public record OpenTitleMessage(TitleDto Title, LibraryDto Library);
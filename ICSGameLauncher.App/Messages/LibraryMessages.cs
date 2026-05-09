using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.App.Messages;

public record OpenLibraryMessage(LibraryDto Library);
public record LibraryDeletedMessage(LibraryDto Library);
public record LibraryUpdatedMessage(LibraryDto Library);
public record UserLoadedMessage;
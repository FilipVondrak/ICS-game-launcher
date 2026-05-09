using CommunityToolkit.Mvvm.ComponentModel;
using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.App.ViewModels;

public partial class SelectableLibrary : ObservableObject
{
    public LibraryDto Library { get; set; } = null!;

    [ObservableProperty]
    public partial bool IsSelected { get; set; }
}
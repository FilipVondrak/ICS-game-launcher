using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using ICSGameLauncher.App.Messages;

namespace ICSGameLauncher.App.ViewModels;

public partial class AddToLibraryPopupViewModel : ObservableObject
{
    private readonly ILibraryFacade _libraryFacade;

    [ObservableProperty]
    public partial TitleDto? SelectedGame { get; set; }

    [ObservableProperty]
    public partial string PopupTitle { get; set; } = "Selected Game Title: ...";

    [ObservableProperty]
    public partial ObservableCollection<SelectableLibrary> Libraries { get; set; } = [];

    [ObservableProperty]
    public partial bool IsValidationVisible { get; set; }

    partial void OnSelectedGameChanged(TitleDto? value)
    {
        if (value is not null)
        {
            PopupTitle = $"Selected Game Title: {value.Name}";
        }
    }

    public Func<Task> RequestClose { get; set; } = null!;

    public AddToLibraryPopupViewModel(ILibraryFacade libraryFacade)
    {
        _libraryFacade = libraryFacade;
    }

    public async Task LoadLibrariesAsync()
    {
        var allLibraries = await _libraryFacade.GetAllLibrariesAsync();

        var validLibraries = allLibraries.Where(l => !string.IsNullOrWhiteSpace(l.Description)).ToList();

        var selectableLibraries = new List<SelectableLibrary>();

        foreach (var lib in validLibraries)
        {
            var detailedLibrary = await _libraryFacade.GetLibraryAsync(lib.Id);

            if (detailedLibrary is not null)
            {
                selectableLibraries.Add(new SelectableLibrary { Library = detailedLibrary });
            }
        }

        Libraries = new ObservableCollection<SelectableLibrary>(selectableLibraries);
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await RequestClose.Invoke();
    }

    [RelayCommand]
    private async Task Confirm()
    {
        var selectedLibraries = Libraries.Where(l => l.IsSelected).ToList();

        if (selectedLibraries.Count == 0)
        {
            IsValidationVisible = true;
            return;
        }

        if (SelectedGame is null) return;

        IsValidationVisible = false;

        foreach (var selectableLib in selectedLibraries)
        {
            await _libraryFacade.AddTitleToLibraryAsync(selectableLib.Library.Id, SelectedGame!.Id);

            var freshLibrary = await _libraryFacade.GetLibraryAsync(selectableLib.Library.Id);

            if (freshLibrary != null)
            {
                selectableLib.Library = freshLibrary;

                WeakReferenceMessenger.Default.Send(new LibraryUpdatedMessage(freshLibrary));
            }
        }

        await RequestClose.Invoke();
    }
}
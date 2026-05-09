using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public partial class StoreView : ContentView
{
    private const double PopupTopGap = 8;
    private const double PopupScreenPadding = 8;
    private const double PopupFallbackWidth = 280;
    private const double PopupMinHeight = 180;

    public StoreView(StoreViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
        SizeChanged += OnStoreViewSizeChanged;
        FilterButton.SizeChanged += OnFilterButtonSizeChanged;
        FilterPopup.SizeChanged += OnFilterPopupSizeChanged;
        viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(StoreViewModel.IsFilterPopupVisible) && FilterPopup.IsVisible)
        {
            SchedulePopupReposition();
        }
    }

    private void OnStoreViewSizeChanged(object? sender, EventArgs e)
    {
        if (FilterPopup.IsVisible)
        {
            SchedulePopupReposition();
        }
    }

    private void OnFilterButtonSizeChanged(object? sender, EventArgs e)
    {
        if (FilterPopup.IsVisible)
        {
            SchedulePopupReposition();
        }
    }

    private void OnFilterPopupSizeChanged(object? sender, EventArgs e)
    {
        if (FilterPopup.IsVisible)
        {
            SchedulePopupReposition();
        }
    }

    private void SchedulePopupReposition()
    {
        // Reposition after layout pass; during window resize immediate coordinates can be stale.
        Dispatcher.Dispatch(PositionFilterPopup);
    }

    private void PositionFilterPopup()
    {
        if (Width <= 0)
        {
            return;
        }

        double anchorX = FilterButton.X;
        if (FilterButton.Parent is VisualElement filterButtonParent)
        {
            anchorX += filterButtonParent.X;
        }

        double popupWidth = FilterPopup.Width > 0 ? FilterPopup.Width : PopupFallbackWidth;
        double maxX = Math.Max(PopupScreenPadding, Width - popupWidth - PopupScreenPadding);
        double clampedX = Math.Clamp(anchorX, PopupScreenPadding, maxX);

        double anchorY = FilterButton.Y + FilterButton.Height + PopupTopGap;
        if (FilterButton.Parent is VisualElement filterButtonParentY)
        {
            anchorY += filterButtonParentY.Y;
        }

        double availableHeight = Height - anchorY - PopupScreenPadding;
        FilterPopup.MaximumHeightRequest = Math.Max(PopupMinHeight, availableHeight);

        FilterPopup.TranslationX = clampedX;
        FilterPopup.TranslationY = anchorY;
    }

}

namespace ICSGameLauncher.App;

public partial class MainPage : ContentPage
{
    int Count { get; set; }

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object? sender, EventArgs e)
    {
        Count++;

        CounterBtn.Text = Count == 1 ? $"Clicked {Count} time" : $"Clicked {Count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}
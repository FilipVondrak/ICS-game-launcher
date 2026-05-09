using System.Windows.Input;

namespace ICSGameLauncher.App.Views.Components;

public partial class TitleCardView : ContentView
{
    public static readonly BindableProperty IsInStoreProperty =
        BindableProperty.Create(nameof(IsInStore), typeof(bool), typeof(TitleCardView), false);

    public static readonly BindableProperty TitleNameProperty =
        BindableProperty.Create(nameof(TitleName), typeof(string), typeof(TitleCardView), string.Empty);

    public static readonly BindableProperty PlayCommandProperty =
        BindableProperty.Create(nameof(PlayCommand), typeof(ICommand), typeof(TitleCardView));

    public static readonly BindableProperty DetailsCommandProperty =
        BindableProperty.Create(nameof(DetailsCommand), typeof(ICommand), typeof(TitleCardView));

    public static readonly BindableProperty EditCommandProperty =
        BindableProperty.Create(nameof(EditCommand), typeof(ICommand), typeof(TitleCardView));

    public static readonly BindableProperty RemoveCommandProperty =
        BindableProperty.Create(nameof(RemoveCommand), typeof(ICommand), typeof(TitleCardView));

    public static readonly BindableProperty AddToCommandProperty =
        BindableProperty.Create(nameof(AddToCommand), typeof(ICommand), typeof(TitleCardView));


    public static readonly BindableProperty AgeRatingProperty =
        BindableProperty.Create(nameof(AgeRating), typeof(string), typeof(TitleCardView), string.Empty);

    public string AgeRating
    {
        get => (string)GetValue(AgeRatingProperty);
        set => SetValue(AgeRatingProperty, value);
    }

    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(TitleCardView), string.Empty);
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }


    public bool IsInStore
    {
        get => (bool)GetValue(IsInStoreProperty);
        set => SetValue(IsInStoreProperty, value);
    }

    public string TitleName
    {
        get => (string)GetValue(TitleNameProperty);
        set => SetValue(TitleNameProperty, value);
    }

    public ICommand PlayCommand
    {
        get => (ICommand)GetValue(PlayCommandProperty);
        set => SetValue(PlayCommandProperty, value);
    }

    public ICommand DetailsCommand
    {
        get => (ICommand)GetValue(DetailsCommandProperty);
        set => SetValue(DetailsCommandProperty, value);
    }

    public ICommand EditCommand
    {
        get => (ICommand)GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }

    public ICommand RemoveCommand
    {
        get => (ICommand)GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }

    public ICommand AddToCommand
    {
        get => (ICommand)GetValue(AddToCommandProperty);
        set => SetValue(AddToCommandProperty, value);
    }

    public TitleCardView()
    {
        InitializeComponent();
    }
}
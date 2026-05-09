using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public partial class GameDetailsView : ContentView
{
    public GameDetailsView(GameDetailsViewModel gameDetailsViewModel)
    {
        InitializeComponent();
        BindingContext = gameDetailsViewModel;
    }

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        if (BindingContext is GameDetailsViewModel viewModel)
        {
            viewModel.LoadInformation();
        }
    }
}
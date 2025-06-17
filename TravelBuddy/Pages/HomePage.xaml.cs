using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class HomePage
{
    public HomePage(HomeViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel  viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
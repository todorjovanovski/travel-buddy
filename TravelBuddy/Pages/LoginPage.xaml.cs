using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class LoginPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
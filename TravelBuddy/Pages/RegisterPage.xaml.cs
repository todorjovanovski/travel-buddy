using TravelBuddy.Pages.Base;
using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class RegisterPage : ContentPageBase
{
    public RegisterPage(RegisterViewModel  viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
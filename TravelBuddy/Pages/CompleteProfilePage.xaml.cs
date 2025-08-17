using TravelBuddy.Pages.Base;
using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class CompleteProfilePage : ContentPageBase
{
    public CompleteProfilePage(CompleteProfileViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class CompleteProfilePage : ContentPage
{
    public CompleteProfilePage(CompleteProfileViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
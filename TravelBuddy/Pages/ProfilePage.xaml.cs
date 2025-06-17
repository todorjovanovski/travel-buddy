using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class ProfilePage
{
    public ProfilePage(ProfileViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
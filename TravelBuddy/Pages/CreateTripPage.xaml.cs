using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class CreateTripPage : ContentPage
{
    public CreateTripPage(CreateTripViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
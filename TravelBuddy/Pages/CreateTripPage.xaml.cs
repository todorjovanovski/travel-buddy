using TravelBuddy.Pages.Base;
using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class CreateTripPage : ContentPageBase
{
    public CreateTripPage(CreateTripViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
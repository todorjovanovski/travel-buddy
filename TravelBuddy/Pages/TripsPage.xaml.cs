using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class TripsPage
{
    public TripsPage(TripsViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
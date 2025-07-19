using TravelBuddy.Services.Interfaces;
using TravelBuddy.ViewModels.Base;

namespace TravelBuddy.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly INavigationService  _navigationService;

    public HomeViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
}
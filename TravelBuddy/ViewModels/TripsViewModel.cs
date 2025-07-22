using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TravelBuddy.Models;
using TravelBuddy.Pages;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.ViewModels.Base;

namespace TravelBuddy.ViewModels;

public partial class TripsViewModel : ViewModelBase
{
    private readonly INavigationService  _navigationService;
    private readonly IFirebaseDbService _firebaseDbService;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUserAnonymous))]
    private User? _loggedInUser;

    [ObservableProperty] 
    private ObservableCollection<Trip> _activeTrips = [];

    [ObservableProperty] 
    private Trip _currentTrip = null!;
    
    public bool IsUserAnonymous => LoggedInUser is null;
    public bool HasAnyTrips => ActiveTrips.Count != 0;

    public TripsViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService)
    {
        _navigationService = navigationService;
        _firebaseDbService = firebaseDbService;
    }
        
    [RelayCommand]
    private async Task GoToLogin()
    {
        await _navigationService.GoToAsync(nameof(LoginPage));
    }

    [RelayCommand]
    private async Task GoToHomePage()
    {
        await _navigationService.GoToAsync($"//{nameof(HomePage)}");
    }

    [RelayCommand]
    private async Task CreateTrip()
    {
        await _navigationService.GoToAsync(nameof(CreateTripPage));
    }

    protected override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        LoggedInUser = await _firebaseDbService.FetchLoggedInUser();
        if (LoggedInUser == null) return;

        var trips = new List<Trip>();
        foreach (var tripId in LoggedInUser.TripIds)
        {
            trips.Add(await _firebaseDbService.FetchTrip(tripId));
        }

        ActiveTrips = trips.Where(t => t.Status == TripStatus.Active).ToObservableCollection();
    }
}
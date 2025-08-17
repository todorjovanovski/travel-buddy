using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TravelBuddy.Models;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
using TravelBuddy.ViewModels.Base;
using TravelBuddy.Models.DTOs;
namespace TravelBuddy.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly INavigationService  _navigationService;
    private readonly IFirebaseDbService _firebaseDbService;

    [ObservableProperty]
    private TripForm _tripForm = new();
    
    [ObservableProperty]
    private ObservableCollection<TripCard> _trips = [];

    [ObservableProperty] 
    private bool _areFilterOptionsVisible;
    
    public User? LoggedInUser { get; set; }

    public HomeViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService)
    {
        _navigationService = navigationService;
        _firebaseDbService = firebaseDbService;
    }

    [RelayCommand]
    private void ToggleFilterOptions()
    {
        AreFilterOptionsVisible = !AreFilterOptionsVisible;
    }

    [RelayCommand]
    private async Task SearchTrips()
    {
        IsLoading = true;
        AreFilterOptionsVisible = false;
        var trips = await _firebaseDbService.FetchTrips(TripForm);
        Trips = await trips.ToTripCards(_firebaseDbService);
        TripForm = new TripForm();
        IsLoading = false;
    }

    [RelayCommand]
    private async Task ResetTripFilters()
    {
        IsLoading = true;
        var trips = await _firebaseDbService.FetchTrips();
        Trips = await trips.ToTripCards(_firebaseDbService);
        IsLoading = false;
    }

    [RelayCommand]
    private async Task RequestToJoinTrip(TripCard tripCard)
    {
        if (LoggedInUser == null) return; //TODO: SHOW LOGIN PAGE
        await _firebaseDbService.RequestToJoinTrip(tripCard.TripId, tripCard.TripOwner.Id, LoggedInUser);
    }

    protected override async Task OnAppearingAsync()
    {
        LoggedInUser = await _firebaseDbService.FetchLoggedInUser();
        var trips = await _firebaseDbService.FetchTrips();
        Trips = await trips.ToTripCards(_firebaseDbService);
        await base.OnAppearingAsync();
    }
}
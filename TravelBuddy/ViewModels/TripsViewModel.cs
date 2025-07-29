using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TravelBuddy.Models;
using TravelBuddy.Pages;
using TravelBuddy.Services;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
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
    [NotifyPropertyChangedFor(nameof(HasAnyTrips))]
    private ObservableCollection<TripCard> _activeTrips = [];

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

        var unsplashService = new UnsplashService(new HttpClient());

        var activeTrips = new ObservableCollection<TripCard>();
        foreach (var trip in trips)
        {
            if (trip.Status is not TripStatus.Active) continue;
            var participants = new ObservableCollection<Participant>();
            
            foreach (var participantId in trip.ParticipantIds)
            {
                var participant = await _firebaseDbService.FetchUser(participantId);
                participants.Add(new Participant
                {
                    Name = participant.Name,
                    ProfileImageSource = participant.ProfilePhotos.FirstOrDefault()?.Url
                });
            }

            //var photoUrl = await unsplashService.GetRandomPhotoUrlAsync(trip.Destination.ToString());

            var activeTrip = new TripCard
            {
                TripImageSource = "",
                Title = trip.Title,
                Destination = trip.Destination.ToString(),
                Date = $"{trip.StartDate:dd.MM.yyyy} -  {trip.EndDate:dd.MM.yyyy}",
                Budget = trip.Budget.GetDescription(),
                AdditionalInfo = trip.AdditionalInfo,
                Participants = participants
            };
            activeTrips.Add(activeTrip);
        }
        ActiveTrips = activeTrips;
    }
}
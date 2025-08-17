using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TravelBuddy.Models;
using TravelBuddy.Models.DTOs;
using TravelBuddy.Models.Enums;
using TravelBuddy.Pages;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
using TravelBuddy.ViewModels.Base;
using TravelBuddy.Views;

namespace TravelBuddy.ViewModels;

public partial class TripsViewModel : ViewModelBase
{
    private readonly INavigationService  _navigationService;
    private readonly IFirebaseDbService _firebaseDbService;
    private CompositeDisposable _tripRequestDisposables = new();
    private Popup _popup = new();
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUserAnonymous))]
    private User? _loggedInUser;

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(HasAnyTrips))]
    private ObservableCollection<TripCard> _activeTrips = [];

    [ObservableProperty] 
    private TripCard? _currentTrip;
    
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

    [RelayCommand]
    private async Task DeleteTrip(string tripId)
    {
        await _firebaseDbService.DeleteTrip(tripId);
    }
    
    [RelayCommand]
    private async Task ToggleRequests(string tripId)
    {
        var trip = ActiveTrips.First(t => t.TripId == tripId);
        _popup = new Popup
        {
            Content = new TripRequestsView
            {
                TripRequests = trip.TripRequests,
                AcceptTripRequestCommand = AcceptTripRequestCommand,
                RejectTripRequestCommand = RejectTripRequestCommand
            },
            CanBeDismissedByTappingOutsideOfPopup = true,
            Padding = new  Thickness(10)
        };
        await Shell.Current.ShowPopupAsync(_popup);
    }

    [RelayCommand]
    private async Task AcceptTripRequest(UserTripRequest userTripRequest)
    {
        var trip = await _firebaseDbService.FetchTrip(userTripRequest.TripId);
        trip.ParticipantIds.Add(userTripRequest.UserId);
        await _firebaseDbService.UpdateTripData(trip);
        
        var user = await _firebaseDbService.FetchUser(userTripRequest.UserId);
        user.TripIds.Add(userTripRequest.TripId);
        await _firebaseDbService.UpdateUserData(user);

        var tripRequest = new TripRequest
        {
            TripOwnerId = LoggedInUser!.Id,
            TripId = userTripRequest.TripId,
            Id = userTripRequest.Id,
            Status = RequestStatus.Accepted
        };
        await _firebaseDbService.UpdateTripRequestData(tripRequest);
        await _popup.CloseAsync();
    }
    
    [RelayCommand]
    private async Task RejectTripRequest(UserTripRequest tripRequest)
    {
        await Task.Delay(10);
    }

    [RelayCommand]
    private async Task OpenTripChat(string tripId)
    {
        try
        {
            await _navigationService.GoToAsync(nameof(ChatPage), new Dictionary<string, object>
            {
                { nameof(ChatViewModel.ChatId), tripId }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    private async Task CurrentTripChanged()
    {
        try
        {
            if (CurrentTrip is null) return;
            
            _tripRequestDisposables.Clear();
            _tripRequestDisposables.Add(_firebaseDbService.ObserveTripRequests(CurrentTrip.TripId).Subscribe(firebaseEvent =>
            {
                var tripRequest = firebaseEvent.Object;
                if (tripRequest is null) return;
                var requestUser = new UserTripRequest
                {
                    ProfileImageSource = tripRequest.RequestingUser.ProfilePhotos.FirstOrDefault()?.Url,
                    NameAndAge = $"{ tripRequest.RequestingUser.Name}, {tripRequest.RequestingUser.Age}",
                    Location = tripRequest.RequestingUser.Location.ToString(),
                    TripId = tripRequest.TripId,
                    UserId = tripRequest.RequestingUser.Id,
                    Id = tripRequest.Id
                };
                CurrentTrip.TripRequests.Add(requestUser);
                CurrentTrip.PendingRequests = CurrentTrip.TripRequests.Count;
            }));
            
            var tripRequests = await _firebaseDbService.FetchTripRequests(CurrentTrip.TripId);
            if (tripRequests == null) return;
            CurrentTrip.TripRequests = tripRequests.ToUserTripRequests();
            CurrentTrip.PendingRequests = CurrentTrip.TripRequests.Count;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    protected override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        _tripRequestDisposables = new CompositeDisposable();
        LoggedInUser = await _firebaseDbService.FetchLoggedInUser();
        if (LoggedInUser == null) return;

        var trips = new List<Trip>();
        foreach (var tripId in LoggedInUser.TripIds)
        {
            trips.Add(await _firebaseDbService.FetchTrip(tripId));
        }
        
        ActiveTrips = await trips.ToTripCards(_firebaseDbService);
    }

    protected override async Task OnDisappearingAsync()
    {
        await base.OnDisappearingAsync();
        _tripRequestDisposables.Dispose();
    }
}
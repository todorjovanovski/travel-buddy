using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using TravelBuddy.Models;
using TravelBuddy.Models.Enums;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
using TravelBuddy.ViewModels.Base;
using Location = TravelBuddy.Models.Location;
using User = TravelBuddy.Models.User;

namespace TravelBuddy.ViewModels;

public partial class CreateTripViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IFirebaseAuthClient _firebaseAuthClient;
    private readonly IFirebaseDbService _firebaseDbService;
    
    [ObservableProperty]
    private string _title = string.Empty;
    
    [ObservableProperty]
    private string _destination = string.Empty;
    
    [ObservableProperty]
    private ObservableCollection<Activity> _activities = Enum.GetValues<Activity>().ToObservableCollection();
    
    [ObservableProperty]
    private ObservableCollection<object> _selectedActivities = [];
    
    [ObservableProperty]
    private DateTime _startDate = DateTime.Now.AddDays(7);
    
    [ObservableProperty]
    private DateTime _endDate = DateTime.Now.AddDays(14);
    
    [ObservableProperty]
    private string _selectedBudget = string.Empty;
    
    [ObservableProperty]
    private string _selectedGroupSize = string.Empty;
    
    [ObservableProperty]
    private string _additionalInfo =  string.Empty;

    private User LoggedInUser { get; set; } = null!;

    public ObservableCollection<string> BudgetRange =>
        Enum.GetValues<Budget>().Select(e => e.GetDescription()).ToObservableCollection();
    public ObservableCollection<string> NumberOfCompanions => 
        Enum.GetValues<GroupSize>().Select(e => e.GetDescription()).ToObservableCollection();

    public CreateTripViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService, IFirebaseAuthClient firebaseAuthClient)
    {
        _navigationService = navigationService;
        _firebaseDbService = firebaseDbService;
        _firebaseAuthClient = firebaseAuthClient;
    }
    
    [RelayCommand]
    private async Task CreateNewTrip()
    {
        var destination = Destination.Split(",");
        var selectedBudget = SelectedBudget.FromDescription<Budget>();
        var selectedGroupSize = SelectedGroupSize.FromDescription<GroupSize>();
        var tripChat = new Chat { Title = Title };
        var tourGuideChat = new TourGuideChat { Id = tripChat.Id, Title = "Your virtual tour guide" };
        var trip = new Trip
        {
            OwnerId = _firebaseAuthClient.User.Uid,
            ChatId = tripChat.Id,
            Title = Title,
            StartDate = StartDate,
            EndDate = EndDate,
            Activities = SelectedActivities.Select(obj => Enum.Parse<Activity>(obj.ToString()!)).ToList(),
            Budget = selectedBudget ?? Budget.Undefined,
            GroupSize = selectedGroupSize ?? GroupSize.Undefined,
            AdditionalInfo = AdditionalInfo,
            Destination = destination.Length == 2
                ? new Location { City = destination[0].Trim(), Country = destination[1].Trim() }
                : new Location()
        };
        try
        {
            LoggedInUser.TripIds.Add(trip.Id);
            await _firebaseDbService.CreateTrip(trip);
            await _firebaseDbService.CreateChat(tripChat);
            await _firebaseDbService.CreateTourGuideChat(tourGuideChat);
            await _firebaseDbService.UpdateUserData(LoggedInUser);
            await _navigationService.GoBackAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    protected override async Task OnAppearingAsync()
    {
        LoggedInUser = (await _firebaseDbService.FetchLoggedInUser())!;
        await base.OnAppearingAsync();
    }
}
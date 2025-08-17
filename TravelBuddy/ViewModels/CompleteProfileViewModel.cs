using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TravelBuddy.Models.Enums;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.ViewModels.Base;
using Location = TravelBuddy.Models.Location;

namespace TravelBuddy.ViewModels;

public partial class CompleteProfileViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IFirebaseDbService _firebaseDbService;
    
    [ObservableProperty]
    private DateTime _birthdate = new(2000, 1, 1);
    
    [ObservableProperty]
    private string _location = string.Empty;
    
    [ObservableProperty]
    private string _bio = string.Empty;
    
    [ObservableProperty]
    private ObservableCollection<Activity> _activities = Enum.GetValues<Activity>().ToObservableCollection();
    
    [ObservableProperty]
    private ObservableCollection<object> _selectedActivities = [];
    
    public DateTime MinimumDate => new(DateTime.Now.Year - 100, 1, 1);
    public DateTime MaximumDate => new(DateTime.Now.Year - 10, 1, 1);

    public CompleteProfileViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService)
    {
        _navigationService = navigationService;
        _firebaseDbService = firebaseDbService;
    }

    [RelayCommand]
    private async Task SaveProfile()
    {
        var user = await _firebaseDbService.FetchLoggedInUser();
        if (user == null) return;
        var location = Location.Split(",");
        user.Birthdate = Birthdate;
        user.Location = location.Length == 2
            ? new Location { City = location[0].Trim(), Country = location[1].Trim() }
            : new Location();
        user.FavoriteActivities = SelectedActivities.Select(obj => Enum.Parse<Activity>(obj.ToString()!)).ToList();
        user.Bio = Bio;
        await _firebaseDbService.UpdateUserData(user);
        await _navigationService.GoBackAsync();
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await _navigationService.GoBackAsync();
    }
}
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using TravelBuddy.Models;
using TravelBuddy.Pages;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
using TravelBuddy.ViewModels.Base;
using User = TravelBuddy.Models.User;

namespace TravelBuddy.ViewModels;

public partial class ProfileViewModel : ViewModelBase
{
    private readonly IFirebaseAuthClient _firebaseAuthClient;
    private readonly IFirebaseDbService _firebaseDbService;
    private readonly INavigationService _navigationService;
    private readonly CompositeDisposable _disposables = new();
    
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(
        nameof(Activities), 
        nameof(Photos),
        nameof(NameAndAge), 
        nameof(Bio), 
        nameof(IsUserAnonymous),
        nameof(ArePhotosEmpty))]
    private User? _loggedInUser;

    [ObservableProperty] 
    private double _indicatorLength;
    
    public string NameAndAge => 
        LoggedInUser is null ? string.Empty : $"{LoggedInUser?.Name}, {LoggedInUser?.Age}";
    
    public string Bio => 
        LoggedInUser is null ? string.Empty : LoggedInUser.Bio;

    public ObservableCollection<Activity> Activities =>
        LoggedInUser is null ? [] : LoggedInUser.FavoriteActivities.ToObservableCollection();
    
    public ObservableCollection<ProfilePhoto> Photos => 
        LoggedInUser is null ? [] : LoggedInUser.ProfilePhotos.ToObservableCollection();
    
    
    public bool IsUserAnonymous => LoggedInUser is null;
    
    public bool ArePhotosEmpty => Photos.Count == 0;

    public ProfileViewModel(IFirebaseDbService firebaseDbService, INavigationService navigationService, IFirebaseAuthClient firebaseAuthClient)
    {
        _firebaseDbService = firebaseDbService;
        _navigationService = navigationService;
        _firebaseAuthClient = firebaseAuthClient;
    }
    
    [RelayCommand]
    private async Task GoToLogin()
    {
        await _navigationService.GoToAsync(nameof(LoginPage));
    }

    [RelayCommand]
    private async Task AddPhotos()
    {
        try
        {
            var options = new PickOptions
            {
                PickerTitle = "Choose your profile images.",
                FileTypes = FilePickerFileType.Images
            };
            var photos = await MainThread.InvokeOnMainThreadAsync(() => FilePicker.Default.PickMultipleAsync(options));
            foreach (var photo in photos)
            {
                await using var stream = await photo.OpenReadAsync();
                await _firebaseDbService.UploadUserPhoto(photo.FileName, stream); 
                var url = await _firebaseDbService.GetPhotoUrl(photo.FileName);
                var profilePhoto = new ProfilePhoto { Url = url };
                LoggedInUser!.ProfilePhotos.Add(profilePhoto);
            }
            await _firebaseDbService.UpdateUserData(LoggedInUser!);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    private void LogOut()
    {
        _firebaseAuthClient.SignOut();
        LoggedInUser = null;
    }

    protected override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        LoggedInUser = await _firebaseDbService.FetchLoggedInUser();
        if (LoggedInUser == null) return;
        
        _disposables.Add(_firebaseDbService.ObserveUserChanges().Subscribe(firebaseEvent =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var user = firebaseEvent.Object;
                LoggedInUser = user.Copy();
            });
        }));
    }

    protected override Task OnDisappearingAsync()
    {
        _disposables.Dispose();
        return base.OnDisappearingAsync();
    }

    public void CalculateIndicatorLength(double photosGridWidth)
    {
        IndicatorLength = (photosGridWidth - 26) / Photos.Count - Photos.Count * 2;
    }
}
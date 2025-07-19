using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using TravelBuddy.Pages;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
using TravelBuddy.ViewModels.Base;

namespace TravelBuddy.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IFirebaseAuthClient _firebaseAuthClient;
    private readonly IFirebaseDbService _firebaseDbService;
    
    [ObservableProperty]
    private string _fullName = string.Empty;
    
    [ObservableProperty]
    private string _email = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsErrorMessageVisible))]
    private string _errorMessage = string.Empty;
    
    [ObservableProperty]
    private bool _agreeToTermsAndConditions;

    public bool IsErrorMessageVisible => ErrorMessage != string.Empty;
    
    public RegisterViewModel(INavigationService navigationService, IFirebaseAuthClient firebaseAuthClient, IFirebaseDbService firebaseDbService)
    {
        _navigationService = navigationService;
        _firebaseAuthClient = firebaseAuthClient;
        _firebaseDbService = firebaseDbService;
    }

    [RelayCommand]
    private async Task Register()
    {
        try
        {
            UserCredentials.ValidateCredentials(FullName, Email, Password);
            await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(Email, Password, FullName);
            await _firebaseDbService.CreateUser(FullName);
            await _navigationService.GoToAsync($"///{nameof(ProfilePage)}/{nameof(CompleteProfilePage)}");
        }
        catch (FirebaseAuthException e)
        {
            ErrorMessage = e.Reason.ToReadableString();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }

    [RelayCommand]
    private async Task GoToLogin()
    {
        await _navigationService.GoBackAsync();
    }
}
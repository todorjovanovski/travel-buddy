using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using TravelBuddy.Pages;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
using TravelBuddy.ViewModels.Base;

namespace TravelBuddy.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IFirebaseAuthClient _firebaseAuthClient;
    
    [ObservableProperty]
    private string _email = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsErrorVisible))]
    private string _errorMessage = string.Empty;
    
    public bool IsErrorVisible => !string.IsNullOrWhiteSpace(ErrorMessage);
    
    public LoginViewModel(INavigationService navigationService, IFirebaseAuthClient firebaseAuthClient)
    {
        _navigationService = navigationService;
        _firebaseAuthClient = firebaseAuthClient;
    }

    [RelayCommand]
    private async Task Login()
    {
        try
        {
            ErrorMessage = string.Empty;
            await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(Email, Password);
            await _navigationService.GoBackAsync();
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
    private async Task CreateAccount()
    {
        ClearValues();
        await _navigationService.GoToAsync(nameof(RegisterPage));
    }

    private void ClearValues()
    {
        Email = string.Empty;
        Password = string.Empty;
        ErrorMessage = string.Empty;
    }
}
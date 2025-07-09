using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Database;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.ViewModels.Base;

namespace TravelBuddy.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly FirebaseAuthClient _firebaseAuthClient;
    
    [ObservableProperty]
    private string _fullName = string.Empty;
    
    [ObservableProperty]
    private string _email = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;
    

    public RegisterViewModel(INavigationService navigationService, FirebaseAuthClient firebaseAuthClient, FirebaseClient firebaseDbClient)
    {
        _navigationService = navigationService;
        _firebaseAuthClient = firebaseAuthClient;
    }

    [RelayCommand]
    private async Task SignUp()
    {
        try
        {
            await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(Email, Password, FullName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    [RelayCommand]
    private async Task GoToLogin()
    {
        await _navigationService.GoBackAsync();
    }
}
using CommunityToolkit.Mvvm.Input;

namespace TravelBuddy.ViewModels.Base;

public interface IBaseViewModel
{
    public IAsyncRelayCommand OnAppearingAsyncCommand { get; }
    public IAsyncRelayCommand OnDisappearingAsyncCommand { get; }
    public IAsyncRelayCommand OnSleepAsyncCommand { get; }
    public IAsyncRelayCommand OnResumeAsyncCommand { get; }
}
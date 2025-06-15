using TravelBuddy.ViewModels.Base;

namespace TravelBuddy.Pages;

public partial class BaseContentPage : ContentPage
{
    public BaseContentPage()
    {
        InitializeComponent();
    }
    
    protected override async void OnAppearing()
    {
        if (BindingContext is IBaseViewModel baseViewModel)
        {
            await baseViewModel.OnAppearingAsyncCommand.ExecuteAsync(null);
        }
        base.OnAppearing();
    }

    protected override async void OnDisappearing()
    {
        if (BindingContext is IBaseViewModel baseViewModel)
        {
            await baseViewModel.OnDisappearingAsyncCommand.ExecuteAsync(null);
        }
        base.OnDisappearing();
    }

    public virtual async Task OnSleep()
    {
        if (BindingContext is IBaseViewModel baseViewModel)
        {
            await baseViewModel.OnSleepAsyncCommand.ExecuteAsync(null);
        }
    }

    public virtual async Task OnResume()
    {
        if (BindingContext is IBaseViewModel baseViewModel)
        {
            await baseViewModel.OnResumeAsyncCommand.ExecuteAsync(null);
        }
    }
}
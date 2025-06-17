using TravelBuddy.ViewModels.Base;

namespace TravelBuddy.Pages;

public partial class ContentPageBase : ContentPage
{
    public ContentPageBase()
    {
        InitializeComponent();
    }
    
    protected override async void OnAppearing()
    {
        if (BindingContext is IViewModelBase baseViewModel)
        {
            await baseViewModel.OnAppearingAsyncCommand.ExecuteAsync(null);
        }
        base.OnAppearing();
    }

    protected override async void OnDisappearing()
    {
        if (BindingContext is IViewModelBase baseViewModel)
        {
            await baseViewModel.OnDisappearingAsyncCommand.ExecuteAsync(null);
        }
        base.OnDisappearing();
    }

    public virtual async Task OnSleep()
    {
        if (BindingContext is IViewModelBase baseViewModel)
        {
            await baseViewModel.OnSleepAsyncCommand.ExecuteAsync(null);
        }
    }

    public virtual async Task OnResume()
    {
        if (BindingContext is IViewModelBase baseViewModel)
        {
            await baseViewModel.OnResumeAsyncCommand.ExecuteAsync(null);
        }
    }
}
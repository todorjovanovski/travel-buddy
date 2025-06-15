using TravelBuddy.Pages;

namespace TravelBuddy;

public partial class App : Application
{
    private BaseContentPage? CurrentPage { get; set; }
    
    public App()
    {
        InitializeComponent();
        UserAppTheme = AppTheme.Unspecified;
        PageAppearing += OnPageAppearing;
    }
    
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
    
    private void OnPageAppearing(object? sender, Page e)
    {
        if (e is BaseContentPage currentPage)
        {
            CurrentPage = currentPage;
        }
    }


    protected override async void OnSleep()
    {
        if (CurrentPage is not null)
        {
            await CurrentPage.OnSleep();
        }
        base.OnSleep();
    }

    protected override async void OnResume()
    {
        if (CurrentPage is not null)
        {
            await CurrentPage.OnResume();
        }
        base.OnResume();
    }
}
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TravelBuddy.Pages;
using TravelBuddy.ViewModels;

namespace TravelBuddy;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterPagesWithViewModels();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
    
    public static void RegisterPagesWithViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransientWithShellRoute<HomePage, HomeViewModel>(nameof(HomePage));
        builder.Services.AddTransientWithShellRoute<LoginPage, LoginViewModel>(nameof(LoginPage));
        builder.Services.AddTransientWithShellRoute<ProfilePage, ProfileViewModel>(nameof(ProfilePage));
        builder.Services.AddTransientWithShellRoute<TripsPage, TripsViewModel>(nameof(TripsPage));
    }
}
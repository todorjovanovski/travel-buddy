using CommunityToolkit.Maui;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Microsoft.Extensions.Logging;
using TravelBuddy.Pages;
using TravelBuddy.Services;
using TravelBuddy.Services.Interfaces;
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
            .RegisterPagesWithViewModels()
            .RegisterServices();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
    
    public static MauiAppBuilder RegisterPagesWithViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransientWithShellRoute<HomePage, HomeViewModel>(nameof(HomePage));
        builder.Services.AddTransientWithShellRoute<LoginPage, LoginViewModel>(nameof(LoginPage));
        builder.Services.AddTransientWithShellRoute<ProfilePage, ProfileViewModel>(nameof(ProfilePage));
        builder.Services.AddTransientWithShellRoute<TripsPage, TripsViewModel>(nameof(TripsPage));
        builder.Services.AddTransientWithShellRoute<RegisterPage, RegisterViewModel>(nameof(RegisterPage));

        return builder;
    }
    
    private static void RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IAlertService, AlertService>();
        builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
        {
            ApiKey = "",
            AuthDomain = "",
            Providers = [new EmailProvider(), new GoogleProvider()]
        }));
        builder.Services.AddSingleton(sp =>
        {
            var authClient = sp.GetRequiredService<FirebaseAuthClient>();
            return new FirebaseClient(
                "",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authClient.User.Credential.IdToken)
                });
        });
    }
}
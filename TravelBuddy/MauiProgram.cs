using CommunityToolkit.Maui;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Microsoft.Extensions.Logging;
using TravelBuddy.Constants;
using TravelBuddy.Controls;
using TravelBuddy.Handlers;
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
                fonts.AddFont("Poppins-Regular.ttf",  "PoppinsRegular");
            })
            .RegisterPagesWithViewModels()
            .RegisterServices()
            .ConfigureMauiHandlers(ConfigureHandlers);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
    
    public static MauiAppBuilder RegisterPagesWithViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<TripsViewModel>();
        builder.Services.AddTransientWithShellRoute<LoginPage, LoginViewModel>(nameof(LoginPage));
        builder.Services.AddTransientWithShellRoute<RegisterPage, RegisterViewModel>(nameof(RegisterPage));
        builder.Services.AddTransientWithShellRoute<CompleteProfilePage, CompleteProfileViewModel>(nameof(CompleteProfilePage));

        return builder;
    }
    
    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IAlertService, AlertService>();
        builder.Services.AddSingleton<IFirebaseDbService, FirebaseDbService>();
        builder.Services.AddSingleton<IFirebaseAuthClient>(new FirebaseAuthClient(new FirebaseAuthConfig
        {
            ApiKey = FirebaseConstants.AuthApiKey,
            AuthDomain = FirebaseConstants.AuthDomain,
            Providers = [new EmailProvider(), new GoogleProvider()],
            UserRepository = new FileUserRepository("travel-buddy-auth")
        }));
        return builder;
    }
    
    private static void ConfigureHandlers(IMauiHandlersCollection handlersCollection)
    {
        handlersCollection.AddHandler<BorderlessEntry, CustomEntryHandler>();
        handlersCollection.AddHandler<BorderlessDatePicker, CustomDatePickerHandler>();
        handlersCollection.AddHandler<BorderlessEditor, CustomEditorHandler>();
    }
}
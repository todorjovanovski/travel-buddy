using System.Windows.Input;
using TravelBuddy.Models.DTOs;

namespace TravelBuddy.Views;

public partial class TripRequestView : ContentView
{
    public TripRequestView()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty UserTripRequestProperty =
        BindableProperty.Create(nameof(UserTripRequest), typeof(UserTripRequest), typeof(TripRequestView));

    public UserTripRequest UserTripRequest
    {
        get => (UserTripRequest)GetValue(UserTripRequestProperty);
        set => SetValue(UserTripRequestProperty, value);
    }
    
    
    public static readonly BindableProperty AcceptTripRequestCommandProperty =
        BindableProperty.Create(nameof(AcceptTripRequestCommand), typeof(ICommand), typeof(TripRequestView));
    
    public ICommand AcceptTripRequestCommand
    {
        get => (ICommand)GetValue(AcceptTripRequestCommandProperty);
        set => SetValue(AcceptTripRequestCommandProperty, value);
    }
    
    public static readonly BindableProperty RejectTripRequestCommandProperty =
        BindableProperty.Create(nameof(RejectTripRequestCommand), typeof(ICommand), typeof(TripRequestView));
    
    public ICommand RejectTripRequestCommand
    {
        get => (ICommand)GetValue(RejectTripRequestCommandProperty);
        set => SetValue(RejectTripRequestCommandProperty, value);
    }
}
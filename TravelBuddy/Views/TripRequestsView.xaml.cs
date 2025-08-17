using System.Collections.ObjectModel;
using System.Windows.Input;
using TravelBuddy.Models.DTOs;

namespace TravelBuddy.Views;

public partial class TripRequestsView : ContentView
{
    public TripRequestsView()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty TripRequestsProperty =
        BindableProperty.Create(nameof(TripRequests), typeof(ObservableCollection<UserTripRequest>), typeof(TripRequestsView));

    public ObservableCollection<UserTripRequest> TripRequests
    {
        get => (ObservableCollection<UserTripRequest>)GetValue(TripRequestsProperty);
        set => SetValue(TripRequestsProperty, value);
    }
    
    
    public static readonly BindableProperty AcceptTripRequestCommandProperty =
        BindableProperty.Create(nameof(AcceptTripRequestCommand), typeof(ICommand), typeof(TripRequestsView));
    
    public ICommand AcceptTripRequestCommand
    {
        get => (ICommand)GetValue(AcceptTripRequestCommandProperty);
        set => SetValue(AcceptTripRequestCommandProperty, value);
    }
    
    public static readonly BindableProperty RejectTripRequestCommandProperty =
        BindableProperty.Create(nameof(RejectTripRequestCommand), typeof(ICommand), typeof(TripRequestsView));
    
    public ICommand RejectTripRequestCommand
    {
        get => (ICommand)GetValue(RejectTripRequestCommandProperty);
        set => SetValue(RejectTripRequestCommandProperty, value);
    }
}
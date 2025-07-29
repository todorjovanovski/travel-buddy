using TravelBuddy.Models;

namespace TravelBuddy.Views;

public partial class TripView : ContentView
{
    public TripView()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty TripProperty =
        BindableProperty.Create(nameof(Trip), typeof(TripCard), typeof(TripView));

    public TripCard Trip
    {
        get => (TripCard)GetValue(TripProperty);
        set => SetValue(TripProperty, value);
    }
}
using System.Windows.Input;
using TravelBuddy.Models.DTOs;

namespace TravelBuddy.Views;

public partial class MinimalTripView : ContentView
{
    public MinimalTripView()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty TripProperty =
        BindableProperty.Create(nameof(Trip), typeof(TripCard), typeof(MinimalTripView));

    public TripCard Trip
    {
        get => (TripCard)GetValue(TripProperty);
        set => SetValue(TripProperty, value);
    }

    public static readonly BindableProperty FillColorProperty =
        BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(MinimalTripView));

    public Color FillColor
    {
        get => (Color)GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }
    
    public static readonly BindableProperty RequestToJoinTripCommandProperty = 
        BindableProperty.Create(nameof(RequestToJoinTripCommand), typeof(ICommand), typeof(MinimalTripView));

    public ICommand RequestToJoinTripCommand
    {
        get => (ICommand)GetValue(RequestToJoinTripCommandProperty);
        set => SetValue(RequestToJoinTripCommandProperty, value);
    }
}
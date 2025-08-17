using System.Windows.Input;
using TravelBuddy.Models.DTOs;

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

    public static readonly BindableProperty FillColorProperty =
        BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(TripView));

    public Color FillColor
    {
        get => (Color)GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }
    
    public static readonly BindableProperty DeleteTripCommandProperty = 
        BindableProperty.Create(nameof(DeleteTripCommand), typeof(ICommand), typeof(TripView));

    public ICommand DeleteTripCommand
    {
        get => (ICommand)GetValue(DeleteTripCommandProperty);
        set => SetValue(DeleteTripCommandProperty, value);
    }
    
    public static readonly BindableProperty ToggleRequestsCommandProperty = 
        BindableProperty.Create(nameof(ToggleRequestsCommand), typeof(ICommand), typeof(TripView));

    public ICommand ToggleRequestsCommand
    {
        get => (ICommand)GetValue(ToggleRequestsCommandProperty);
        set => SetValue(ToggleRequestsCommandProperty, value);
    }
}
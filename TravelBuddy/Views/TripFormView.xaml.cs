using System.Windows.Input;
using TravelBuddy.Models.DTOs;

namespace TravelBuddy.Views;

public partial class TripFormView : ContentView
{
    public TripFormView()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty TripFormProperty =
        BindableProperty.Create(nameof(TripForm), typeof(TripForm), typeof(TripView));

    public TripForm TripForm
    {
        get => (TripForm)GetValue(TripFormProperty);
        set => SetValue(TripFormProperty, value);
    }
    
    public static readonly BindableProperty SearchTripsCommandProperty =
        BindableProperty.Create(nameof(SearchTripsCommand), typeof(ICommand), typeof(TripView));
    
    public ICommand SearchTripsCommand
    {
        get => (ICommand)GetValue(SearchTripsCommandProperty);
        set => SetValue(SearchTripsCommandProperty, value);
    }
}
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TravelBuddy.Models;

public partial class TripForm : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;
    
    [ObservableProperty]
    private string _destination = string.Empty;

    [ObservableProperty]
    private DateTime _startDate = DateTime.Today.AddDays(1);
    
    [ObservableProperty]
    private DateTime _endDate = DateTime.Today.AddDays(7);

    [ObservableProperty] 
    private string _budget = string.Empty;
    
    [ObservableProperty] 
    private string _groupSize = string.Empty;
    
    [ObservableProperty]
    private ObservableCollection<object> _selectedActivities = [];

}
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TravelBuddy.Models;

public partial class TripCard : ObservableObject
{
    [ObservableProperty]
    private ImageSource _tripImageSource = null!;
    
    [ObservableProperty]
    private string _title = string.Empty;
    
    [ObservableProperty]
    private string _destination = string.Empty;
    
    [ObservableProperty]
    private string _date = string.Empty;
    
    [ObservableProperty]
    private string _budget = string.Empty;
    
    [ObservableProperty]
    private string _additionalInfo = string.Empty;
    
    [ObservableProperty]
    private ObservableCollection<Participant> _participants = [];
}
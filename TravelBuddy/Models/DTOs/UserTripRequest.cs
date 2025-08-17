using CommunityToolkit.Mvvm.ComponentModel;

namespace TravelBuddy.Models.DTOs;

public partial class UserTripRequest : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;
    
    [ObservableProperty] 
    private string _tripId = string.Empty;
    
    [ObservableProperty]
    private string _userId = string.Empty;
    
    [ObservableProperty]
    private ImageSource _profileImageSource = null!;
    
    [ObservableProperty]
    private string _nameAndAge = string.Empty;
    
    [ObservableProperty]
    private string _location = string.Empty;
}
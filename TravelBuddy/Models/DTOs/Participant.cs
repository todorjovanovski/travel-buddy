using CommunityToolkit.Mvvm.ComponentModel;

namespace TravelBuddy.Models.DTOs;

public partial class Participant : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;
    
    [ObservableProperty]
    private ImageSource _profileImageSource = null!;
}
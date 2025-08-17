using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TravelBuddy.Models.DTOs;

public partial class TripCard : ObservableObject
{
    [ObservableProperty]
    private string _tripId = string.Empty;
    
    [ObservableProperty]
    private string _chatId = string.Empty;
    
    [ObservableProperty]
    private ImageSource _tripImageSource = null!;
    
    [ObservableProperty]
    private User _tripOwner = null!;
    
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
    
    [ObservableProperty]
    private ObservableCollection<UserTripRequest> _tripRequests = [];
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsRequestsCountVisible))]
    private int _pendingRequests;

    [ObservableProperty] 
    private bool _isCurrentUserTripOwner;

    public bool IsRequestsCountVisible => PendingRequests > 0;
    public ImageSource OwnerProfilePhoto => TripOwner.ProfilePhotos.FirstOrDefault()?.Url ?? ImageSource.FromFile("participant");
}
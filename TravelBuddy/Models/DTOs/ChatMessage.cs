using CommunityToolkit.Mvvm.ComponentModel;

namespace TravelBuddy.Models.DTOs;

public partial class ChatMessage : ObservableObject
{
    [ObservableProperty]
    private string _messageText = string.Empty;
    
    [ObservableProperty]
    private string _senderId = string.Empty;
    
    [ObservableProperty]
    private string _currentUserId =  string.Empty;
    
    [ObservableProperty]
    private DateTime _dateSent;
}
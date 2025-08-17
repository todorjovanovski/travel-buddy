

namespace TravelBuddy.Models;

public class Chat
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TripId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<Message> Messages { get; set; } = [];
    public Message? LastMessage { get; set; }
}
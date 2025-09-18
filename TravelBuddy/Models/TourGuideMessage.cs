using OpenAI.Chat;

namespace TravelBuddy.Models;

public class TourGuideMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ChatId { get; set; } = string.Empty;
    public DateTime Time { get; set; } = DateTime.UtcNow;
    public string SenderId { get; set; } = string.Empty;
    public string ChatMessage { get; set; } = string.Empty;
}
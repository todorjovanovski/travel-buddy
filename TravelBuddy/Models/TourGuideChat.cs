namespace TravelBuddy.Models;

public class TourGuideChat
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TripId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<TourGuideMessage> Messages { get; set; } = [];
}
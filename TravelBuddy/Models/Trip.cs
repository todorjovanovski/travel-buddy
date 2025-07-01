namespace TravelBuddy.Models;

public class Trip
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid OwnerId { get; set; } = Guid.Empty;
    public Location Destination { get; set; } = new();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Activity> Activities { get; set; } = [];
    public List<Guid> ParticipantIds { get; set; } = [];
    public TripStatus Status { get; set; }
}
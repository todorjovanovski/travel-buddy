namespace TravelBuddy.Models;

public class Trip
{
    public Guid Id { get; set; } = Guid.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public Location Destination { get; set; } = new();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Activity> Activities { get; set; } = [];
    public Budget Budget { get; set; } = Budget.Undefined;
    public GroupSize GroupSize { get; set; } = GroupSize.Undefined;
    public string AdditionalInfo { get; set; } = string.Empty;
    public List<string> ParticipantIds { get; set; } = [];
    public TripStatus Status { get; set; } = TripStatus.Active;
}
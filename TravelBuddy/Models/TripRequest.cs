using TravelBuddy.Models.Enums;

namespace TravelBuddy.Models;

public class TripRequest
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TripOwnerId { get; set; } = string.Empty;
    public string TripId { get; set; } = string.Empty;
    public User RequestingUser { get; set; } = null!;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
}
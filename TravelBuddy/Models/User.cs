namespace TravelBuddy.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
    public string Bio { get; set; } = string.Empty;
    public Location Location { get; set; } = new();
    public List<Activity> FavoriteActivities { get; set; } = [];
    public List<ProfilePhoto> ProfilePhotos { get; set; } = [];
    public int Age => (int) DateTime.UtcNow.Subtract(Birthdate).TotalDays / 365;
}
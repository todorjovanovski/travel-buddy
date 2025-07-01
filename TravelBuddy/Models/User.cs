namespace TravelBuddy.Models;

public class User
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Bio { get; set; } = string.Empty;
    public Location Location { get; set; } = new();
    public string FavoriteActivity { get; set; } = string.Empty;
}
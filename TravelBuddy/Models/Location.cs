namespace TravelBuddy.Models;

public class Location
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{City}, {Country}";
    }
}
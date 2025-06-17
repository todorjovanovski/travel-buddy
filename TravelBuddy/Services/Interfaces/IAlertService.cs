namespace TravelBuddy.Services.Interfaces;

public interface IAlertService
{
    public Task AlertAsync(string title, string message, string cancel);
    public Task<bool> AlertAsync(string title, string message, string accept, string cancel);
}
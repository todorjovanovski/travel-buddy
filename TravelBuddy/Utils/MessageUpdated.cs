namespace TravelBuddy.Utils;

public static class MessageUpdated
{
    public static event EventHandler? MessageUpdatedEvent;
    
    public static void Raise(object? sender, EventArgs e)
    {
        MessageUpdatedEvent?.Invoke(sender, e);
    }
}
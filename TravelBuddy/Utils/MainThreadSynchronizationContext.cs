namespace TravelBuddy.Utils;

public class MainThreadSynchronizationContext : SynchronizationContext
{
    public override void Post(SendOrPostCallback d, object? state)
    {
        MainThread.BeginInvokeOnMainThread(() => d(state));
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        if (MainThread.IsMainThread) d(state);
        else MainThread.InvokeOnMainThreadAsync(() => d(state)).Wait();
    }
}

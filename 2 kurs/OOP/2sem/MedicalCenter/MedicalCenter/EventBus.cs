using System;

public static class EventBus
{
    public static event Action? ServicesUpdated;

    public static void RaiseServicesUpdated()
    {
        ServicesUpdated?.Invoke();
    }
}

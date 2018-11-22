namespace Improbable.Gdk.Subscriptions
{
    public interface ISubscriptionAvailabilityHandler
    {
        void OnAvailable();
        void OnUnavailable();
    }
}

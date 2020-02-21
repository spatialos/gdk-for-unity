namespace Improbable.Gdk.Subscriptions
{
    public interface IRequireable
    {
        bool IsValid { get; set; }
        void RemoveAllCallbacks();
    }
}

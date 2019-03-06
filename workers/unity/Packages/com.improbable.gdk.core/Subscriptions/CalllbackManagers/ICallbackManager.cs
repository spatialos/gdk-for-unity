namespace Improbable.Gdk.Subscriptions
{
    internal interface ICallbackManager
    {
        void InvokeCallbacks();
        bool UnregisterCallback(ulong callbackKey);
    }
}

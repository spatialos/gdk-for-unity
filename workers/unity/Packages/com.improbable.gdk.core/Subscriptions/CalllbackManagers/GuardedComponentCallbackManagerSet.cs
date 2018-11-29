namespace Improbable.Gdk.Subscriptions
{
    internal class GuardedComponentCallbackManagerSet<TIndex, TManager> : GuardedCallbackManagerSet<TIndex, TManager>
        where TManager : ICallbackManager
    {
        public void InvokeCallbacks()
        {
            foreach (var manager in GetManagers())
            {
                manager.InvokeCallbacks();
            }
        }
    }
}

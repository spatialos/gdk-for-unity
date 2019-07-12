namespace Improbable.Gdk.Subscriptions
{
    internal class GuardedAuthorityCallbackManagerSet<TIndex, TManager> : GuardedCallbackManagerSet<TIndex, TManager>
        where TManager : IAuthorityCallbackManager
    {
        public void InvokeLossImminentCallbacks()
        {
            foreach (var manager in GetManagers())
            {
                manager.InvokeLossImminentCallbacks();
            }
        }
    }
}

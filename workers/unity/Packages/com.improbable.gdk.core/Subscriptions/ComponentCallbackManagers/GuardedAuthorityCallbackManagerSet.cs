using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    internal class GuardedAuthorityCallbackManagerSet<TIndex, TManager> : GuardedCallbackManagerSet<TIndex, TManager>
        where TManager : IAuthorityCallbackManager
    {
        public void InvokeCallbacks(ComponentUpdateSystem componentUpdateSystem)
        {
            foreach (var manager in GetManagers())
            {
                manager.InvokeCallbacks(componentUpdateSystem);
            }
        }

        public void InvokeLossImminentCallbacks(ComponentUpdateSystem componentUpdateSystem)
        {
            foreach (var manager in GetManagers())
            {
                manager.InvokeLossImminentCallbacks(componentUpdateSystem);
            }
        }
    }
}

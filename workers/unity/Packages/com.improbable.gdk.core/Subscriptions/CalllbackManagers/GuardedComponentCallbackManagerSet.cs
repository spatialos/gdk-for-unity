using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    internal class GuardedComponentCallbackManagerSet<TIndex, TManager> : GuardedCallbackManagerSet<TIndex, TManager>
        where TManager : IComponentCallbackManager
    {
        public void InvokeCallbacks(ComponentUpdateSystem componentUpdateSystem)
        {
            foreach (var manager in GetManagers())
            {
                manager.InvokeCallbacks(componentUpdateSystem);
            }
        }
    }
}

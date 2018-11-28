using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    internal class GuardedRequestCallbackManagerSet<TIndex, TManager> : GuardedCallbackManagerSet<TIndex, TManager>
        where TManager : ICommandCallbackManager
    {
        public void InvokeCallbacks(CommandSystem commandSystem)
        {
            foreach (var manager in GetManagers())
            {
                manager.InvokeCallbacks(commandSystem);
            }
        }
    }
}

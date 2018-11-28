using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    public interface ICommandCallbackManager
    {
        void InvokeCallbacks(CommandSystem commandSystem);
        bool UnregisterCallback(ulong callbackKey);
    }
}

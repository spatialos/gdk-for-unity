using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    public interface ICommandResponseCallbackManager
    {
        void InvokeCallbacks(CommandSystem commandSystem);
    }
}

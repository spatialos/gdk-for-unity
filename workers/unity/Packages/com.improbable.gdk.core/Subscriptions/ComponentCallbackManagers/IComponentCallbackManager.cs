using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    internal interface IComponentCallbackManager
    {
        void InvokeCallbacks(ComponentUpdateSystem componentUpdateSystem);
        bool UnregisterCallback(ulong callbackKey);
    }
}

using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    internal interface IAuthorityCallbackManager : IComponentCallbackManager
    {
        void InvokeLossImminentCallbacks(ComponentUpdateSystem componentUpdateSystem);
    }
}

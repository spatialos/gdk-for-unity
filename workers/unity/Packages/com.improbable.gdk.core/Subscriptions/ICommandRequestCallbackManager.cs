using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public interface ICommandRequestCallbackManager
    {
        EntityArchetypeQuery Query { get; }
        void InvokeCallbacks(ComponentGroup group, ComponentSystemBase system);
    }
}

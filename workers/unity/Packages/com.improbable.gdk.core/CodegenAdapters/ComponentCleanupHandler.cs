using Unity.Entities;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public abstract class ComponentCleanupHandler
    {
        public abstract ComponentType[] CleanUpComponentTypes { get; }
        public abstract ComponentType ComponentUpdateType { get; }

        public abstract void CleanupUpdates(ComponentGroup updateGroup, ref EntityCommandBuffer buffer);
    }
}

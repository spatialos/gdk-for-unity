using Unity.Entities;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public abstract class ComponentCleanupHandler
    {
        public abstract ComponentType[] CleanUpComponentTypes { get; }
        public abstract ComponentType[] EventComponentTypes { get; }
        public abstract ComponentType ComponentUpdateType { get; }
        public abstract ComponentType AuthorityChangesType { get; }
        public abstract ComponentType[] CommandReactiveTypes { get; }

        public abstract void CleanupUpdates(ComponentGroup updateGroup, ref EntityCommandBuffer buffer);
        public abstract void CleanupAuthChanges(ComponentGroup authorityChangesGroup, ref EntityCommandBuffer buffer);
        public abstract void CleanupEvents(ComponentGroup[] eventGroups, ref EntityCommandBuffer buffer);
        public abstract void CleanupCommands(ComponentGroup[] commandCleanupGroups, ref EntityCommandBuffer buffer);
    }
}

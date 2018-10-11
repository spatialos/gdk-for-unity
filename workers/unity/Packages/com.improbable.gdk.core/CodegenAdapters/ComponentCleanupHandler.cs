using Unity.Entities;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public abstract class ComponentCleanupHandler
    {
        public abstract EntityArchetypeQuery CleanupArchetypeQuery { get; }

        public abstract void CleanComponents(ComponentGroup group, ComponentSystemBase system,
            EntityCommandBuffer buffer);
    }
}

using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    public abstract class ComponentCleanupHandler
    {
        public abstract EntityQueryDesc CleanupArchetypeQuery { get; }

        public abstract void CleanComponents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
            EntityCommandBuffer buffer);
    }
}

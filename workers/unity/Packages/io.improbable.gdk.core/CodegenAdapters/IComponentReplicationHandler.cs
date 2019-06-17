using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public interface IComponentReplicationHandler
    {
        uint ComponentId { get; }
        EntityQueryDesc ComponentUpdateQuery { get; }

        void SendUpdates(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
            EntityManager entityManager, ComponentUpdateSystem componentUpdateSystem);
    }
}

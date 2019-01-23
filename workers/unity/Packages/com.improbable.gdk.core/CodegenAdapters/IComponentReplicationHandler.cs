using Unity.Entities;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public interface IComponentReplicationHandler
    {
        uint ComponentId { get; }
        EntityArchetypeQuery ComponentUpdateQuery { get; }

        void SendUpdates(ComponentGroup replicationGroup, ComponentSystemBase system,
            EntityManager entityManager, ComponentUpdateSystem componentUpdateSystem);
    }
}

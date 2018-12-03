using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandSenderComponentManager
    {
        void AddComponents(Entity entity, EntityManager entityManager, World world);
        void RemoveComponents(EntityId entity, EntityManager entityManager, World world);
        void Clean(World world);
    }
}

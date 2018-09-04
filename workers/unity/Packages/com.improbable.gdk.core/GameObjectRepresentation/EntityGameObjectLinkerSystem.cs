using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    [DisableAutoCreation]
    public class EntityGameObjectLinkerSystem : ComponentSystem
    {
        public EntityGameObjectLinker Linker;

        protected override void OnCreateManager(int capacity)
        {
            Linker = new EntityGameObjectLinker(World, World.GetExistingManager<WorkerSystem>().LogDispatcher);
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}

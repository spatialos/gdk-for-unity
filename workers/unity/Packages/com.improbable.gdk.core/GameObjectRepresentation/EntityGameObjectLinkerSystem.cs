using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public class EntityGameObjectLinkerSystem : ComponentSystem
    {
        public EntityGameObjectLinker Linker;

        protected override void OnCreateManager(int capacity)
        {
            Linker = new EntityGameObjectLinker(World, Worker.GetWorkerFromWorld(World).LogDispatcher);
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}

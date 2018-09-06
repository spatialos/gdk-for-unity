using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    [DisableAutoCreation]
    public class EntityGameObjectLinkerSystem : ComponentSystem
    {
        public EntityGameObjectLinker Linker;

        [Inject] private WorkerSystem worker;

        protected override void OnCreateManager(int capacity)
        {
            Linker = new EntityGameObjectLinker(World, worker);
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}

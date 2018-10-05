using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Represents an <see cref="EntityGameObjectLinker"/> in the ECS context.
    /// </summary>
    /// <remarks>
    ///    This system will not tick.
    /// </remarks>
    [DisableAutoCreation]
    public class EntityGameObjectLinkerSystem : ComponentSystem
    {
        public EntityGameObjectLinker Linker;

        [Inject] private WorkerSystem worker;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            Linker = new EntityGameObjectLinker(World, worker);
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}

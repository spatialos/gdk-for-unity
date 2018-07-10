using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class EntityGameObjectLinker
    {
        private readonly World world;

        public EntityGameObjectLinker(World world)
        {
            this.world = world;
        }

        public void LinkGameObjectToEntity(GameObject gameObject, Entity entity, long spatialEntityId,
            ViewCommandBuffer viewCommandBuffer)
        {
            foreach (var component in gameObject.GetComponents<Component>())
            {
                viewCommandBuffer.AddComponent(entity, component.GetType(), component);
            }

            var spatialOSComponent = gameObject.AddComponent<SpatialOSComponent>();
            spatialOSComponent.Entity = entity;
            spatialOSComponent.SpatialEntityId = spatialEntityId;
            spatialOSComponent.World = world;
        }
    }
}

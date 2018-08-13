using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class EntityGameObjectLinker
    {
        private readonly World world;
        private readonly MutableView view;
        private readonly HashSet<Type> gameObjectComponentTypes = new HashSet<Type>();

        public EntityGameObjectLinker(World world, MutableView view)
        {
            this.world = world;
            this.view = view;
        }

        public void LinkGameObjectToEntity(GameObject gameObject, Entity entity, long spatialEntityId,
            ViewCommandBuffer viewCommandBuffer)
        {
            gameObjectComponentTypes.Clear();
            foreach (var component in gameObject.GetComponents<Component>())
            {
                var componentType = component.GetType();
                if (gameObjectComponentTypes.Contains(componentType))
                {
                    view.LogDispatcher.HandleLog(LogType.Warning, new LogEvent(
                            "GameObject contains multiple instances of the same component type. Only one instance of each component type will be added to the corresponding ECS entity.")
                        .WithField("EntityId", spatialEntityId)
                        .WithField("ComponentType", componentType));
                    continue;
                }

                gameObjectComponentTypes.Add(componentType);
                viewCommandBuffer.AddComponent(entity, component.GetType(), component);
            }

            var spatialOSComponent = gameObject.AddComponent<SpatialOSComponent>();
            spatialOSComponent.Entity = entity;
            spatialOSComponent.SpatialEntityId = spatialEntityId;
            spatialOSComponent.World = world;
        }
    }
}

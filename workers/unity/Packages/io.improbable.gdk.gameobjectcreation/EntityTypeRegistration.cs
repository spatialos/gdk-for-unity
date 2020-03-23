using System;
using System.Linq;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation
{
    public readonly struct EntityTypeRegistration
    {
        public readonly Action<SpatialOSEntity, EntityGameObjectLinker> CreateGameObject;
        public readonly ComponentType[] ComponentTypes;

        public EntityTypeRegistration(Action<SpatialOSEntity, EntityGameObjectLinker> createCallback,
            params Type[] componentTypes)
        {
            CreateGameObject = createCallback;
            ComponentTypes = componentTypes
                .Select(type => new ComponentType(type, ComponentType.AccessMode.ReadOnly))
                .ToArray();
        }
    }
}

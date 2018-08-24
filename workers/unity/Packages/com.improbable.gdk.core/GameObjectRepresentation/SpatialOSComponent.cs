using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public class SpatialOSComponent : MonoBehaviour
    {
        public EntityId SpatialEntityId;
        public Entity Entity;
        public World World;
    }
}

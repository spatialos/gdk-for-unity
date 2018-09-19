using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation
{
    public class SpatialOSComponent : MonoBehaviour
    {
        public EntityId SpatialEntityId;
        public Entity Entity;
        public World World;
        public WorkerSystem Worker;

        public bool IsEntityOnThisWorker(EntityId entityId)
        {
            return Worker.EntityIdToEntity.ContainsKey(entityId);
        }
    }
}

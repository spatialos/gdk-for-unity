using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public interface IEntityGameObjectCreator
    {
        GameObject GetGameObjectForEntityAdded(SpatialOSEntity entity, WorkerSystem worker);
        void OnEntityRemoved(SpatialOSEntity entity, WorkerSystem worker, GameObject linkedGameObject);
    }
}

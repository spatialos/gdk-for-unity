using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public interface IEntityGameObjectCreator
    {
        GameObject GetGameObjectForEntityAdded(SpatialOSEntity entity, WorkerSystem worker);
        void OnEntityGameObjectRemoved(SpatialOSEntity entity, WorkerSystem worker, GameObject linkedGameObject);
    }
}

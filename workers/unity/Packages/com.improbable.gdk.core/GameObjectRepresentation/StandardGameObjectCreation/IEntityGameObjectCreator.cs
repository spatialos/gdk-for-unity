using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public interface IEntityGameObjectCreator
    {
        GameObject CreateGameObjectForEntityAdded(SpatialOSEntity entity, WorkerSystem worker);
        void OnEntityRemoved(SpatialOSEntity entity, WorkerSystem worker, GameObject linkedGameObject);
    }
}

using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public interface IEntityGameObjectCreator
    {
        GameObject CreateGameObjectForEntity(SpatialOSEntity entity, Worker worker);
    }
}

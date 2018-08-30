using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;

namespace Packages.com.improbable.gdk.core.GameObjectRepresentation.GameObjectCreation
{
    public interface IEntityGameObjectCreator
    {
        GameObject CreateGameObjectForEntity(SpatialOSEntity entity, Worker worker);
    }
}

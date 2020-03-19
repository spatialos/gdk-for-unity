using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation
{
    public struct GameObjectInitializationComponent : ISystemStateComponentData
    {
        public EntityId EntityId;
    }
}

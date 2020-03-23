using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation
{
    public struct GameObjectInitSystemStateComponent : ISystemStateComponentData
    {
        public EntityId EntityId;
    }
}

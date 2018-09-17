using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct LastPositionSentData : IComponentData
    {
        public Position.Component Position;
        public float TimeSinceLastUpdate;
    }
}

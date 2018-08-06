using Improbable.Gdk.Core;
using Unity.Entities;

namespace Playground
{
    public struct LocalInput : IComponentData
    {
        public float Horizontal;
        public float Vertical;
        public float RightStickHorizontal;
        public float RightStickVertical;
        public float CameraDistance;
        public BlittableBool Running;
        public BlittableBool ShootSmall;
        public BlittableBool ShootLarge;
    }
}

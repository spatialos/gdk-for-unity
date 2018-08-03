using Improbable.Gdk.Core;
using Unity.Entities;

namespace Playground
{
    public struct LocalInput : IComponentData
    {
        public BlittableBool ShootSmall;
        public BlittableBool ShootLarge;
    }
}

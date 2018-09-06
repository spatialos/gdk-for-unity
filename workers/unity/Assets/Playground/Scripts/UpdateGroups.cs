using Improbable.Gdk.Core;
using Unity.Entities;

namespace Playground
{
    [UpdateAfter(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateBefore(typeof(SpatialOSReceiveGroup.GameObjectReceiveGroup))]
    public class EntityInitialization
    { 
    }
}

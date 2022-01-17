using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
    [UpdateBefore(typeof(RequireLifecycleGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [DisableAutoCreation]
    public class GameObjectInitializationGroup : ComponentSystemGroup
    {
    }
}

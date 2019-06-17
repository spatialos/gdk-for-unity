using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [DisableAutoCreation]
    public class RequireLifecycleGroup : ComponentSystemGroup
    {
    }
}

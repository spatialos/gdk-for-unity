using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
    [UpdateAfter(typeof(RequireLifecycleGroup))]
    public class GameObjectInitializationGroup : ComponentSystemGroup
    {
    }
}

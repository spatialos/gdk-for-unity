using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;

namespace Improbable.Gdk.Core
{
    [UpdateBefore(typeof(Update))]
    public class SpatialOSReceiveGroup
    {
        [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
        public class InternalSpatialOSReceiveGroup
        {
        }

        [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
        [UpdateAfter(typeof(InternalSpatialOSReceiveGroup))]
        public class GameObjectInitializationGroup
        {
        }

        [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
        [UpdateAfter(typeof(GameObjectInitializationGroup))]
        public class GameObjectReceiveGroup
        {
        }
    }

    [UpdateAfter(typeof(PostLateUpdate))]
    public class SpatialOSSendGroup
    {
        [UpdateInGroup(typeof(SpatialOSSendGroup))]
        public class InternalSpatialOSSendGroup
        {
        }

        [UpdateInGroup(typeof(SpatialOSSendGroup))]
        [UpdateAfter(typeof(InternalSpatialOSSendGroup))]
        public class CustomSpatialOSSendGroup
        {
        }

        [UpdateInGroup(typeof(SpatialOSSendGroup))]
        [UpdateAfter(typeof(CustomSpatialOSSendGroup))]
        public class InternalSpatialOSCleanGroup
        {
        }
    }

    [UpdateAfter(typeof(SpatialOSReceiveGroup))]
    [UpdateBefore(typeof(SpatialOSSendGroup))]
    public class SpatialOSUpdateGroup
    {
    }
}

using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;

namespace Improbable.Gdk.Core
{
    [UpdateBefore(typeof(Update))]
    public class SpatialOSReceiveGroup : ComponentSystemGroup
    {
        [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
        public class InternalSpatialOSReceiveGroup : ComponentSystemGroup
        {
        }
    }

    [UpdateAfter(typeof(PostLateUpdate))]
    public class SpatialOSSendGroup : ComponentSystemGroup
    {
        [UpdateInGroup(typeof(SpatialOSSendGroup))]
        public class CustomSpatialOSSendGroup : ComponentSystemGroup
        {
        }

        [UpdateInGroup(typeof(SpatialOSSendGroup))]
        [UpdateAfter(typeof(CustomSpatialOSSendGroup))]
        public class InternalSpatialOSSendGroup : ComponentSystemGroup
        {
        }

        [UpdateInGroup(typeof(SpatialOSSendGroup))]
        [UpdateAfter(typeof(CustomSpatialOSSendGroup))]
        public class InternalSpatialOSCleanGroup : ComponentSystemGroup
        {
        }
    }

    [UpdateAfter(typeof(SpatialOSReceiveGroup))]
    [UpdateBefore(typeof(SpatialOSSendGroup))]
    public class SpatialOSUpdateGroup : ComponentSystemGroup
    {
    }
}

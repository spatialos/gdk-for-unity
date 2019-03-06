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
    }

    [UpdateAfter(typeof(PostLateUpdate))]
    public class SpatialOSSendGroup
    {
        [UpdateInGroup(typeof(SpatialOSSendGroup))]
        public class CustomSpatialOSSendGroup
        {
        }

        [UpdateInGroup(typeof(SpatialOSSendGroup))]
        [UpdateAfter(typeof(CustomSpatialOSSendGroup))]
        public class InternalSpatialOSSendGroup
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

using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class SpatialOSReceiveGroup : ComponentSystemGroup
    {
        [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
        public class InternalSpatialOSReceiveGroup : ComponentSystemGroup
        {
        }
    }

    [UpdateInGroup(typeof(PresentationSystemGroup))]
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
    }

    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateAfter(typeof(SpatialOSSendGroup))]
    public class InternalSpatialOSCleanGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class SpatialOSUpdateGroup : ComponentSystemGroup
    {
    }

    [PlayerLoopUtils.UpdateInSubSystemAttribute(typeof(FixedUpdate))]
    public class FixedUpdateSystemGroup : ComponentSystemGroup
    {
    }
}

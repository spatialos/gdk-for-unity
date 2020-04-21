using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [DisableAutoCreation]
    public class SpatialOSReceiveGroup : ComponentSystemGroup
    {
        [UpdateInGroup(typeof(SpatialOSReceiveGroup))]
        [DisableAutoCreation]
        public class InternalSpatialOSReceiveGroup : ComponentSystemGroup
        {
        }
    }

    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [DisableAutoCreation]
    public class SpatialOSSendGroup : ComponentSystemGroup
    {
        [UpdateInGroup(typeof(SpatialOSSendGroup))]
        [UpdateBefore(typeof(InternalSpatialOSSendGroup))]
        [DisableAutoCreation]
        public class CustomSpatialOSSendGroup : ComponentSystemGroup
        {
        }

        [UpdateInGroup(typeof(SpatialOSSendGroup))]
        [DisableAutoCreation]
        public class InternalSpatialOSSendGroup : ComponentSystemGroup
        {
        }
    }

    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateAfter(typeof(SpatialOSSendGroup))]
    [DisableAutoCreation]
    public class InternalSpatialOSCleanGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [DisableAutoCreation]
    public class SpatialOSUpdateGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    public class FixedUpdateSystemGroup : ComponentSystemGroup
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            FixedRateUtils.EnableFixedRateWithCatchUp(this, UnityEngine.Time.fixedDeltaTime);
        }
    }
}

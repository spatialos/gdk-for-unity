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

    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    [DisableAutoCreation]
    public class SpatialOSReplicationGroup : ComponentSystemGroup
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

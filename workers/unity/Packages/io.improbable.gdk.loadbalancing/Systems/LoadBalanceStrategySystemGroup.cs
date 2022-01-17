using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.LoadBalancing
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class LoadBalanceStrategySystemGroup : ComponentSystemGroup
    {
    }
}
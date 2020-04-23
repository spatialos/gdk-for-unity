using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(HandlePlayerHeartbeatResponseSystem))]
    public class PlayerHeartbeatInitializationSystem : ComponentSystem
    {
        private EntityQuery initializerQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            var entityQueryDesc = new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<PlayerHeartbeatServer.Component>(),
                },
                None = new[]
                {
                    ComponentType.ReadWrite<HeartbeatData>(),
                },
            };

            initializerQuery = GetEntityQuery(entityQueryDesc);
        }

        protected override void OnUpdate()
        {
            EntityManager.AddComponent<HeartbeatData>(initializerQuery);
        }
    }
}

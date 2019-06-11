using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateAfter(typeof(DefaultUpdateLatestTransformSystem))]
    [UpdateAfter(typeof(RateLimitedTransformSendSystem))]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class RateLimitedPositionSendSystem : ComponentSystem
    {
        private EntityQuery positionGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            positionGroup = GetEntityQuery(
                ComponentType.ReadWrite<LastPositionSentData>(),
                ComponentType.ReadWrite<Position.Component>(),
                ComponentType.ReadOnly<TransformInternal.Component>(),
                ComponentType.ReadOnly<RateLimitedSendConfig>(),
                ComponentType.ReadOnly<Position.ComponentAuthority>()
            );
            positionGroup.SetFilter(Position.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            Entities.With(positionGroup).ForEach((RateLimitedSendConfig config, ref Position.Component position,
                ref TransformInternal.Component transformInternal, ref LastPositionSentData lastPositionSent) =>
            {
                lastPositionSent.TimeSinceLastUpdate += Time.deltaTime;

                if (lastPositionSent.TimeSinceLastUpdate < 1.0f / config.MaxPositionUpdateRateHz)
                {
                    return;
                }

                var coords = transformInternal.Location.ToCoordinates();
                if (!TransformUtils.HasChanged(coords, position.Coords))
                {
                    return;
                }

                position.Coords = coords;

                lastPositionSent.TimeSinceLastUpdate = 0.0f;
                lastPositionSent.Position = position;
            });
        }
    }
}

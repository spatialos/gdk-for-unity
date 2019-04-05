using Improbable.Gdk.Core;
using Improbable.Transform;
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
        private ComponentGroup positionGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            positionGroup = GetComponentGroup(
                ComponentType.Create<LastPositionSentData>(),
                ComponentType.Create<Position.Component>(),
                ComponentType.ReadOnly<TransformInternal.Component>(),
                ComponentType.ReadOnly<RateLimitedSendConfig>(),
                ComponentType.ReadOnly<Position.ComponentAuthority>()
            );
            positionGroup.SetFilter(Position.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            var rateLimitedConfigArray = positionGroup.GetSharedComponentDataArray<RateLimitedSendConfig>();
            var positionArray = positionGroup.GetComponentDataArray<Position.Component>();
            var transformArray = positionGroup.GetComponentDataArray<TransformInternal.Component>();
            var lastSentPositionArray = positionGroup.GetComponentDataArray<LastPositionSentData>();

            for (int i = 0; i < positionArray.Length; ++i)
            {
                var position = positionArray[i];

                var lastPositionSent = lastSentPositionArray[i];
                lastPositionSent.TimeSinceLastUpdate += Time.deltaTime;
                lastSentPositionArray[i] = lastPositionSent;

                if (lastPositionSent.TimeSinceLastUpdate <
                    1.0f / rateLimitedConfigArray[i].MaxPositionUpdateRateHz)
                {
                    continue;
                }

                var transform = transformArray[i];

                var coords = transform.Location.ToCoordinates();

                if (!TransformUtils.HasChanged(coords, position.Coords))
                {
                    continue;
                }

                position.Coords = coords;
                positionArray[i] = position;

                lastPositionSent.TimeSinceLastUpdate = 0.0f;
                lastPositionSent.Position = position;
                lastSentPositionArray[i] = lastPositionSent;
            }
        }
    }
}

using Generated.Improbable;
using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public class PositionSendSystem : CustomSpatialOSSendSystem<SpatialOSPosition>
    {
        private struct PositionData
        {
            public readonly int Length;
            public ComponentDataArray<SpatialOSPosition> Position;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSPosition>> PositionAuthority;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
        }

        [Inject] private PositionData positionData;

        // Number of position sends per second.
        private const float SendRateHz = 1.0f;

        private float timeSinceLastSend = 0.0f;

        protected override void OnUpdate()
        {
            // Send update at SendRateHz.
            timeSinceLastSend += Time.deltaTime;
            if (timeSinceLastSend < 1.0f / SendRateHz)
            {
                return;
            }

            timeSinceLastSend = 0.0f;

            for (var i = 0; i < positionData.Length; i++)
            {
                var component = positionData.Position[i];

                if (component.DirtyBit != true)
                {
                    continue;
                }

                var entityId = positionData.SpatialEntityIds[i].EntityId;

                var update = new SchemaComponentUpdate(component.ComponentId);
                Generated.Improbable.SpatialOSPosition.Serialization.Serialize(component, update.GetFields());
                worker.Connection.SendComponentUpdate(entityId, new ComponentUpdate(update));

                component.DirtyBit = false;
                positionData.Position[i] = component;
            }
        }
    }
}

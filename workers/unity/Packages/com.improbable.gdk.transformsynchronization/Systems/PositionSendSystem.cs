using Generated.Improbable;
using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    public class PositionSendSystem : CustomSpatialOSSendSystem<Position.Component>
    {
        private struct PositionData
        {
            public readonly int Length;
            public ComponentDataArray<Position.Component> Position;
            public ComponentDataArray<LastPositionSentData> LastPositionSent;
            [ReadOnly] public ComponentDataArray<Authoritative<Position.Component>> PositionAuthority;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
        }

        [Inject] private PositionData positionData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < positionData.Length; i++)
            {
                var position = positionData.Position[i];

                if (position.DirtyBit != true)
                {
                    continue;
                }

                var lastPositionSent = positionData.LastPositionSent[i];
                lastPositionSent.TimeSinceLastUpdate += Time.deltaTime;
                positionData.LastPositionSent[i] = lastPositionSent;

                if (lastPositionSent.TimeSinceLastUpdate <
                    1.0f / TransformSynchronizationConfig.MaxPositionUpdateRateHz)
                {
                    continue;
                }

                var squareDistance =
                    TransformUtils.SquareDistance(lastPositionSent.Position.Coords, positionData.Position[i].Coords);

                if (squareDistance == 0.0f)
                {
                    continue;
                }

                if (lastPositionSent.TimeSinceLastUpdate <
                    TransformSynchronizationConfig.MaxTimeForStalePositionWithoutUpdateS)
                {
                    if (squareDistance < TransformSynchronizationConfig.MaxSquarePositionChangeWithoutUpdate)
                    {
                        continue;
                    }
                }

                var entityId = positionData.SpatialEntityIds[i].EntityId;

                var update = new SchemaComponentUpdate(position.ComponentId);
                Position.Serialization.SerializeUpdate(position, update);
                WorkerSystem.Connection.SendComponentUpdate(entityId, new ComponentUpdate(update));

                position.DirtyBit = false;
                positionData.Position[i] = position;

                lastPositionSent.TimeSinceLastUpdate = 0.0f;
                lastPositionSent.Position = position;
                positionData.LastPositionSent[i] = lastPositionSent;
            }
        }
    }
}

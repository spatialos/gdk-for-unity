using Generated.Improbable.Transform;
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
    public class TransformSendSystem : CustomSpatialOSSendSystem<TransformInternal.Component>
    {
        private struct TransformData
        {
            public readonly int Length;
            public ComponentDataArray<TransformInternal.Component> Transform;
            public ComponentDataArray<LastTransformSentData> LastTransformSent;
            [ReadOnly] public ComponentDataArray<Authoritative<TransformInternal.Component>> TransformAuthority;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
        }

        [Inject] private TransformData transformData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < transformData.Length; i++)
            {
                var transform = transformData.Transform[i];

                if (transform.DirtyBit != true)
                {
                    continue;
                }

                var lastTransformSent = transformData.LastTransformSent[i];
                lastTransformSent.TimeSinceLastUpdate += Time.deltaTime;
                transformData.LastTransformSent[i] = lastTransformSent;

                if (lastTransformSent.TimeSinceLastUpdate <
                    1.0f / TransformSynchronizationConfig.MaxTransformUpdateRateHz)
                {
                    continue;
                }

                // todo Need to be doing things with velocity and orientation too for this to work
                // also consider moving all this to the intermediate systems
                // var squareDisatnce =
                //     TransformUtils.SquareDisatnce(lastTransformSent.Transform.Location,
                //         transformData.Transform[i].Location);
                //
                // if (squareDisatnce == 0.0f)
                // {
                //     continue;
                // }
                //
                // if (lastTransformSent.TimeSinceLastUpdate <
                //     TransformSynchronizationConfig.MaxTimeForStalePositionWithoutUpdateS)
                // {
                //     if (squareDisatnce < TransformSynchronizationConfig.MaxSquarePositionChangeWithoutUpdate)
                //     {
                //         continue;
                //     }
                // }

                var entityId = transformData.SpatialEntityIds[i].EntityId;

                var update = new SchemaComponentUpdate(transform.ComponentId);
                TransformInternal.Serialization.SerializeUpdate(transform, update);
                WorkerSystem.Connection.SendComponentUpdate(entityId, new ComponentUpdate(update));

                transform.DirtyBit = false;
                transformData.Transform[i] = transform;

                lastTransformSent.TimeSinceLastUpdate = 0.0f;
                lastTransformSent.Transform = transform;
                transformData.LastTransformSent[i] = lastTransformSent;
            }
        }
    }
}

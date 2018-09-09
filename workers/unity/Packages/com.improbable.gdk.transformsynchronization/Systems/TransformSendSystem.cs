using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Transform = Generated.Improbable.Transform.Transform;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    public class TransformSendSystem : CustomSpatialOSSendSystem<Transform.Component>
    {
        private struct TransformData
        {
            public readonly int Length;
            public ComponentDataArray<Transform.Component> Transform;
            public ComponentDataArray<LastTransformSentData> LastTransformSent;
            [ReadOnly] public ComponentDataArray<Authoritative<Transform.Component>> TransformAuthority;
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

                // Need to be doing things with velocity and orientation too for this to work
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
                Transform.Serialization.Serialize(transform, update.GetFields());
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

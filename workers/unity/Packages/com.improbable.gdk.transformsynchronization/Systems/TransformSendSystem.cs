using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Transform = Generated.Improbable.Transform.Transform;

namespace Improbable.Gdk.TransformSynchronization
{
    public class TransformSendSystem : CustomSpatialOSSendSystem<Transform.Component>
    {
        private struct TransformData
        {
            public readonly int Length;
            public ComponentDataArray<Transform.Component> Transforms;
            [ReadOnly] public ComponentDataArray<Authoritative<Transform.Component>> TransformAuthority;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
        }

        [Inject] private TransformData transformData;

        // Number of transform sends per second.
        private const float SendRateHz = 30.0f;

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

            for (var i = 0; i < transformData.Length; i++)
            {
                var component = transformData.Transforms[i];

                if (component.DirtyBit != true)
                {
                    continue;
                }

                var entityId = transformData.SpatialEntityIds[i].EntityId;

                var update = new SchemaComponentUpdate(component.ComponentId);
                Transform.Serialization.Serialize(component,
                    update.GetFields());
                worker.Connection.SendComponentUpdate(entityId, new ComponentUpdate(update));

                component.DirtyBit = false;
                transformData.Transforms[i] = component;
            }
        }
    }
}

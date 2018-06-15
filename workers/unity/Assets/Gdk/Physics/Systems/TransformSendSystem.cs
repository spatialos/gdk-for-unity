using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public class TransformSendSystem : CustomSpatialOSSendSystem<SpatialOSTransform>
    {
        public struct TransformData
        {
            public int Length;
            public ComponentDataArray<SpatialOSTransform> Transforms;
            public ComponentDataArray<Authoritative<SpatialOSTransform>> TransformAuthority;
            public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
        }

        [Inject] private TransformData transformData;

        protected override void OnUpdate()
        {
            // Send update every other tick.
            if (World.GetExistingManager<TickSystem>().GlobalTick % 2 != 0)
            {
                return;
            }

            for (var i = 0; i < transformData.Length; i++)
            {
                var component = transformData.Transforms[i];

                if (component.DirtyBit != true)
                {
                    continue;
                }

                var entityId = transformData.SpatialEntityIds[i].EntityId;
                var update = new global::Improbable.Transform.Transform.Update();
                update.SetLocation(global::Generated.Improbable.Transform.Location.ToSpatial(component.Location));
                update.SetRotation(global::Generated.Improbable.Transform.Quaternion.ToSpatial(component.Rotation));
                update.SetTick(component.Tick);
                Generated.Improbable.Transform.Transform.Translation.SendComponentUpdate(worker.Connection, entityId,
                    update);

                component.DirtyBit = false;
                transformData.Transforms[i] = component;
            }
        }
    }
}

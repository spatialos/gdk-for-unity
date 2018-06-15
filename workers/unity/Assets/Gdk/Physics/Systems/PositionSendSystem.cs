using Generated.Improbable;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public class PositionSendSystem : CustomSpatialOSSendSystem<SpatialOSPosition>
    {
        public struct PositionData
        {
            public int Length;
            public ComponentDataArray<SpatialOSPosition> Position;
            public ComponentDataArray<Authoritative<SpatialOSPosition>> PositionAuthority;
            public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
        }

        [Inject] private PositionData positionData;

        protected override void OnUpdate()
        {
            if (World.GetExistingManager<TickSystem>().GlobalTick % 60 != 0) // Update once per second
            {
                return;
            }

            for (var i = 0; i < positionData.Length; i++)
            {
                var component = positionData.Position[i];

                if (component.DirtyBit != true)
                {
                    continue;
                }

                var entityId = positionData.SpatialEntityIds[i].EntityId;
                var update = new global::Improbable.Position.Update();
                update.SetCoords(global::Generated.Improbable.Coordinates.ToSpatial(component.Coords));
                Generated.Improbable.Position.Translation.SendComponentUpdate(worker.Connection, entityId, update);

                component.DirtyBit = false;
                positionData.Position[i] = component;
            }
        }
    }
}

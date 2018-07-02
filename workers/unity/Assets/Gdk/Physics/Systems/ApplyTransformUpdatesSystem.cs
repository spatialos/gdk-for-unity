using System.Collections.Generic;
using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class ApplyTransformUpdatesSystem : ComponentSystem
    {
        public struct TransformUpdateData
        {
            public int Length;
            public ComponentDataArray<SpatialOSTransform> Transform;
            public ComponentArray<ComponentsUpdated<SpatialOSTransformUpdate>> TransformUpdate;
            public ComponentArray<BufferedTransform> BufferedTransform;
            public ComponentDataArray<NotAuthoritative<SpatialOSTransform>> TransformAuthority;
        }

        [Inject] private TransformUpdateData transformUpdateData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < transformUpdateData.Length; i++)
            {
                var transformUpdates = transformUpdateData.TransformUpdate[i].Buffer;
                var lastTransformSnapshot = transformUpdateData.BufferedTransform[i].LastTransformSnapshot;
                var transformSnapshots = new List<SpatialOSTransform>();
                foreach (var update in transformUpdates)
                {
                    lastTransformSnapshot.Location = update.Location.HasValue
                        ? update.Location.Value
                        : lastTransformSnapshot.Location;
                    lastTransformSnapshot.Rotation = update.Rotation.HasValue
                        ? update.Rotation.Value
                        : lastTransformSnapshot.Rotation;
                    lastTransformSnapshot.Tick = update.Tick.HasValue
                        ? update.Tick.Value
                        : lastTransformSnapshot.Tick;
                    transformSnapshots.Add(lastTransformSnapshot);
                }
                transformUpdateData.BufferedTransform[i].TransformUpdates.AddRange(transformSnapshots);
                transformUpdateData.BufferedTransform[i].LastTransformSnapshot = transformUpdateData.Transform[i];
            }
        }
    }
}

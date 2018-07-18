using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class ApplyTransformUpdatesSystem : ComponentSystem
    {
        public struct TransformUpdateData
        {
            public readonly int Length;
            [ReadOnly] public ComponentArray<ComponentsUpdated<SpatialOSTransform.Update>> TransformUpdate;
            public ComponentArray<BufferedTransform> BufferedTransform;
            [ReadOnly] public ComponentDataArray<NotAuthoritative<SpatialOSTransform>> TransformAuthority;
        }

        [Inject] private TransformUpdateData transformUpdateData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < transformUpdateData.Length; i++)
            {
                var transformUpdates = transformUpdateData.TransformUpdate[i].Buffer;
                var lastTransformSnapshot = transformUpdateData.BufferedTransform[i].LastTransformSnapshot;
                foreach (var update in transformUpdates)
                {
                    if (update.Location.HasValue)
                    {
                        lastTransformSnapshot.Location = update.Location.Value;
                    }

                    if (update.Rotation.HasValue)
                    {
                        lastTransformSnapshot.Rotation = update.Rotation.Value;
                    }

                    if (update.Tick.HasValue)
                    {
                        lastTransformSnapshot.Tick = update.Tick.Value;
                    }

                    transformUpdateData.BufferedTransform[i].TransformUpdates.Add(lastTransformSnapshot);
                }

                transformUpdateData.BufferedTransform[i].LastTransformSnapshot = lastTransformSnapshot;
            }
        }
    }
}

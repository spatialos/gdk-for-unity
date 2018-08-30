using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class ApplyTransformUpdatesSystem : ComponentSystem
    {
        private struct TransformUpdateData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<Transform.ReceivedUpdates> TransformUpdate;
            public ComponentArray<BufferedTransform> BufferedTransform;
            [ReadOnly] public ComponentDataArray<NotAuthoritative<Transform.Component>> TransformAuthority;
            [ReadOnly] public ComponentDataArray<Transform.Component> Transform;
        }

        [Inject] private TransformUpdateData transformUpdateData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < transformUpdateData.Length; i++)
            {
                var transformUpdates = transformUpdateData.TransformUpdate[i].Updates;
                var bufferedTransform = transformUpdateData.BufferedTransform[i];
                var lastTransformSnapshot = bufferedTransform.LastTransformSnapshot;

                if (!bufferedTransform.IsInitialised)
                {
                    lastTransformSnapshot = transformUpdateData.Transform[i];
                    bufferedTransform.IsInitialised = true;
                }

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

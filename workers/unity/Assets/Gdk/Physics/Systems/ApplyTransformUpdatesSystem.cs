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
            public ComponentArray<ComponentsUpdated<SpatialOSTransform>> TransformUpdate;
            public ComponentArray<BufferedTransform> BufferedTransform;
            public ComponentDataArray<NotAuthoritative<SpatialOSTransform>> TransformAuthority;
        }

        [Inject] private TransformUpdateData transformUpdateData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < transformUpdateData.Length; i++)
            {
                var newUpdates = transformUpdateData.TransformUpdate[i].Buffer;
                transformUpdateData.BufferedTransform[i].TransformUpdates.AddRange(newUpdates);
            }
        }
    }
}

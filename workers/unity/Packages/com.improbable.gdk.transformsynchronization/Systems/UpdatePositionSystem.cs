using Generated.Improbable;
using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion


namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateAfter(typeof(UpdateTransformSystem))]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class UpdatePositionSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public ComponentDataArray<Position.Component> Position;
            [ReadOnly] public ComponentDataArray<TransformInternal.Component> Transform;

            [ReadOnly] public ComponentDataArray<Authoritative<Position.Component>> DenotesHasAuthority;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; ++i)
            {
                var t = data.Transform[i];
                var coords = new Coordinates
                {
                    X = t.Location.X,
                    Y = t.Location.Y,
                    Z = t.Location.Z,
                };
                var position = new Position.Component
                {
                    Coords = coords
                };

                data.Position[i] = position;
            }
        }
    }
}

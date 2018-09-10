using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(DefaultUpdateLatestTransformSystem))]
    [UpdateInGroup(typeof(FixedUpdate))]
    public class TickSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<TransformInternal.Component> Transform;
            public ComponentDataArray<TicksSinceLastTransformUpdate> TicksSinceLastUpdate;

            [ReadOnly] public ComponentDataArray<Authoritative<TransformInternal.Component>> DenotesAuthority;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; ++i)
            {
                TicksSinceLastTransformUpdate t = data.TicksSinceLastUpdate[i];
                t.NumberOfTicks += 1;
                data.TicksSinceLastUpdate[i] = t;
            }
        }
    }
}

using Improbable.Transform;
using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(FixedUpdate.PhysicsFixedUpdate))]
    public class TickSystem : ComponentSystem
    {
        private ComponentGroup transformGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            transformGroup = GetComponentGroup(
                ComponentType.Create<TicksSinceLastTransformUpdate>(),
                ComponentType.ReadOnly<TransformInternal.Component>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            var ticksSinceLastUpdateArray = transformGroup.GetComponentDataArray<TicksSinceLastTransformUpdate>();

            for (int i = 0; i < ticksSinceLastUpdateArray.Length; ++i)
            {
                var t = ticksSinceLastUpdateArray[i];
                t.NumberOfTicks += 1;
                ticksSinceLastUpdateArray[i] = t;
            }
        }
    }
}

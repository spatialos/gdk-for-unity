using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(FixedUpdateSystemGroup))]
    public class TickSystem : ComponentSystem
    {
        private ComponentGroup transformGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            transformGroup = GetComponentGroup(
                ComponentType.ReadWrite<TicksSinceLastTransformUpdate>(),
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

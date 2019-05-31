using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(FixedUpdateSystemGroup))]
    public class TickSystem : ComponentSystem
    {
        private EntityQuery transformGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            transformGroup = GetEntityQuery(
                ComponentType.ReadWrite<TicksSinceLastTransformUpdate>(),
                ComponentType.ReadOnly<TransformInternal.Component>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            Entities.With(transformGroup).ForEach((ref TicksSinceLastTransformUpdate ticksSinceLastTransformUpdate) =>
            {
                ticksSinceLastTransformUpdate.NumberOfTicks += 1;
            });
        }
    }
}

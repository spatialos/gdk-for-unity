using Improbable.Transform;
using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion


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
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>());
        }

        protected override void OnUpdate()
        {
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);

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

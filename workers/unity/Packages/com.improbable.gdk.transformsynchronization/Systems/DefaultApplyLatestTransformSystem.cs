using Improbable.Transform;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(FixedUpdate.PhysicsFixedUpdate))]
    public class DefaultApplyLatestTransformSystem : ComponentSystem
    {
        private ComponentGroup rigidbodyGroup;
        private ComponentGroup transformGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            rigidbodyGroup = GetComponentGroup(
                ComponentType.ReadOnly<Rigidbody>(),
                ComponentType.ReadOnly<TransformToSet>(),
                ComponentType.ReadOnly<SetTransformToGameObjectTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            rigidbodyGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);

            transformGroup = GetComponentGroup(
                ComponentType.Subtractive<Rigidbody>(),
                ComponentType.ReadOnly<UnityEngine.Transform>(),
                ComponentType.ReadOnly<TransformToSet>(),
                ComponentType.ReadOnly<SetTransformToGameObjectTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);
        }

        protected override void OnUpdate()
        {
            UpdateRigidbodyData();
            UpdateTransformData();
        }

        private void UpdateRigidbodyData()
        {
            var rigidbodyArray = rigidbodyGroup.GetComponentArray<Rigidbody>();
            var transformToSetArray = rigidbodyGroup.GetComponentDataArray<TransformToSet>();

            for (int i = 0; i < rigidbodyArray.Length; ++i)
            {
                var transform = transformToSetArray[i];
                var rigidbody = rigidbodyArray[i];
                rigidbody.MovePosition(transform.Position);
                rigidbody.MoveRotation(transform.Orientation);
                rigidbody.AddForce(transform.Velocity - rigidbody.velocity, ForceMode.VelocityChange);
            }
        }

        private void UpdateTransformData()
        {
            var transformArray = transformGroup.GetComponentArray<UnityEngine.Transform>();
            var transformToSetArray = transformGroup.GetComponentDataArray<TransformToSet>();

            for (int i = 0; i < transformArray.Length; ++i)
            {
                var transformToSet = transformToSetArray[i];
                var transform = transformArray[i];

                transform.localPosition = transformToSet.Position;
                transform.localRotation = transformToSet.Orientation;
            }
        }
    }
}

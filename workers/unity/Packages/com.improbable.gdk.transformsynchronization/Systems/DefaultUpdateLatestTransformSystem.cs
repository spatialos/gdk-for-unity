using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

// ReSharper disable ClassNeverInstantiated.Global

#endregion


namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class DefaultUpdateLatestTransformSystem : ComponentSystem
    {
        private ComponentGroup rigidbodyGroup;
        private ComponentGroup transformGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            rigidbodyGroup = GetComponentGroup(
                ComponentType.ReadOnly<Rigidbody>(),
                ComponentType.Create<TransformToSend>(),
                ComponentType.ReadOnly<GetTransformFromGameObjectTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>());

            transformGroup = GetComponentGroup(
                ComponentType.Subtractive<Rigidbody>(),
                ComponentType.ReadOnly<UnityEngine.Transform>(),
                ComponentType.Create<TransformToSend>(),
                ComponentType.ReadOnly<GetTransformFromGameObjectTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>());
        }

        protected override void OnUpdate()
        {
            UpdateRigidbodyData();
            UpdateTransformData();
        }

        private void UpdateRigidbodyData()
        {
            rigidbodyGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);

            var rigidbodyArray = rigidbodyGroup.GetComponentArray<Rigidbody>();
            var transformToSendArray = rigidbodyGroup.GetComponentDataArray<TransformToSend>();

            for (int i = 0; i < rigidbodyArray.Length; ++i)
            {
                var rigidbody = rigidbodyArray[i];
                var transformToSend = new TransformToSend
                {
                    Position = rigidbody.position,
                    Velocity = rigidbody.velocity,
                    Orientation = rigidbody.rotation
                };
                transformToSendArray[i] = transformToSend;
            }
        }

        private void UpdateTransformData()
        {
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);

            var transformArray = transformGroup.GetComponentArray<UnityEngine.Transform>();
            var transformToSendArray = transformGroup.GetComponentDataArray<TransformToSend>();

            for (int i = 0; i < transformArray.Length; ++i)
            {
                var transform = transformArray[i];
                var transformToSend = new TransformToSend
                {
                    Position = transform.position,
                    Velocity = Vector3.zero,
                    Orientation = transform.rotation
                };
                transformToSendArray[i] = transformToSend;
            }
        }
    }
}

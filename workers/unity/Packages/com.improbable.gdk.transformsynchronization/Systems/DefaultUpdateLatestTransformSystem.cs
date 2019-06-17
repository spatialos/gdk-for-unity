using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class DefaultUpdateLatestTransformSystem : ComponentSystem
    {
        private EntityQuery rigidbodyGroup;
        private EntityQuery transformGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            rigidbodyGroup = GetEntityQuery(
                ComponentType.ReadOnly<Rigidbody>(),
                ComponentType.ReadWrite<TransformToSend>(),
                ComponentType.ReadOnly<GetTransformFromGameObjectTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            rigidbodyGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);

            transformGroup = GetEntityQuery(
                ComponentType.Exclude<Rigidbody>(),
                ComponentType.ReadOnly<UnityEngine.Transform>(),
                ComponentType.ReadWrite<TransformToSend>(),
                ComponentType.ReadOnly<GetTransformFromGameObjectTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            UpdateRigidbodyData();
            UpdateTransformData();
        }

        private void UpdateRigidbodyData()
        {
            Entities.With(rigidbodyGroup).ForEach((ref TransformToSend transformToSend, Rigidbody rigidbody) =>
            {
                transformToSend.Position = rigidbody.position;
                transformToSend.Velocity = rigidbody.velocity;
                transformToSend.Orientation = rigidbody.rotation;
            });
        }

        private void UpdateTransformData()
        {
            Entities.With(transformGroup).ForEach((ref TransformToSend transformToSend, Transform transform) =>
            {
                transformToSend.Position = transform.position;
                transformToSend.Velocity = Vector3.zero;
                transformToSend.Orientation = transform.rotation;
            });
        }
    }
}

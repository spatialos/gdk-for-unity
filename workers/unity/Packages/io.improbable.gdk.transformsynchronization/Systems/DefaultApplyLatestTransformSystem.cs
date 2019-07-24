using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(FixedUpdateSystemGroup))]
    public class DefaultApplyLatestTransformSystem : ComponentSystem
    {
        private EntityQuery rigidbodyGroup;
        private EntityQuery rigidbody2DGroup;
        private EntityQuery transformGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            rigidbodyGroup = GetEntityQuery(
                ComponentType.ReadOnly<Rigidbody>(),
                ComponentType.ReadOnly<TransformToSet>(),
                ComponentType.ReadOnly<SetTransformToGameObjectTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            rigidbodyGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);
            
            rigidbody2DGroup = GetEntityQuery(
                ComponentType.ReadOnly<Rigidbody2D>(),
                ComponentType.ReadOnly<TransformToSet>(),
                ComponentType.ReadOnly<SetTransformToGameObjectTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            rigidbody2DGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);

            transformGroup = GetEntityQuery(
                ComponentType.Exclude<Rigidbody>(),
                ComponentType.Exclude<Rigidbody2D>(),
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
            Entities.With(rigidbodyGroup).ForEach((Rigidbody rigidbody, ref TransformToSet transformToSet) =>
            {
                rigidbody.MovePosition(transformToSet.Position);
                rigidbody.MoveRotation(transformToSet.Orientation);
                rigidbody.AddForce(transformToSet.Velocity - rigidbody.velocity, ForceMode.VelocityChange);
            });
            
            Entities.With(rigidbody2DGroup).ForEach((Rigidbody2D rigidbody, ref TransformToSet transformToSet) =>
            {
                rigidbody.MovePosition(transformToSet.Position);
                rigidbody.MoveRotation(transformToSet.Orientation);
                rigidbody.AddForce((Vector2)transformToSet.Velocity - rigidbody.velocity, ForceMode2D.Impulse);
            });
        }

        private void UpdateTransformData()
        {
            Entities.With(transformGroup).ForEach((Transform transform, ref TransformToSet transformToSet) =>
            {
                transform.localPosition = transformToSet.Position;
                transform.localRotation = transformToSet.Orientation;
            });
        }
    }
}

using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(DefaultUpdateLatestTransformSystem))]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class ResetForAuthorityGainedSystem : ComponentSystem
    {
        private WorkerSystem worker;
        private ComponentUpdateSystem updateSystem;
        private ComponentGroup rigidbodyGroup;
        private ComponentGroup transformGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();
            updateSystem = World.GetExistingManager<ComponentUpdateSystem>();

            rigidbodyGroup = GetComponentGroup(
                ComponentType.ReadOnly<Rigidbody>(),
                ComponentType.ReadOnly<TransformInternal.Component>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.Create<TicksSinceLastTransformUpdate>(),
                ComponentType.Create<BufferedTransform>(),
                ComponentType.Subtractive<NewlyAddedSpatialOSEntity>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            rigidbodyGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);

            transformGroup = GetComponentGroup(
                ComponentType.ReadOnly<UnityEngine.Transform>(),
                ComponentType.ReadOnly<TransformInternal.Component>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.Create<TicksSinceLastTransformUpdate>(),
                ComponentType.Create<BufferedTransform>(),
                ComponentType.Subtractive<NewlyAddedSpatialOSEntity>(),
                ComponentType.Subtractive<Rigidbody>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            UpdateRigidbodyData();
            UpdateTransformData();
        }

        private void UpdateTransformData()
        {
            var ticksSinceLastUpdateArray = rigidbodyGroup.GetComponentDataArray<TicksSinceLastTransformUpdate>();
            var transformComponentArray = rigidbodyGroup.GetComponentDataArray<TransformInternal.Component>();
            var bufferedTransformArray = rigidbodyGroup.GetBufferArray<BufferedTransform>();
            var rigidbodyArray = rigidbodyGroup.GetComponentArray<Rigidbody>();
            var spatialEntityIdArray = rigidbodyGroup.GetComponentDataArray<SpatialEntityId>();

            for (int i = 0; i < transformComponentArray.Length; ++i)
            {
                // todo this is not a correct constraint. Needs a the auth loss temporary exposed to correctly do this
                // alternatively this needs an authority changed component that is filled at the beginning of the tick
                if (updateSystem
                    .GetAuthorityChangesReceived(spatialEntityIdArray[i].EntityId, TransformInternal.ComponentId)
                    .Count == 0)
                {
                    continue;
                }

                var t = transformComponentArray[i];
                var rigidbody = rigidbodyArray[i];
                rigidbody.MovePosition(t.Location.ToUnityVector3() + worker.Origin);
                rigidbody.MoveRotation(t.Rotation.ToUnityQuaternion());
                rigidbody.AddForce(t.Velocity.ToUnityVector3() - rigidbody.velocity, ForceMode.VelocityChange);
                bufferedTransformArray[i].Clear();
                ticksSinceLastUpdateArray[i] = new TicksSinceLastTransformUpdate();
            }
        }

        private void UpdateRigidbodyData()
        {
            var ticksSinceLastUpdateArray = transformGroup.GetComponentDataArray<TicksSinceLastTransformUpdate>();
            var transformComponentArray = transformGroup.GetComponentDataArray<TransformInternal.Component>();
            var bufferedTransformArray = transformGroup.GetBufferArray<BufferedTransform>();
            var unityTransformArray = transformGroup.GetComponentArray<UnityEngine.Transform>();
            var spatialEntityIdArray = transformGroup.GetComponentDataArray<SpatialEntityId>();

            for (int i = 0; i < transformComponentArray.Length; ++i)
            {
                if (updateSystem
                    .GetAuthorityChangesReceived(spatialEntityIdArray[i].EntityId, TransformInternal.ComponentId)
                    .Count == 0)
                {
                    continue;
                }

                var t = transformComponentArray[i];
                var unityTransform = unityTransformArray[i];
                unityTransform.position = t.Location.ToUnityVector3() + worker.Origin;
                unityTransform.rotation = t.Rotation.ToUnityQuaternion();
                bufferedTransformArray[i].Clear();
                ticksSinceLastUpdateArray[i] = new TicksSinceLastTransformUpdate();
            }
        }
    }
}

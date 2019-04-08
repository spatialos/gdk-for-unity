using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(DefaultUpdateLatestTransformSystem))]
    public class DirectTransformUpdateSystem : ComponentSystem
    {
        private WorkerSystem worker;
        private ComponentUpdateSystem updateSystem;

        private ComponentGroup transformGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();
            updateSystem = World.GetExistingManager<ComponentUpdateSystem>();

            transformGroup = GetComponentGroup(
                ComponentType.Create<TransformToSet>(),
                ComponentType.ReadOnly<TransformInternal.Component>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.ReadOnly<DirectReceiveTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);
        }

        protected override void OnUpdate()
        {
            var transformToSetArray = transformGroup.GetComponentDataArray<TransformToSet>();
            var spatialEntityIdArray = transformGroup.GetComponentDataArray<SpatialEntityId>();
            var transformComponentArray = transformGroup.GetComponentDataArray<TransformInternal.Component>();

            for (int i = 0; i < transformToSetArray.Length; ++i)
            {
                var entityId = spatialEntityIdArray[i];
                var updates =
                    updateSystem.GetEntityComponentUpdatesReceived<TransformInternal.Update>(entityId.EntityId);

                if (updates.Count == 0)
                {
                    continue;
                }

                var t = transformComponentArray[i];
                transformToSetArray[i] = new TransformToSet
                {
                    Position = t.Location.ToUnityVector3() + worker.Origin,
                    Velocity = t.Velocity.ToUnityVector3(),
                    Orientation = t.Rotation.ToUnityQuaternion()
                };
            }
        }
    }
}

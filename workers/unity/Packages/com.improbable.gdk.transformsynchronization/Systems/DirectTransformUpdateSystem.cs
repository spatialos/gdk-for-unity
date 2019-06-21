using Improbable.Gdk.Core;
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

        private EntityQuery transformGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();
            updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

            transformGroup = GetEntityQuery(
                ComponentType.ReadWrite<TransformToSet>(),
                ComponentType.ReadOnly<TransformInternal.Component>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.ReadOnly<DirectReceiveTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);
        }

        protected override void OnUpdate()
        {
            Entities.With(transformGroup).ForEach(
                (ref TransformToSet transformToSet, ref SpatialEntityId spatialEntityId,
                    ref TransformInternal.Component transformInternal) =>
                {
                    var updates =
                        updateSystem.GetEntityComponentUpdatesReceived<TransformInternal.Update>(spatialEntityId.EntityId);

                    if (updates.Count == 0)
                    {
                        return;
                    }

                    transformToSet = new TransformToSet
                    {
                        Position = TransformUtils.ToUnityVector3(transformInternal.Location) + worker.Origin,
                        Velocity = TransformUtils.ToUnityVector3(transformInternal.Velocity),
                        Orientation = TransformUtils.ToUnityQuaternion(transformInternal.Rotation)
                    };
                });
        }
    }
}

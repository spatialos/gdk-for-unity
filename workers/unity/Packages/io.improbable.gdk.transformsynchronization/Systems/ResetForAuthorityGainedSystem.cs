using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
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

        private ComponentType[] baseComponentTypes;
        private ComponentType[] baseExcludeComponentTypes;
        private EntityQuery transformQuery;

        public delegate void ResetAuthFunc<in T>(WorkerSystem Worker, Entity entity, ref TransformInternal.Component transformComponent, T component)
            where T : class;

        private readonly Dictionary<Type, Action> resetAuthorityActions = new Dictionary<Type, Action>();

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();
            updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

            baseComponentTypes = new[]
            {
                ComponentType.ReadOnly<TransformInternal.Component>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.ReadWrite<TicksSinceLastTransformUpdate>(),
                ComponentType.ReadWrite<BufferedTransform>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            };

            baseExcludeComponentTypes = new[]
            {
                ComponentType.ReadOnly<NewlyAddedSpatialOSEntity>()
            };

            UpdateTransformQuery();

            RegisterTransformSyncType(new RigidbodyTransformSync());
        }

        internal void RegisterTransformSyncType<T>(ITransformSync<T> impl)
            where T : class
        {
            var componentQueryDesc = TransformUtils.ConstructEntityQueryDesc<T>(baseComponentTypes);
            componentQueryDesc.None = baseExcludeComponentTypes;

            var entityQuery = GetEntityQuery(componentQueryDesc);
            entityQuery.SetFilter(TransformInternal.ComponentAuthority.Authoritative);

            resetAuthorityActions.Add(typeof(T), () => Entities.With(entityQuery).ForEach(
                (Entity entity,
                    DynamicBuffer<BufferedTransform> buffer,
                    ref TicksSinceLastTransformUpdate ticksSinceLastTransformUpdate,
                    ref TransformInternal.Component transformInternal,
                    ref SpatialEntityId spatialEntityId) =>
                {
                    if (updateSystem
                        .GetAuthorityChangesReceived(spatialEntityId.EntityId, TransformInternal.ComponentId)
                        .Count == 0)
                    {
                        return;
                    }

                    var component = EntityManager.GetComponentObject<T>(entity);

                    impl.OnResetAuth(worker, entity, ref transformInternal, component);

                    buffer.Clear();
                    ticksSinceLastTransformUpdate = new TicksSinceLastTransformUpdate();
                }));

            UpdateTransformQuery();
        }

        private void UpdateTransformQuery()
        {
            var transformQueryDesc = TransformUtils.ConstructEntityQueryDesc<UnityEngine.Transform>(baseComponentTypes);
            transformQueryDesc.None = resetAuthorityActions.Keys
                .Select(ComponentType.ReadOnly)
                .Concat(baseExcludeComponentTypes)
                .ToArray();

            transformQuery = GetEntityQuery(transformQueryDesc);
            transformQuery.SetFilter(TransformInternal.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            ResetDataFromType();
            ResetTransforms();
        }

        private void ResetDataFromType()
        {
            foreach (var resetAction in resetAuthorityActions)
            {
                resetAction.Value();
            }
        }

        private void ResetTransforms()
        {
            Entities.With(transformQuery).ForEach((Entity entity,
                DynamicBuffer<BufferedTransform> buffer,
                ref TicksSinceLastTransformUpdate ticksSinceLastTransformUpdate,
                ref TransformInternal.Component transformInternal,
                ref SpatialEntityId spatialEntityId) =>
            {
                if (updateSystem
                    .GetAuthorityChangesReceived(spatialEntityId.EntityId, TransformInternal.ComponentId)
                    .Count == 0)
                {
                    return;
                }

                var unityTransform = EntityManager.GetComponentObject<UnityEngine.Transform>(entity);
                unityTransform.position = transformInternal.Location.ToUnityVector() + worker.Origin;
                unityTransform.rotation = transformInternal.Rotation.ToUnityQuaternion();

                buffer.Clear();
                ticksSinceLastTransformUpdate = new TicksSinceLastTransformUpdate();
            });
        }
    }
}

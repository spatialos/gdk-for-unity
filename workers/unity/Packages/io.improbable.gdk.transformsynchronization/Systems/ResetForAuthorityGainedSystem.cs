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

            RegisterType<Rigidbody>((
                Entity entity,
                ref TransformInternal.Component transformInternal,
                Rigidbody rigidbody) =>
            {
                rigidbody.MovePosition(TransformUtils.ToUnityVector3(transformInternal.Location) + worker.Origin);
                rigidbody.MoveRotation(TransformUtils.ToUnityQuaternion(transformInternal.Rotation));
                rigidbody.AddForce(TransformUtils.ToUnityVector3(transformInternal.Velocity) - rigidbody.velocity,
                    ForceMode.VelocityChange);
            });

            RegisterType<Rigidbody2D>((
                Entity entity,
                ref TransformInternal.Component transformInternal,
                Rigidbody2D rigidbody) =>
            {
                rigidbody.MovePosition(TransformUtils.ToUnityVector3(transformInternal.Location) + worker.Origin);
                rigidbody.MoveRotation(TransformUtils.ToUnityQuaternion(transformInternal.Rotation));
                rigidbody.AddForce(
                    (Vector2) TransformUtils.ToUnityVector3(transformInternal.Velocity) - rigidbody.velocity,
                    ForceMode2D.Impulse);
            });
        }

        public void RegisterType<T>(EntityQueryBuilder.F_EDC<TransformInternal.Component, T> func)
            where T : class
        {
            var componentType = ComponentType.ReadOnly<T>();

            var includedComponentTypes = baseComponentTypes
                .Concat(new[] { componentType })
                .ToArray();

            var componentQueryDesc = new EntityQueryDesc()
            {
                All = includedComponentTypes,
                None = baseExcludeComponentTypes
            };

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

                    func(entity, ref transformInternal, component);

                    buffer.Clear();
                    ticksSinceLastTransformUpdate = new TicksSinceLastTransformUpdate();
                }));

            UpdateTransformQuery();
        }

        private void UpdateTransformQuery()
        {
            var componentType = ComponentType.ReadOnly<UnityEngine.Transform>();

            var includedComponentTypes = baseComponentTypes
                .Concat(new[] { componentType })
                .ToArray();
            var excludedComponentTypes = resetAuthorityActions.Keys
                .Select(ComponentType.ReadOnly)
                .Concat(baseExcludeComponentTypes)
                .ToArray();

            var transformQueryDesc = new EntityQueryDesc()
            {
                All = includedComponentTypes,
                None = excludedComponentTypes
            };

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
                unityTransform.position = TransformUtils.ToUnityVector3(transformInternal.Location) + worker.Origin;
                unityTransform.rotation = TransformUtils.ToUnityQuaternion(transformInternal.Rotation);

                buffer.Clear();
                ticksSinceLastTransformUpdate = new TicksSinceLastTransformUpdate();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class DefaultUpdateLatestTransformSystem : ComponentSystem
    {
        private ComponentType[] baseComponentTypes;
        private EntityQuery transformQuery;

        private readonly Dictionary<Type, Action> updateLatestTransformActions = new Dictionary<Type, Action>();

        protected override void OnCreate()
        {
            base.OnCreate();

            baseComponentTypes = new[]
            {
                ComponentType.ReadWrite<TransformToSend>(),
                ComponentType.ReadOnly<GetTransformFromGameObjectTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            };

            RegisterType<Rigidbody>((ref TransformToSend transformToSend, Rigidbody rigidbody) =>
            {
                transformToSend = new TransformToSend
                {
                    Position = rigidbody.position,
                    Velocity = rigidbody.velocity,
                    Orientation = rigidbody.rotation
                };
            });
        }

        internal void RegisterType<T>(EntityQueryBuilder.F_DC<TransformToSend, T> func)
            where T : class
        {
            var componentType = ComponentType.ReadOnly<T>();

            var includedComponentTypes = baseComponentTypes
                .Append(componentType)
                .ToArray();

            var componentQueryDesc = new EntityQueryDesc()
            {
                All = includedComponentTypes
            };

            var entityQuery = GetEntityQuery(componentQueryDesc);
            entityQuery.SetFilter(TransformInternal.ComponentAuthority.Authoritative);

            updateLatestTransformActions.Add(typeof(T), () => Entities.With(entityQuery).ForEach(func));
            UpdateTransformQuery();
        }

        private void UpdateTransformQuery()
        {
            var componentType = ComponentType.ReadOnly<UnityEngine.Transform>();

            var includedComponentTypes = baseComponentTypes
                .Append(componentType)
                .ToArray();
            var excludedComponentTypes = updateLatestTransformActions.Keys
                .Select(ComponentType.ReadOnly)
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
            UpdateDataFromTypes();
            UpdateDataFromTransform();
        }

        private void UpdateDataFromTypes()
        {
            foreach (var latestTransformAction in updateLatestTransformActions)
            {
                latestTransformAction.Value();
            }
        }

        private void UpdateDataFromTransform()
        {
            Entities.With(transformQuery).ForEach((ref TransformToSend transformToSend, Transform transform) =>
            {
                transformToSend = new TransformToSend
                {
                    Position = transform.position,
                    Velocity = Vector3.zero,
                    Orientation = transform.rotation
                };
            });
        }
    }
}

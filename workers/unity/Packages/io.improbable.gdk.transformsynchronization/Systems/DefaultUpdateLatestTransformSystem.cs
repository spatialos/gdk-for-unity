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
            };

            RegisterTransformSyncType(new RigidbodyTransformSync());
        }

        internal void RegisterTransformSyncType<T>(ITransformSync<T> impl)
            where T : class
        {
            var entityQuery = GetEntityQuery(TransformUtils.ConstructEntityQueryDesc<T>(requireAuthority: true, baseComponentTypes));

            updateLatestTransformActions.Add(typeof(T),
                () => Entities.With(entityQuery)
                    .ForEach((EntityQueryBuilder.F_DC<TransformToSend, T>) impl.UpdateLatestTransform));

            UpdateTransformQuery();
        }

        private void UpdateTransformQuery()
        {
            var transformQueryDesc = TransformUtils.ConstructEntityQueryDesc<UnityEngine.Transform>(requireAuthority: true, baseComponentTypes);
            transformQueryDesc.None = updateLatestTransformActions.Keys
                .Select(ComponentType.ReadOnly)
                .ToArray();

            transformQuery = GetEntityQuery(transformQueryDesc);
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

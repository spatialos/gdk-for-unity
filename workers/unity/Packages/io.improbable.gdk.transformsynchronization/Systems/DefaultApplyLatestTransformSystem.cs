using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(FixedUpdateSystemGroup))]
    public class DefaultApplyLatestTransformSystem : ComponentSystem
    {
        private ComponentType[] baseComponentTypes;
        private EntityQuery transformQuery;

        private readonly Dictionary<Type, Action> applyLatestTransformActions = new Dictionary<Type, Action>();

        protected override void OnCreate()
        {
            base.OnCreate();

            baseComponentTypes = new[]
            {
                ComponentType.ReadOnly<TransformToSet>(),
                ComponentType.ReadOnly<SetTransformToGameObjectTag>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            };

            RegisterTransformSyncType(new RigidbodyTransformSync());
        }

        internal void RegisterTransformSyncType<T>(ITransformSync<T> impl)
            where T : class
        {
            var entityQuery = GetEntityQuery(TransformUtils.ConstructEntityQueryDesc<T>(baseComponentTypes));
            entityQuery.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);

            applyLatestTransformActions.Add(typeof(T), () => Entities.With(entityQuery).ForEach((EntityQueryBuilder.F_DC<TransformToSet, T>) impl.ApplyLatestTransform));
            UpdateTransformQuery();
        }

        private void UpdateTransformQuery()
        {
            var transformQueryDesc = TransformUtils.ConstructEntityQueryDesc<UnityEngine.Transform>(baseComponentTypes);
            transformQueryDesc.None = applyLatestTransformActions.Keys
                .Select(ComponentType.ReadOnly)
                .ToArray();

            transformQuery = GetEntityQuery(transformQueryDesc);
            transformQuery.SetFilter(TransformInternal.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            ApplyDataToType();
            ApplyDataToTransform();
        }

        private void ApplyDataToType()
        {
            foreach (var latestTransformAction in applyLatestTransformActions)
            {
                latestTransformAction.Value();
            }
        }

        private void ApplyDataToTransform()
        {
            Entities.With(transformQuery).ForEach((Transform transform, ref TransformToSet transformToSet) =>
            {
                transform.localPosition = transformToSet.Position;
                transform.localRotation = transformToSet.Orientation;
            });
        }
    }
}

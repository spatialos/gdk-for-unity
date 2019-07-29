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

            RegisterType<Rigidbody>((ref TransformToSet transformToSet, Rigidbody rigidbody) =>
            {
                rigidbody.MovePosition(transformToSet.Position);
                rigidbody.MoveRotation(transformToSet.Orientation);
                rigidbody.AddForce(transformToSet.Velocity - rigidbody.velocity, ForceMode.VelocityChange);
            });
        }

        public void RegisterType<T>(EntityQueryBuilder.F_DC<TransformToSet, T> func)
            where T : class
        {
            var componentType = ComponentType.ReadOnly<T>();

            var includedComponentTypes = baseComponentTypes
                .Concat(new[] { componentType })
                .ToArray();

            var componentQueryDesc = new EntityQueryDesc()
            {
                All = includedComponentTypes
            };

            var entityQuery = GetEntityQuery(componentQueryDesc);
            entityQuery.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);

            applyLatestTransformActions.Add(typeof(T), () => Entities.With(entityQuery).ForEach(func));
            UpdateTransformQuery();
        }

        private void UpdateTransformQuery()
        {
            var excludedComponentTypes = applyLatestTransformActions.Keys.Select(ComponentType.ReadOnly).ToArray();

            var componentType = ComponentType.ReadOnly<UnityEngine.Transform>();
            var transformQueryDesc = new EntityQueryDesc()
            {
                All = new ComponentType[baseComponentTypes.Length + 1],
                None = excludedComponentTypes
            };

            Array.Copy(baseComponentTypes, transformQueryDesc.All, baseComponentTypes.Length);
            transformQueryDesc.All[baseComponentTypes.Length] = componentType;

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

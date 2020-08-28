using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;
using static Improbable.Gdk.TransformSynchronization.TransformUtils;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(FixedUpdateSystemGroup))]
    public class DefaultApplyLatestTransformSystem : ComponentSystem
    {
        private WorkerSystem worker;
        private ComponentType[] baseComponentTypes;
        private EntityQuery transformQuery;

        private readonly Dictionary<Type, Action> applyLatestTransformActions = new Dictionary<Type, Action>();

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();

            baseComponentTypes = new[]
            {
                ComponentType.ReadOnly<TransformToSet>(),
                ComponentType.ReadOnly<SetTransformToGameObjectTag>(),
            };

            RegisterTransformSyncType(new RigidbodyTransformSync());
        }

        internal void RegisterTransformSyncType<T>(ITransformSync<T> impl)
            where T : class
        {
            var entityQuery = GetEntityQuery(ConstructEntityQueryDesc<T>(AuthorityRequirements.Exclude, baseComponentTypes));

            applyLatestTransformActions.Add(typeof(T),
                () => Entities.With(entityQuery)
                    .ForEach((EntityQueryBuilder.F_DC<TransformToSet, T>) impl.ApplyLatestTransform));
            UpdateTransformQuery();
        }

        private void UpdateTransformQuery()
        {
            var transformQueryDesc = ConstructEntityQueryDesc<UnityEngine.Transform>(AuthorityRequirements.Exclude, baseComponentTypes);
            transformQueryDesc.None = transformQueryDesc.None
                .Union(
                    applyLatestTransformActions.Keys
                        .Select(ComponentType.ReadOnly))
                .ToArray();

            transformQuery = GetEntityQuery(transformQueryDesc);
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

                if (transformToSet.SpanId == null)
                {
                    return;
                }

                var childSpan = worker.EventTracer.AddSpan(transformToSet.SpanId.Value);
                worker.EventTracer.AddEvent(new Worker.CInterop.Event
                {
                    Id = childSpan,
                    Message = "DefaultApplyTransform receive",
                    Type = "update",
                    Data = null
                });
            });
        }
    }
}

using Improbable.Gdk.Core;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class InterpolateTransformSystem : ComponentSystem
    {
        private ComponentUpdateSystem updateSystem;
        private EntityQuery interpolationGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

            interpolationGroup = GetEntityQuery(
                ComponentType.ReadWrite<BufferedTransform>(),
                ComponentType.ReadWrite<DeferredUpdateTransform>(),
                ComponentType.ReadOnly<TransformInternal.Component>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.ReadOnly<InterpolationConfig>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            interpolationGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);
        }

        protected override void OnUpdate()
        {
            Entities.With(interpolationGroup).ForEach(
                (Entity entity,
                    InterpolationConfig config,
                    ref SpatialEntityId spatialEntityId,
                    ref TransformInternal.Component transformInternal,
                    ref DeferredUpdateTransform deferredUpdateTransform) =>
                {
                    var transformBuffer = EntityManager.GetBuffer<BufferedTransform>(entity);
                    var lastTransformApplied = deferredUpdateTransform.Transform;

                    if (transformBuffer.Length >= config.MaxLoadMatchedBufferSize)
                    {
                        transformBuffer.Clear();
                    }

                    if (transformBuffer.Length == 0)
                    {
                        var currentTransformComponent = transformInternal;
                        if (currentTransformComponent.PhysicsTick <= lastTransformApplied.PhysicsTick)
                        {
                            return;
                        }

                        deferredUpdateTransform = new DeferredUpdateTransform
                        {
                            Transform = currentTransformComponent
                        };

                        var transformToInterpolateTo = ToBufferedTransform(currentTransformComponent);

                        var ticksToFill = math.max((uint) config.TargetBufferSize, 1);

                        if (ticksToFill > 1)
                        {
                            var transformToInterpolateFrom = ToBufferedTransformAtTick(lastTransformApplied,
                                transformToInterpolateTo.PhysicsTick - ticksToFill + 1);

                            transformBuffer.Add(transformToInterpolateFrom);

                            for (uint j = 0; j < ticksToFill - 2; ++j)
                            {
                                transformBuffer.Add(InterpolateValues(transformToInterpolateFrom,
                                    transformToInterpolateTo,
                                    j + 1));
                            }
                        }

                        transformBuffer.Add(transformToInterpolateTo);
                        return;
                    }

                    var updates =
                        updateSystem
                            .GetEntityComponentUpdatesReceived<TransformInternal.Update>(spatialEntityId.EntityId);

                    for (var j = 0; j < updates.Count; ++j)
                    {
                        var update = updates[j].Update;
                        UpdateLastTransform(ref lastTransformApplied, update);
                        deferredUpdateTransform = new DeferredUpdateTransform
                        {
                            Transform = lastTransformApplied
                        };

                        if (!update.PhysicsTick.HasValue)
                        {
                            continue;
                        }

                        var transformToInterpolateTo = ToBufferedTransform(lastTransformApplied);

                        var transformToInterpolateFrom = transformBuffer[transformBuffer.Length - 1];
                        var lastTickId = transformToInterpolateFrom.PhysicsTick;

                        // This could go backwards if authority changes quickly between two workers with different loads
                        if (lastTickId >= transformToInterpolateTo.PhysicsTick)
                        {
                            continue;
                        }

                        var ticksToFill = math.max(transformToInterpolateTo.PhysicsTick - lastTickId, 1);

                        for (uint k = 0; k < ticksToFill - 1; ++k)
                        {
                            transformBuffer.Add(InterpolateValues(
                                transformToInterpolateFrom,
                                transformToInterpolateTo,
                                k + 1));
                        }

                        transformBuffer.Add(transformToInterpolateTo);
                    }
                });
        }

        private static void UpdateLastTransform(ref TransformInternal.Component lastTransform,
            TransformInternal.Update update)
        {
            if (update.Location.HasValue)
            {
                lastTransform.Location = update.Location.Value;
            }

            if (update.Rotation.HasValue)
            {
                lastTransform.Rotation = update.Rotation.Value;
            }

            if (update.Velocity.HasValue)
            {
                lastTransform.Velocity = update.Velocity.Value;
            }

            if (update.TicksPerSecond.HasValue)
            {
                lastTransform.TicksPerSecond = update.TicksPerSecond.Value;
            }

            if (update.PhysicsTick.HasValue)
            {
                lastTransform.PhysicsTick = update.PhysicsTick.Value;
            }
        }

        private static BufferedTransform ToBufferedTransform(TransformInternal.Component transform)
        {
            return new BufferedTransform
            {
                Position = TransformUtils.ToUnityVector3(transform.Location),
                Velocity = TransformUtils.ToUnityVector3(transform.Velocity),
                Orientation = TransformUtils.ToUnityQuaternion(transform.Rotation),
                PhysicsTick = transform.PhysicsTick
            };
        }

        private static BufferedTransform ToBufferedTransformAtTick(TransformInternal.Component component, uint tick)
        {
            return new BufferedTransform
            {
                Position = TransformUtils.ToUnityVector3(component.Location),
                Velocity = TransformUtils.ToUnityVector3(component.Velocity),
                Orientation = TransformUtils.ToUnityQuaternion(component.Rotation),
                PhysicsTick = tick
            };
        }

        private static BufferedTransform InterpolateValues(BufferedTransform first, BufferedTransform second,
            uint ticksAfterFirst)
        {
            var t = (float) ticksAfterFirst / (float) (second.PhysicsTick - first.PhysicsTick);
            return new BufferedTransform
            {
                Position = Vector3.Lerp(first.Position, second.Position, t),
                Velocity = Vector3.Lerp(first.Velocity, second.Velocity, t),
                Orientation = UnityEngine.Quaternion.Slerp(first.Orientation, second.Orientation, t),
                PhysicsTick = first.PhysicsTick + ticksAfterFirst
            };
        }
    }
}

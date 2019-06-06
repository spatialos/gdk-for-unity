using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.TransformSynchronization
{
    public class TransformSynchronization : MonoBehaviour
    {
#pragma warning disable 649
        [Require] private TransformInternalReader transformReader;
        [Require] private Entity entity;
        [Require] private World world;
#pragma warning restore 649

        public List<TransformSynchronizationReceiveStrategy> ReceiveStrategies;
        public List<TransformSynchronizationSendStrategy> SendStrategies;

        public bool SetKinematicWhenNotAuthoritative = true;

        private EntityManager entityManager;

        private bool initialised;

        public uint TickNumber
        {
            get
            {
                if (!initialised || enabled == false)
                {
                    return 0;
                }

                var manager = world.EntityManager;

                if (transformReader.Authority != Authority.NotAuthoritative)
                {
                    return manager.GetComponentData<TicksSinceLastTransformUpdate>(entity)
                        .NumberOfTicks + transformReader.Data.PhysicsTick;
                }

                return manager.GetComponentData<TransformToSet>(entity).ApproximateRemoteTick;
            }
        }

        private void OnEnable()
        {
            if (ReceiveStrategies == null || ReceiveStrategies.Count == 0)
            {
                throw new InvalidOperationException($"{nameof(TransformSynchronization)} " +
                    $"on {gameObject.name} must be provided a transform receive strategy");
            }

            entityManager = world.EntityManager;

            StartCoroutine(DelayedApply());
        }

        private IEnumerator DelayedApply()
        {
            yield return null;
            if (initialised)
            {
                yield break;
            }

            ApplyStrategies();
            initialised = true;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void ApplyStrategies()
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

            if (SetKinematicWhenNotAuthoritative)
            {
                commandBuffer.AddComponent(entity, new ManageKinematicOnAuthorityChangeTag());
                commandBuffer.AddComponent(entity, new KinematicStateWhenAuth());
            }

            ApplyReceiveStrategies(commandBuffer);
            ApplySendStrategies(commandBuffer);
            commandBuffer.Playback(entityManager);
            commandBuffer.Dispose();
        }

        private void ApplyReceiveStrategies(EntityCommandBuffer commandBuffer)
        {
            AddCommonReceiveComponents(commandBuffer);

            foreach (var strategy in ReceiveStrategies)
            {
                if (strategy.WorkerType != world.GetExistingSystem<WorkerSystem>().WorkerType)
                {
                    continue;
                }

                strategy.Apply(entity, world, commandBuffer);
            }
        }

        private void ApplySendStrategies(EntityCommandBuffer commandBuffer)
        {
            if (SendStrategies == null || SendStrategies.Count == 0)
            {
                return;
            }

            AddCommonSendComponents(commandBuffer);

            foreach (var strategy in SendStrategies)
            {
                if (strategy.WorkerType != world.GetExistingSystem<WorkerSystem>().WorkerType)
                {
                    continue;
                }

                strategy.Apply(entity, world, commandBuffer);
            }
        }

        private void AddCommonReceiveComponents(EntityCommandBuffer commandBuffer)
        {
            commandBuffer.AddComponent(entity, new SetTransformToGameObjectTag());

            var transformComponent = transformReader.Data;

            var defaultToSet = new TransformToSet
            {
                Position = transformComponent.Location.ToUnityVector3() + world.GetExistingSystem<WorkerSystem>().Origin,
                Velocity = transformComponent.Velocity.ToUnityVector3(),
                Orientation = transformComponent.Rotation.ToUnityQuaternion(),
                ApproximateRemoteTick = 0
            };

            var previousTransform = new DeferredUpdateTransform
            {
                Transform = transformComponent
            };

            commandBuffer.AddComponent(entity, defaultToSet);
            commandBuffer.AddComponent(entity, previousTransform);
            commandBuffer.AddBuffer<BufferedTransform>(entity);
        }

        private void AddCommonSendComponents(EntityCommandBuffer commandBuffer)
        {
            commandBuffer.AddComponent(entity, new GetTransformFromGameObjectTag());

            var transformComponent = transformReader.Data;

            var defaultToSend = new TransformToSend
            {
                Position = transformComponent.Location.ToUnityVector3() - world.GetExistingSystem<WorkerSystem>().Origin,
                Velocity = transformComponent.Velocity.ToUnityVector3(),
                Orientation = transformComponent.Rotation.ToUnityQuaternion()
            };

            var ticksSinceLastUpdate = new TicksSinceLastTransformUpdate
            {
                NumberOfTicks = 0
            };

            var lastTransform = new LastTransformSentData
            {
                // could set this to the max time if we want to immediately send something
                TimeSinceLastUpdate = 0.0f,
                Transform = transformComponent
            };

            var position = entityManager.GetComponentData<Position.Component>(entity);
            var lastPosition = new LastPositionSentData
            {
                // could set this to the max time if we want to immediately send something
                TimeSinceLastUpdate = 0.0f,
                Position = position
            };

            commandBuffer.AddComponent(entity, defaultToSend);
            commandBuffer.AddComponent(entity, ticksSinceLastUpdate);
            commandBuffer.AddComponent(entity, lastPosition);
            commandBuffer.AddComponent(entity, lastTransform);
        }
    }
}

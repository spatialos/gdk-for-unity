using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Transform;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public class TransformSynchronization : MonoBehaviour
    {
        [Require] private TransformInternal.Requirable.Reader transformReader;

        public List<TransformSynchronizationReceiveStrategy> ReceiveStrategies;
        public List<TransformSynchronizationSendStrategy> SendStrategies;

        public bool SetKinematicWhenNotAuthoritative = true;

        private SpatialOSComponent spatialOSComponent;
        private EntityManager entityManager;

        private bool initialised;

        public uint TickNumber
        {
            get
            {
                if (enabled == false)
                {
                    return 0;
                }

                var manager = spatialOSComponent.World.GetOrCreateManager<EntityManager>();
                if (!initialised)
                {
                    initialised = manager.HasComponent<TransformToSet>(spatialOSComponent.Entity);
                    if (!initialised)
                    {
                        return 0;
                    }
                }

                if (transformReader.Authority != Authority.NotAuthoritative)
                {
                    return manager.GetComponentData<TicksSinceLastTransformUpdate>(spatialOSComponent.Entity)
                        .NumberOfTicks + transformReader.Data.PhysicsTick;
                }

                return manager.GetComponentData<TransformToSet>(spatialOSComponent.Entity).ApproximateRemoteTick;
            }
        }

        private void OnEnable()
        {
            if (ReceiveStrategies == null || ReceiveStrategies.Count == 0)
            {
                throw new InvalidOperationException($"{nameof(TransformSynchronization)} " +
                    $"on {gameObject.name} must be provided a transform receive strategy");
            }

            spatialOSComponent = GetComponent<SpatialOSComponent>();
            if (spatialOSComponent == null)
            {
                throw new InvalidOperationException($"{nameof(TransformSynchronization)} " +
                    $" on should only be added to a GameObject linked to a SpatialOS entity");
            }

            entityManager = spatialOSComponent.World.GetOrCreateManager<EntityManager>();

            ApplyStrategies();
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
                commandBuffer.AddComponent(spatialOSComponent.Entity, new ManageKinematicOnAuthorityChangeTag());
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
                if (strategy.WorkerType != spatialOSComponent.Worker.WorkerType)
                {
                    continue;
                }

                strategy.Apply(spatialOSComponent.Entity, spatialOSComponent.World, commandBuffer);
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
                if (strategy.WorkerType != spatialOSComponent.Worker.WorkerType)
                {
                    continue;
                }

                strategy.Apply(spatialOSComponent.Entity, spatialOSComponent.World, commandBuffer);
            }
        }

        private void AddCommonReceiveComponents(EntityCommandBuffer commandBuffer)
        {
            commandBuffer.AddComponent(spatialOSComponent.Entity, new SetTransformToGameObjectTag());

            var transformComponent = transformReader.Data;

            var defaultToSet = new TransformToSet
            {
                Position = transformComponent.Location.ToUnityVector3() + spatialOSComponent.Worker.Origin,
                Velocity = transformComponent.Velocity.ToUnityVector3(),
                Orientation = transformComponent.Rotation.ToUnityQuaternion(),
                ApproximateRemoteTick = 0
            };

            var previousTransform = new DeferredUpdateTransform
            {
                Transform = transformComponent
            };

            commandBuffer.AddComponent(spatialOSComponent.Entity, defaultToSet);
            commandBuffer.AddComponent(spatialOSComponent.Entity, previousTransform);
            commandBuffer.AddBuffer<BufferedTransform>(spatialOSComponent.Entity);
        }

        private void AddCommonSendComponents(EntityCommandBuffer commandBuffer)
        {
            commandBuffer.AddComponent(spatialOSComponent.Entity, new GetTransformFromGameObjectTag());

            var transformComponent = transformReader.Data;

            var defaultToSend = new TransformToSend
            {
                Position = transformComponent.Location.ToUnityVector3() - spatialOSComponent.Worker.Origin,
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

            var position = entityManager.GetComponentData<Position.Component>(spatialOSComponent.Entity);
            var lastPosition = new LastPositionSentData
            {
                // could set this to the max time if we want to immediately send something
                TimeSinceLastUpdate = 0.0f,
                Position = position
            };

            commandBuffer.AddComponent(spatialOSComponent.Entity, defaultToSend);
            commandBuffer.AddComponent(spatialOSComponent.Entity, ticksSinceLastUpdate);
            commandBuffer.AddComponent(spatialOSComponent.Entity, lastPosition);
            commandBuffer.AddComponent(spatialOSComponent.Entity, lastTransform);
        }
    }
}

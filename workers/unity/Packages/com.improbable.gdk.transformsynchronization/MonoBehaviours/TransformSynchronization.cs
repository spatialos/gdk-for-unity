using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Transform;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public class TransformSynchronization : MonoBehaviour
    {
        [Require] private TransformInternal.Requirable.Reader transformReader;

        public List<TransformSynchronizationReceiveStrategy> ReceiveStrategies;
        public bool AutomaticallyApplyTransformToGameObject = true;

        public List<TransformSynchronizationSendStrategy> SendStrategies;
        public bool AutomaticallyGetTransformFromGameObject = true;

        public bool SetKinematicWhenNotAuthoritative = true;

        private SpatialOSComponent spatialOSComponent;
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

            // Check if this is actually needed - to not use the entity manager while in a system
            StartCoroutine(ApplyStrategies());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator ApplyStrategies()
        {
            yield return null;
            if (SetKinematicWhenNotAuthoritative)
            {
                spatialOSComponent.World.GetOrCreateManager<EntityManager>().AddComponent(spatialOSComponent.Entity,
                    typeof(ManageKinematicOnAuthorityChangeTag));
            }

            ApplyReceiveStrategies();
            ApplySendStrategies();
        }

        private void ApplyReceiveStrategies()
        {
            var entityManager = spatialOSComponent.World.GetOrCreateManager<EntityManager>();
            if (AutomaticallyApplyTransformToGameObject)
            {
                entityManager.AddComponent(spatialOSComponent.Entity, typeof(SetTransformToGameObjectTag));
            }

            var transformComponent = transformReader.Data;

            var defaultToSet = new TransformToSet
            {
                Position = transformComponent.Location.ToUnityVector3() + spatialOSComponent.Worker.Origin,
                Velocity = transformComponent.Velocity.ToUnityVector3(),
                Orientation = transformComponent.Rotation.ToUnityQuaternion(),
                ApproximateRemoteTick = 0
            };

            var previousTransform = new DefferedUpdateTransform
            {
                Transform = transformComponent
            };


            entityManager.AddComponentData(spatialOSComponent.Entity, defaultToSet);
            entityManager.AddComponentData(spatialOSComponent.Entity, previousTransform);
            entityManager.AddBuffer<BufferedTransform>(spatialOSComponent.Entity);

            foreach (var strategy in ReceiveStrategies)
            {
                if (strategy.WorkerType != spatialOSComponent.Worker.WorkerType)
                {
                    continue;
                }

                strategy.Apply(spatialOSComponent.Entity, spatialOSComponent.World);
            }
        }

        private void ApplySendStrategies()
        {
            if (SendStrategies == null || SendStrategies.Count == 0)
            {
                return;
            }

            var entityManager = spatialOSComponent.World.GetOrCreateManager<EntityManager>();
            if (AutomaticallyGetTransformFromGameObject)
            {
                entityManager.AddComponent(spatialOSComponent.Entity, typeof(GetTransformFromGameObjectTag));
            }

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

            entityManager.AddComponentData(spatialOSComponent.Entity, defaultToSend);
            entityManager.AddComponentData(spatialOSComponent.Entity, ticksSinceLastUpdate);
            entityManager.AddComponentData(spatialOSComponent.Entity, lastPosition);
            entityManager.AddComponentData(spatialOSComponent.Entity, lastTransform);

            foreach (var strategy in SendStrategies)
            {
                if (strategy.WorkerType != spatialOSComponent.Worker.WorkerType)
                {
                    continue;
                }

                strategy.Apply(spatialOSComponent.Entity, spatialOSComponent.World);
            }
        }
    }
}

using System.Collections.Generic;
using Improbable.Worker;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Keeps track of Reader/Writer availability for SpatialOSBehaviours on a particular GameObject and decides when
    ///     a SpatialOSBehaviour should be enabled, calling into the SpatialOSBehaviourLibrary for injection, storing
    ///     the created Readers/Writers in the given ReaderWriterStore.
    /// </summary>
    internal class MonoBehaviourActivationManager
    {
        private readonly Entity entity;
        private readonly long spatialId;

        private readonly Dictionary<uint, HashSet<MonoBehaviour>> behavioursRequiringReaderTypes
            = new Dictionary<uint, HashSet<MonoBehaviour>>();

        private readonly Dictionary<uint, HashSet<MonoBehaviour>> behavioursRequiringWriterTypes
            = new Dictionary<uint, HashSet<MonoBehaviour>>();

        private readonly Dictionary<MonoBehaviour, int> numUnsatisfiedReadersOrWriters
            = new Dictionary<MonoBehaviour, int>();

        private readonly HashSet<MonoBehaviour> behavioursToEnable = new HashSet<MonoBehaviour>();
        private readonly HashSet<MonoBehaviour> behavioursToDisable = new HashSet<MonoBehaviour>();

        private readonly ReaderWriterStore store;
        private readonly RequireTagInjector injector;

        private readonly ILogDispatcher logger;

        private const string LoggerName = nameof(MonoBehaviourActivationManager);

        public MonoBehaviourActivationManager(GameObject gameObject, RequireTagInjector injector,
            ReaderWriterStore store, ILogDispatcher logger)
        {
            this.logger = logger;
            this.store = store;
            this.injector = injector;

            var spatialComponent = gameObject.GetComponent<SpatialOSComponent>();
            entity = spatialComponent.Entity;
            spatialId = spatialComponent.SpatialEntityId;

            foreach (var behaviour in gameObject.GetComponents<MonoBehaviour>())
            {
                var readerIds = injector.GetRequiredReaderComponentIds(behaviour.GetType());
                AddBehaviourForComponentIds(behaviour, readerIds, behavioursRequiringReaderTypes);

                var writerIds = injector.GetRequiredWriterComponentIds(behaviour.GetType());
                AddBehaviourForComponentIds(behaviour, writerIds, behavioursRequiringWriterTypes);

                numUnsatisfiedReadersOrWriters[behaviour] = readerIds.Count + writerIds.Count;

                behaviour.enabled = false;
            }
        }

        private void AddBehaviourForComponentIds(MonoBehaviour behaviour, List<uint> componentIds,
            Dictionary<uint, HashSet<MonoBehaviour>> componentIdsToBehaviours)
        {
            foreach (var id in componentIds)
            {
                if (!componentIdsToBehaviours.TryGetValue(id, out var behaviours))
                {
                    behaviours = new HashSet<MonoBehaviour>();
                    componentIdsToBehaviours[id] = behaviours;
                }

                behaviours.Add(behaviour);
            }
        }

        public void EnableSpatialOSBehaviours()
        {
            foreach (var behaviour in behavioursToEnable)
            {
                var componentIdsToReaderWriterLists = injector.InjectAllReadersWriters(behaviour, entity);
                store.AddReaderWritersForBehaviour(behaviour, componentIdsToReaderWriterLists);
            }

            foreach (var behaviour in behavioursToEnable)
            {
                behaviour.enabled = true;
            }

            behavioursToEnable.Clear();
        }

        public void DisableSpatialOSBehaviours()
        {
            foreach (var behaviour in behavioursToDisable)
            {
                behaviour.enabled = false;
            }

            foreach (var behaviour in behavioursToDisable)
            {
                injector.DeInjectAllReadersWriters(behaviour);
                store.RemoveReaderWritersForBehaviour(behaviour);
            }

            behavioursToDisable.Clear();
        }

        public void AddComponent(uint componentId)
        {
            if (!behavioursRequiringReaderTypes.ContainsKey(componentId))
            {
                return;
            }

            // Mark reader components ready in relevant SpatialOSBehaviours
            var relevantReaderSpatialOSBehaviours = behavioursRequiringReaderTypes[componentId];
            MarkComponentRequirementSatisfied(relevantReaderSpatialOSBehaviours);
        }

        public void RemoveComponent(uint componentId)
        {
            if (!behavioursRequiringReaderTypes.ContainsKey(componentId))
            {
                return;
            }

            // Mark reader components not ready in relevant SpatialOSBehaviours
            var relevantReaderSpatialOSBehaviours = behavioursRequiringReaderTypes[componentId];
            MarkComponentRequirementUnsatisfied(relevantReaderSpatialOSBehaviours);
        }

        public void ChangeAuthority(uint componentId, Authority authority)
        {
            if (!behavioursRequiringWriterTypes.ContainsKey(componentId))
            {
                return;
            }

            if (authority == Authority.Authoritative)
            {
                // Mark writer components ready in relevant SpatialOSBehaviours
                var relevantWriterSpatialOSBehaviours = behavioursRequiringWriterTypes[componentId];
                MarkComponentRequirementSatisfied(relevantWriterSpatialOSBehaviours);
            }
            else if (authority == Authority.NotAuthoritative)
            {
                // Mark writer components not ready in relevant SpatialOSBehaviours
                var relevantWriterSpatialOSBehaviours = behavioursRequiringWriterTypes[componentId];
                MarkComponentRequirementUnsatisfied(relevantWriterSpatialOSBehaviours);
            }
        }

        private void MarkComponentRequirementSatisfied(IEnumerable<MonoBehaviour> behaviours)
        {
            // Inject all Readers/Writers at once when all requirements are met
            foreach (var behaviour in behaviours)
            {
                numUnsatisfiedReadersOrWriters[behaviour]--;
                if (numUnsatisfiedReadersOrWriters[behaviour] == 0)
                {
                    if (!behaviour.enabled)
                    {
                        // Schedule activation
                        behavioursToEnable.Add(behaviour);
                    }

                    if (behavioursToDisable.Contains(behaviour))
                    {
                        // Must be enabled already, so we were going to disable it - let's not
                        behavioursToDisable.Remove(behaviour);
                    }
                }
            }
        }

        private void MarkComponentRequirementUnsatisfied(IEnumerable<MonoBehaviour> behaviours)
        {
            foreach (var behaviour in behaviours)
            {
                // De-inject all Readers/Writers at once when a single requirement is not met
                if (numUnsatisfiedReadersOrWriters[behaviour] == 0)
                {
                    if (behaviour.enabled)
                    {
                        // Schedule deactivation
                        behavioursToDisable.Add(behaviour);
                    }

                    if (behavioursToEnable.Contains(behaviour))
                    {
                        // Must be disabled already, so we were going to enable it - let's not
                        behavioursToEnable.Remove(behaviour);
                    }
                }

                numUnsatisfiedReadersOrWriters[behaviour]++;
            }
        }
    }
}

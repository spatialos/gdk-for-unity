using System;
using System.Collections.Generic;
using System.Net;
using Improbable.Gdk.Core.MonoBehaviours;
using Improbable.Worker;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Keeps track of Reader/Writer availability for SpatialOSBehaviours on a particular GameObject and decides when
    ///     a SpatialOSBehaviour should be enabled, calling into the SpatialOSBehaviourLibrary for injection.
    /// </summary>
    internal class SpatialOSBehaviourManager
    {
        private readonly Entity entity;
        private readonly long spatialId;

        private readonly Dictionary<uint, HashSet<MonoBehaviour>> behavioursRequiringReaderTypes
            = new Dictionary<uint, HashSet<MonoBehaviour>>();

        private readonly Dictionary<uint, HashSet<MonoBehaviour>> behavioursRequiringWriterTypes
            = new Dictionary<uint, HashSet<MonoBehaviour>>();

        private readonly Dictionary<MonoBehaviour, int> numUnsatisfiedReadersOrWriters
            = new Dictionary<MonoBehaviour, int>();

        private readonly Dictionary<MonoBehaviour, Dictionary<uint, IReaderInternal>> behaviourToReadersWriters
            = new Dictionary<MonoBehaviour, Dictionary<uint, IReaderInternal>>();

        private readonly Dictionary<uint, HashSet<IReaderInternal>> compIdToReadersWriters =
            new Dictionary<uint, HashSet<IReaderInternal>>();

        private readonly HashSet<MonoBehaviour> behavioursToEnable = new HashSet<MonoBehaviour>();
        private readonly HashSet<MonoBehaviour> behavioursToDisable = new HashSet<MonoBehaviour>();

        private readonly SpatialOSBehaviourLibrary behaviourLibrary;

        private readonly ILogDispatcher logger;

        private const string LoggerName = "SpatialOSBehaviourManager";

        public SpatialOSBehaviourManager(GameObject gameObject, SpatialOSBehaviourLibrary library, ILogDispatcher logger)
        {
            this.logger = logger;
            behaviourLibrary = library;

            var spatialComponent = gameObject.GetComponent<SpatialOSComponent>();
            entity = spatialComponent.Entity;
            spatialId = spatialComponent.SpatialEntityId;

            foreach (var behaviour in gameObject.GetComponents<MonoBehaviour>())
            {
                var readerIds = library.GetRequiredReaderComponentIds(behaviour.GetType());
                foreach (var id in readerIds)
                {
                    GetOrCreateValue(behavioursRequiringReaderTypes, id).Add(behaviour);
                }

                var writerIds = library.GetRequiredWriterComponentIds(behaviour.GetType());
                foreach (var id in writerIds)
                {
                    GetOrCreateValue(behavioursRequiringWriterTypes, id).Add(behaviour);
                }

                numUnsatisfiedReadersOrWriters[behaviour] = readerIds.Count + writerIds.Count;
            }
        }


        public HashSet<IReaderInternal> GetReadersWriters(uint componentId)
        {
            return compIdToReadersWriters[componentId];
        }

        public void EnableSpatialOSBehaviours()
        {
            foreach (var behaviour in behavioursToEnable)
            {
                var dict = behaviourLibrary.InjectAllReadersWriters(behaviour, entity);
                behaviourToReadersWriters[behaviour] = dict;
                foreach (var idToReaderWriter in dict)
                {
                    var id = idToReaderWriter.Key;
                    var reader = idToReaderWriter.Value;
                    GetOrCreateValue(compIdToReadersWriters, id).Add(reader);
                }
            }

            foreach (var behaviour in behavioursToDisable)
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
                behaviourLibrary.DeInjectAllReadersWriters(behaviour);
                foreach (var idToReaderWriter in behaviourToReadersWriters[behaviour])
                {
                    var id = idToReaderWriter.Key;
                    var reader = idToReaderWriter.Value;
                    compIdToReadersWriters[id].Remove(reader);
                }

                behaviourToReadersWriters.Remove(behaviour);
            }

            behavioursToDisable.Clear();
        }

        public void AddComponent(uint componentId)
        {
            // Mark reader components ready in relevant SpatialOSBehaviours
            var relevantReaderSpatialOSBehaviours = behavioursRequiringReaderTypes[componentId];
            MarkComponentRequirementSatisfied(relevantReaderSpatialOSBehaviours);
        }

        public void RemoveComponent(uint componentId)
        {
            // Mark reader components not ready in relevant SpatialOSBehaviours
            var relevantReaderSpatialOSBehaviours = behavioursRequiringReaderTypes[componentId];
            MarkComponentRequirementUnsatisfied(relevantReaderSpatialOSBehaviours);
        }

        public void ChangeAuthority(uint componentId, Authority authority)
        {
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
                    if (!behavioursToEnable.Contains(behaviour))
                    {
                        // Schedule activation
                        behavioursToEnable.Add(behaviour);
                    }
                    else
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
                    if (!behavioursToDisable.Contains(behaviour))
                    {
                        // Schedule deactivation
                        behavioursToDisable.Add(behaviour);
                    }
                    else
                    {
                        // Must be disabled already, so we were going to enable it - let's not
                        behavioursToEnable.Remove(behaviour);
                    }
                }

                numUnsatisfiedReadersOrWriters[behaviour]++;
            }
        }

        private TValue GetOrCreateValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = new TValue();
                dictionary[key] = value;
            }

            return value;
        }
    }
}

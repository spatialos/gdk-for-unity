using System.Collections.Generic;
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

        private readonly Dictionary<uint, HashSet<MonoBehaviour>> componentIdToReaderSpatialOSBehaviours
            = new Dictionary<uint, HashSet<MonoBehaviour>>();

        private readonly Dictionary<uint, HashSet<MonoBehaviour>> componentIdToWriterSpatialOSBehaviours
            = new Dictionary<uint, HashSet<MonoBehaviour>>();

        private readonly Dictionary<MonoBehaviour, int> numUnsatisfiedReadersOrWriters
            = new Dictionary<MonoBehaviour, int>();

        private readonly Dictionary<MonoBehaviour, Dictionary<uint, IReaderInternal>> behaviourToCreatedReadersWriters
            = new Dictionary<MonoBehaviour, Dictionary<uint, IReaderInternal>>();

        private readonly HashSet<MonoBehaviour> spatialOSBehavioursToEnable = new HashSet<MonoBehaviour>();
        private readonly HashSet<MonoBehaviour> spatialOSBehavioursToDisable = new HashSet<MonoBehaviour>();

        private readonly Dictionary<uint, HashSet<IReaderInternal>> componentIdToCreatedReadersWriters =
            new Dictionary<uint, HashSet<IReaderInternal>>();

        private readonly SpatialOSBehaviourLibrary behaviourLibrary;

        private const string LoggerName = "SpatialOSBehaviourManager";

        public SpatialOSBehaviourManager(GameObject gameObject, SpatialOSBehaviourLibrary library)
        {
            behaviourLibrary = library;

            var spatialComponent = gameObject.GetComponent<SpatialOSComponent>();
            entity = spatialComponent.Entity;
            spatialId = spatialComponent.SpatialEntityId;

            foreach (var behaviour in gameObject.GetComponents<MonoBehaviour>())
            {
                var readerIds = library.GetRequiredReaderComponentIds(behaviour.GetType());
                foreach (var id in readerIds)
                {
                    if (!componentIdToReaderSpatialOSBehaviours.ContainsKey(id))
                    {
                        componentIdToReaderSpatialOSBehaviours.Add(id, new HashSet<MonoBehaviour>());
                    }

                    componentIdToReaderSpatialOSBehaviours[id].Add(behaviour);
                }

                var writerIds = library.GetRequiredWriterComponentIds(behaviour.GetType());
                foreach (var id in writerIds)
                {
                    if (!componentIdToWriterSpatialOSBehaviours.ContainsKey(id))
                    {
                        componentIdToWriterSpatialOSBehaviours.Add(id, new HashSet<MonoBehaviour>());
                    }

                    componentIdToWriterSpatialOSBehaviours[id].Add(behaviour);
                }

                numUnsatisfiedReadersOrWriters[behaviour] = readerIds.Count + writerIds.Count;
            }
        }


        public HashSet<IReaderInternal> GetReadersWriters(uint componentId)
        {
            return componentIdToCreatedReadersWriters[componentId];
        }

        public void EnableSpatialOSBehaviours()
        {
            foreach (var behaviour in spatialOSBehavioursToEnable)
            {
                var dict = behaviourLibrary.InjectAllReadersWriters(behaviour, entity);
                behaviourToCreatedReadersWriters[behaviour] = dict;
                foreach (var idToReaderWriter in dict)
                {
                    var id = idToReaderWriter.Key;
                    var reader = idToReaderWriter.Value;
                    if (!componentIdToCreatedReadersWriters.ContainsKey(id))
                    {
                        componentIdToCreatedReadersWriters[id] = new HashSet<IReaderInternal>();
                    }

                    componentIdToCreatedReadersWriters[id].Add(reader);
                }

                behaviour.enabled = true;
            }

            spatialOSBehavioursToEnable.Clear();
        }

        public void DisableSpatialOSBehaviours()
        {
            foreach (var behaviour in spatialOSBehavioursToDisable)
            {
                behaviour.enabled = false;
                behaviourLibrary.DeInjectAllReadersWriters(behaviour);
                foreach (var idToReaderWriter in behaviourToCreatedReadersWriters[behaviour])
                {
                    var id = idToReaderWriter.Key;
                    var reader = idToReaderWriter.Value;
                    componentIdToCreatedReadersWriters[id].Remove(reader);
                }

                behaviourToCreatedReadersWriters.Remove(behaviour);
            }

            spatialOSBehavioursToDisable.Clear();
        }

        public void AddComponent(uint componentId)
        {
            // Mark reader components ready in relevant SpatialOSBehaviours
            var relevantReaderSpatialOSBehaviours = componentIdToReaderSpatialOSBehaviours[componentId];
            foreach (var spatialOSBehaviour in relevantReaderSpatialOSBehaviours)
            {
                MarkComponentReady(spatialOSBehaviour);
            }
        }

        public void RemoveComponent(uint componentId)
        {
            // Mark reader components not ready in relevant SpatialOSBehaviours
            var relevantReaderSpatialOSBehaviours = componentIdToReaderSpatialOSBehaviours[componentId];
            foreach (var spatialOSBehaviour in relevantReaderSpatialOSBehaviours)
            {
                MarkComponentNotReady(spatialOSBehaviour);
            }
        }

        public void ChangeAuthority(uint componentId, Authority authority)
        {
            if (authority == Authority.Authoritative)
            {
                // Mark writer components ready in relevant SpatialOSBehaviours
                var relevantWriterSpatialOSBehaviours = componentIdToWriterSpatialOSBehaviours[componentId];
                foreach (var spatialOSBehaviour in relevantWriterSpatialOSBehaviours)
                {
                    MarkComponentReady(spatialOSBehaviour);
                }
            }
            else if (authority == Authority.NotAuthoritative)
            {
                // Mark writer components not ready in relevant SpatialOSBehaviours
                var relevantWriterSpatialOSBehaviours = componentIdToWriterSpatialOSBehaviours[componentId];
                foreach (var spatialOSBehaviour in relevantWriterSpatialOSBehaviours)
                {
                    MarkComponentNotReady(spatialOSBehaviour);
                }
            }
        }

        private void MarkComponentReady(MonoBehaviour spatialOSBehaviour)
        {
            // Inject all Readers/Writers at once when all requirements are met
            numUnsatisfiedReadersOrWriters[spatialOSBehaviour]--;
            if (numUnsatisfiedReadersOrWriters[spatialOSBehaviour] == 0)
            {
                // Schedule activation
                if (!spatialOSBehaviour.enabled)
                {
                    spatialOSBehavioursToEnable.Add(spatialOSBehaviour);
                }
                else
                {
                    spatialOSBehavioursToDisable.Remove(spatialOSBehaviour);
                }
            }
        }

        private void MarkComponentNotReady(MonoBehaviour spatialOSBehaviour)
        {
            // De-inject all Readers/Writers at once when a single requirement is not met
            if (numUnsatisfiedReadersOrWriters[spatialOSBehaviour] == 0)
            {
                // Schedule deactivation
                if (spatialOSBehaviour.enabled)
                {
                    spatialOSBehavioursToDisable.Add(spatialOSBehaviour);
                }
                else
                {
                    spatialOSBehavioursToEnable.Remove(spatialOSBehaviour);
                }
            }

            numUnsatisfiedReadersOrWriters[spatialOSBehaviour]++;
        }
    }
}

using System;
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
                    if (!behavioursRequiringReaderTypes.TryGetValue(id, out var behavioursForReader))
                    {
                        behavioursForReader = new HashSet<MonoBehaviour>();
                        behavioursRequiringReaderTypes[id] = behavioursForReader;
                    }

                    behavioursForReader.Add(behaviour);
                }

                var writerIds = library.GetRequiredWriterComponentIds(behaviour.GetType());
                foreach (var id in writerIds)
                {
                    if (!behavioursRequiringWriterTypes.TryGetValue(id, out var behavioursForWriter))
                    {
                        behavioursForWriter = new HashSet<MonoBehaviour>();
                        behavioursRequiringWriterTypes[id] = behavioursForWriter;
                    }

                    behavioursForWriter.Add(behaviour);
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
                    if (!compIdToReadersWriters.TryGetValue(id, out var readersWriters))
                    {
                        readersWriters = new HashSet<IReaderInternal>();
                        compIdToReadersWriters[id] = readersWriters;
                    }

                    readersWriters.Add(reader);
                }

                try
                {
                    behaviour.enabled = true;
                }
                catch (Exception e)
                {
                    logger.HandleLog(LogType.Exception,
                        new LogEvent("Exception thrown in OnEnable() method of MonoBehaviour.")
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(LoggingUtils.EntityId, spatialId)
                            .WithException(e));
                }
            }

            behavioursToEnable.Clear();
        }

        public void DisableSpatialOSBehaviours()
        {
            foreach (var behaviour in behavioursToDisable)
            {
                try
                {
                    behaviour.enabled = false;
                }
                catch (Exception e)
                {
                    logger.HandleLog(LogType.Error,
                        new LogEvent("Exception thrown in OnDisable() method of MonoBehaviour.")
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(LoggingUtils.EntityId, spatialId)
                            .WithException(e));
                }

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
            MarkComponentsReady(relevantReaderSpatialOSBehaviours);
        }

        public void RemoveComponent(uint componentId)
        {
            // Mark reader components not ready in relevant SpatialOSBehaviours
            var relevantReaderSpatialOSBehaviours = behavioursRequiringReaderTypes[componentId];
            MarkComponentsNotReady(relevantReaderSpatialOSBehaviours);
        }

        public void ChangeAuthority(uint componentId, Authority authority)
        {
            if (authority == Authority.Authoritative)
            {
                // Mark writer components ready in relevant SpatialOSBehaviours
                var relevantWriterSpatialOSBehaviours = behavioursRequiringWriterTypes[componentId];
                MarkComponentsReady(relevantWriterSpatialOSBehaviours);
            }
            else if (authority == Authority.NotAuthoritative)
            {
                // Mark writer components not ready in relevant SpatialOSBehaviours
                var relevantWriterSpatialOSBehaviours = behavioursRequiringWriterTypes[componentId];
                MarkComponentsNotReady(relevantWriterSpatialOSBehaviours);
            }
        }

        private void MarkComponentsReady(IEnumerable<MonoBehaviour> behaviours)
        {
            // Inject all Readers/Writers at once when all requirements are met
            foreach (var behaviour in behaviours)
            {
                numUnsatisfiedReadersOrWriters[behaviour]--;
                if (numUnsatisfiedReadersOrWriters[behaviour] == 0)
                {
                    // Schedule activation
                    if (!behaviour.enabled)
                    {
                        behavioursToEnable.Add(behaviour);
                    }
                    else
                    {
                        behavioursToDisable.Remove(behaviour);
                    }
                }
            }
        }

        private void MarkComponentsNotReady(IEnumerable<MonoBehaviour> behaviours)
        {
            foreach (var behaviour in behaviours)
            {
                // De-inject all Readers/Writers at once when a single requirement is not met
                if (numUnsatisfiedReadersOrWriters[behaviour] == 0)
                {
                    // Schedule deactivation
                    if (behaviour.enabled)
                    {
                        behavioursToDisable.Add(behaviour);
                    }
                    else
                    {
                        behavioursToEnable.Remove(behaviour);
                    }
                }

                numUnsatisfiedReadersOrWriters[behaviour]++;
            }
        }
    }
}

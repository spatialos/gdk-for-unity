﻿using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    /// Retrieves Reader and Writer fields from MonoBehaviours and handles injection into them.
    /// </summary>
    public class SpatialOSBehaviourLibrary {
        // Stores maps from Spatial component IDs to MemberAdapters to reflectively set Readers/Writers for each SpatialOSBehaviour
        private readonly Dictionary<Type, Dictionary<uint, IMemberAdapter>> adapterCache
            = new Dictionary<Type, Dictionary<uint, IMemberAdapter>>();

        // List of component IDs for which a reader/writer is required for each SpatialOSBehaviour
        private readonly Dictionary<Type, List<uint>> componentReaderIdsForBehaviours = new Dictionary<Type, List<uint>>();
        private readonly Dictionary<Type, List<uint>> componentWriterIdsForBehaviours = new Dictionary<Type, List<uint>>();

        // Helper objects for querying members with the [Require] attribute
        private readonly MemberReflectionUtil reflectionUtil = new MemberReflectionUtil(typeof(RequireAttribute));

        private readonly ILogDispatcher logger;
        private const string LoggerName = "SpatialOSBehaviourLibrary";

        private const string BadRequiredMemberWarning
            = "[Require] attribute found on member that is not Reader or Writer, ignoring!";

        public SpatialOSBehaviourLibrary(World world)
        {
            logger = WorkerRegistry.GetWorkerForWorld(world).View.LogDispatcher;
        }

        public void InjectAllReadersWriters(MonoBehaviour spatialOSBehaviour)
        {
            EnsureLoaded(spatialOSBehaviour.GetType());
            foreach (var readerWriterComponentId in componentReaderIdsForBehaviours[spatialOSBehaviour.GetType()])
            {
                Inject(spatialOSBehaviour, readerWriterComponentId);
            }

            foreach (var readerWriterComponentId in componentWriterIdsForBehaviours[spatialOSBehaviour.GetType()])
            {
                Inject(spatialOSBehaviour, readerWriterComponentId);
            }
        }

        public void DeInjectAllReadersWriters(MonoBehaviour spatialOSBehaviour)
        {
            EnsureLoaded(spatialOSBehaviour.GetType());
            foreach (var readerWriterComponentId in componentReaderIdsForBehaviours[spatialOSBehaviour.GetType()])
            {
                DeInject(spatialOSBehaviour, readerWriterComponentId);
            }

            foreach (var readerWriterComponentId in componentWriterIdsForBehaviours[spatialOSBehaviour.GetType()])
            {
                DeInject(spatialOSBehaviour, readerWriterComponentId);
            }
        }

        public IEnumerable<uint> GetRequiredReaderComponentIds(Type behaviourType)
        {
            EnsureLoaded(behaviourType);
            return componentReaderIdsForBehaviours[behaviourType];
        }

        public IEnumerable<uint> GetRequiredWriterComponentIds(Type behaviourType)
        {
            EnsureLoaded(behaviourType);
            return componentWriterIdsForBehaviours[behaviourType];
        }

        private void Inject(MonoBehaviour spatialOSBehaviour, uint componentId)
        {
            var readerWriter = ReaderWriterFactory.CreateReaderWriter(componentId);
            var memberAdapter = adapterCache[spatialOSBehaviour.GetType()][componentId];
            memberAdapter.SetValue(spatialOSBehaviour, readerWriter);
        }

        private void DeInject(MonoBehaviour spatialOSBehaviour, uint componentId)
        {
            var memberAdapter = adapterCache[spatialOSBehaviour.GetType()][componentId];
            memberAdapter.SetValue(spatialOSBehaviour, null);
        }

        private void EnsureLoaded(Type behaviourType)
        {
            if (adapterCache.ContainsKey(behaviourType))
            {
                return;
            }

            var adapters = reflectionUtil.GetMembersWithMatchingAttributes(behaviourType);
            var componentIdsToAdapters = new Dictionary<uint, IMemberAdapter>();
            var readerIds = new List<uint>();
            var writerIds = new List<uint>();
            adapterCache[behaviourType] = componentIdsToAdapters;
            componentReaderIdsForBehaviours[behaviourType] = readerIds;
            componentWriterIdsForBehaviours[behaviourType] = writerIds;
            foreach (var adapter in adapters)
            {
                // Figure out if reader or writer
                // Get component ID
                // Store in data structures
                Type requiredType = adapter.TypeOfMember;
                var componentId =
                    ((ComponentIdAttribute) Attribute.GetCustomAttribute(requiredType, typeof(ComponentIdAttribute),
                        false)).Id;
                componentIdsToAdapters[componentId] = adapter;
                if (Attribute.IsDefined(requiredType, typeof(ReaderInterfaceAttribute), false))
                {
                    readerIds.Add(componentId);
                }
                else if (Attribute.IsDefined(requiredType, typeof(WriterInterfaceAttribute), false))
                {
                    writerIds.Add(componentId);
                }
                else
                {
                    logger.HandleLog(LogType.Warning, new LogEvent(BadRequiredMemberWarning)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("SpatialOSBehaviour", behaviourType.Name)
                        .WithField("RequiredType", requiredType.Name));
                }
            }
        }
    }
}



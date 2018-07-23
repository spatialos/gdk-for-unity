using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    /// Retrieves Reader and Writer fields from MonoBehaviours and handles injection into them.
    /// </summary>
    public class SpatialOSBehaviourLibrary {
        private readonly Dictionary<Type, Dictionary<uint, FieldInfo>> adapterCache
            = new Dictionary<Type, Dictionary<uint, FieldInfo>>();

        private readonly Dictionary<Type, List<uint>> componentReaderIdsForBehaviours = new Dictionary<Type, List<uint>>();
        private readonly Dictionary<Type, List<uint>> componentWriterIdsForBehaviours = new Dictionary<Type, List<uint>>();

        private readonly HashSet<Type> invalidMonoBehaviourTypes = new HashSet<Type>();

        private readonly ILogDispatcher logger;
        private const string LoggerName = "SpatialOSBehaviourLibrary";

        private const string BadRequiredMemberWarning
            = "[Require] attribute found on member that is not Reader or Writer or incorrectly generated, ignoring this member!";

        private const string MultipleReadersWritersRequiredError
            = "MonoBehaviour found requesting more than one Reader or Writer for the same component, " +
            "this is invalid and will not be enabled!";

        private const string MalformedReaderOrWriter
            = "Reader or Writer found without a Component ID attribute, this is invalid!";

        public SpatialOSBehaviourLibrary(ILogDispatcher logger)
        {
            this.logger = logger;
        }

        public void InjectAllReadersWriters(MonoBehaviour spatialOSBehaviour)
        {
            EnsureLoaded(spatialOSBehaviour.GetType());
            if (invalidMonoBehaviourTypes.Contains(spatialOSBehaviour.GetType()))
            {
                return;
            }

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
            if (invalidMonoBehaviourTypes.Contains(spatialOSBehaviour.GetType()))
            {
                return;
            }

            foreach (var readerWriterComponentId in componentReaderIdsForBehaviours[spatialOSBehaviour.GetType()])
            {
                DeInject(spatialOSBehaviour, readerWriterComponentId);
            }

            foreach (var readerWriterComponentId in componentWriterIdsForBehaviours[spatialOSBehaviour.GetType()])
            {
                DeInject(spatialOSBehaviour, readerWriterComponentId);
            }
        }

        public List<uint> GetRequiredReaderComponentIds(Type behaviourType)
        {
            EnsureLoaded(behaviourType);
            return componentReaderIdsForBehaviours[behaviourType];
        }

        public List<uint> GetRequiredWriterComponentIds(Type behaviourType)
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

            var adapters = GetMembersWithMatchingAttributes(behaviourType);
            var componentIdsToAdapters = new Dictionary<uint, FieldInfo>();
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
                Type requiredType = adapter.FieldType;
                var isReader = Attribute.IsDefined(requiredType, typeof(ReaderInterfaceAttribute), false);
                var isWriter = Attribute.IsDefined(requiredType, typeof(WriterInterfaceAttribute), false);
                if (!isReader && !isWriter)
                {
                    logger.HandleLog(LogType.Warning, new LogEvent(BadRequiredMemberWarning)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("MonoBehaviour", behaviourType.Name)
                        .WithField("RequiredType", requiredType.Name));
                    continue;
                }

                var componentIdAttribute =
                    (ComponentIdAttribute) Attribute.GetCustomAttribute(requiredType, typeof(ComponentIdAttribute), false);
                if (componentIdAttribute == null)
                {
                    logger.HandleLog(LogType.Error, new LogEvent(MalformedReaderOrWriter)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("MonoBehaviour", behaviourType.Name)
                        .WithField("RequiredType", requiredType.Name));
                    continue;
                }

                var componentId = componentIdAttribute.Id;
                if (componentIdsToAdapters.ContainsKey(componentId))
                {
                    logger.HandleLog(LogType.Error, new LogEvent(MultipleReadersWritersRequiredError)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("MonoBehaviour", behaviourType.Name)
                        .WithField("ComponentID", componentId)
                        .WithField("RequiredType", requiredType.Name));
                    invalidMonoBehaviourTypes.Add(behaviourType);
                    break;
                }

                componentIdsToAdapters[componentId] = adapter;
                if (isReader)
                {
                    readerIds.Add(componentId);
                }
                else
                {
                    writerIds.Add(componentId);
                }
            }
        }

        private const BindingFlags MemberFlags = BindingFlags.Instance | BindingFlags.NonPublic |
            BindingFlags.Public;

        private List<FieldInfo> GetMembersWithMatchingAttributes(Type targetType)
        {
            List<FieldInfo> adapters = new List<FieldInfo>();
            foreach (var field in targetType.GetFields(MemberFlags))
            {
                if (Attribute.IsDefined(field, typeof(RequireAttribute), false))
                {
                    adapters.Add(field);
                }
            }

            return adapters;
        }
    }
}



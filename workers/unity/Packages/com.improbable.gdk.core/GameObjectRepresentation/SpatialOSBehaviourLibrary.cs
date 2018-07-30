using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    /// <summary>
    ///     Retrieves Reader and Writer fields from MonoBehaviours and handles injection into them.
    /// </summary>
    public class SpatialOSBehaviourLibrary
    {
        private readonly Dictionary<Type, Dictionary<uint, FieldInfo>> fieldInfoCache
            = new Dictionary<Type, Dictionary<uint, FieldInfo>>();

        private readonly Dictionary<Type, List<uint>> componentReaderIdsForBehaviours =
            new Dictionary<Type, List<uint>>();

        private readonly Dictionary<Type, List<uint>> componentWriterIdsForBehaviours =
            new Dictionary<Type, List<uint>>();

        private readonly HashSet<Type> invalidMonoBehaviourTypes = new HashSet<Type>();

        private readonly ILogDispatcher logger;
        private const string LoggerName = "SpatialOSBehaviourLibrary";

        private const string BadRequiredMemberWarning
            = "[Require] attribute found on member that is not Reader or Writer. This member will be ignored.";

        private const string MultipleReadersWritersRequiredError
            = "MonoBehaviour found requesting more than one Reader or Writer for the same component. " +
            "This MonoBehaviour will not be enabled.";

        private const string MalformedReaderOrWriter
            = "Reader or Writer found without a Component ID attribute, this is invalid.";

        public SpatialOSBehaviourLibrary(ILogDispatcher logger)
        {
            this.logger = logger;
        }

        public void InjectAllReadersWriters(MonoBehaviour spatialOSBehaviour)
        {
            var spatialOSBehaviourType = spatialOSBehaviour.GetType();
            EnsureLoaded(spatialOSBehaviourType);
            if (invalidMonoBehaviourTypes.Contains(spatialOSBehaviourType))
            {
                return;
            }

            foreach (var readerWriterComponentId in componentReaderIdsForBehaviours[spatialOSBehaviourType])
            {
                Inject(spatialOSBehaviour, readerWriterComponentId);
            }

            foreach (var readerWriterComponentId in componentWriterIdsForBehaviours[spatialOSBehaviourType])
            {
                Inject(spatialOSBehaviour, readerWriterComponentId);
            }
        }

        public void DeInjectAllReadersWriters(MonoBehaviour spatialOSBehaviour)
        {
            var spatialOSBehaviourType = spatialOSBehaviour.GetType();
            EnsureLoaded(spatialOSBehaviourType);
            if (invalidMonoBehaviourTypes.Contains(spatialOSBehaviourType))
            {
                return;
            }

            foreach (var readerWriterComponentId in componentReaderIdsForBehaviours[spatialOSBehaviourType])
            {
                DeInject(spatialOSBehaviour, readerWriterComponentId);
            }

            foreach (var readerWriterComponentId in componentWriterIdsForBehaviours[spatialOSBehaviourType])
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
            var field = fieldInfoCache[spatialOSBehaviour.GetType()][componentId];
            field.SetValue(spatialOSBehaviour, readerWriter);
        }

        private void DeInject(MonoBehaviour spatialOSBehaviour, uint componentId)
        {
            var field = fieldInfoCache[spatialOSBehaviour.GetType()][componentId];
            field.SetValue(spatialOSBehaviour, null);
        }

        private void EnsureLoaded(Type behaviourType)
        {
            if (fieldInfoCache.ContainsKey(behaviourType))
            {
                return;
            }

            var fieldInfos = GetFieldsWithRequireAttribute(behaviourType);
            var componentIdsToFieldInfos = new Dictionary<uint, FieldInfo>();
            var readerComponentIds = new List<uint>();
            var writerComponentIds = new List<uint>();
            foreach (var field in fieldInfos)
            {
                // Figure out if reader or writer
                Type requiredType = field.FieldType;
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

                // Get component ID
                var componentIdAttribute =
                    (ComponentIdAttribute) Attribute.GetCustomAttribute(requiredType, typeof(ComponentIdAttribute),
                        false);
                if (componentIdAttribute == null)
                {
                    logger.HandleLog(LogType.Error, new LogEvent(MalformedReaderOrWriter)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("MonoBehaviour", behaviourType.Name)
                        .WithField("RequiredType", requiredType.Name));
                    continue;
                }

                var componentId = componentIdAttribute.Id;
                if (componentIdsToFieldInfos.ContainsKey(componentId))
                {
                    logger.HandleLog(LogType.Error, new LogEvent(MultipleReadersWritersRequiredError)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("MonoBehaviour", behaviourType.Name)
                        .WithField("ComponentID", componentId)
                        .WithField("RequiredType", requiredType.Name));
                    invalidMonoBehaviourTypes.Add(behaviourType);
                    break;
                }

                // Store in data structures
                componentIdsToFieldInfos[componentId] = field;
                if (isReader)
                {
                    readerComponentIds.Add(componentId);
                }
                else
                {
                    writerComponentIds.Add(componentId);
                }
            }

            fieldInfoCache[behaviourType] = componentIdsToFieldInfos;
            componentReaderIdsForBehaviours[behaviourType] = readerComponentIds;
            componentWriterIdsForBehaviours[behaviourType] = writerComponentIds;
        }

        private const BindingFlags MemberFlags = BindingFlags.Instance | BindingFlags.NonPublic |
            BindingFlags.Public;

        private List<FieldInfo> GetFieldsWithRequireAttribute(Type targetType)
        {
            List<FieldInfo> fields = new List<FieldInfo>();
            foreach (var field in targetType.GetFields(MemberFlags))
            {
                if (Attribute.IsDefined(field, typeof(RequireAttribute), false))
                {
                    fields.Add(field);
                }
            }

            return fields;
        }
    }
}

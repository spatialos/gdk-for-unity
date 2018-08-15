using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Retrieves fields with [Require] tags from MonoBehaviours and handles injection into them.
    /// </summary>
    internal class RequiredFieldInjector
    {
        private readonly Dictionary<Type, Dictionary<uint, FieldInfo[]>> fieldInfoCache
            = new Dictionary<Type, Dictionary<uint, FieldInfo[]>>();
        private readonly Dictionary<Type, List<uint>> componentReaderIdsForBehaviours =
            new Dictionary<Type, List<uint>>();
        private readonly Dictionary<Type, List<uint>> componentWriterIdsForBehaviours =
            new Dictionary<Type, List<uint>>();

        private readonly ILogDispatcher logger;
        private readonly ReaderWriterFactory readerWriterFactory;

        private const string LoggerName = nameof(RequiredFieldInjector);
        private const string BadRequiredMemberWarning
            = "[Require] attribute found on member that is not Reader or Writer. This member will be ignored.";
        private const string MalformedReaderOrWriter
            = "Reader or Writer found without a Component ID attribute, this is invalid.";

        public RequiredFieldInjector(EntityManager entityManager, ILogDispatcher logger)
        {
            this.logger = logger;
            this.readerWriterFactory = new ReaderWriterFactory(entityManager, logger);
        }

        public Dictionary<uint, IReaderWriterInternal[]> InjectAllReadersWriters(MonoBehaviour behaviour, Entity entity)
        {
            var behaviourType = behaviour.GetType();
            EnsureLoaded(behaviourType);
            var createdReaderWriters = new Dictionary<uint, IReaderWriterInternal[]>();
            foreach (var idToFields in fieldInfoCache[behaviourType])
            {
                var id = idToFields.Key;
                var fields = idToFields.Value;
                createdReaderWriters[id] = fields.Select(field => Inject(behaviour, id, entity, field)).ToArray();
            }

            return createdReaderWriters;
        }

        public void DeInjectAllReadersWriters(MonoBehaviour behaviour)
        {
            var behaviourType = behaviour.GetType();
            EnsureLoaded(behaviourType);

            foreach (var readerWriterComponentId in componentReaderIdsForBehaviours[behaviourType])
            {
                DeInject(behaviour, readerWriterComponentId);
            }

            foreach (var readerWriterComponentId in componentWriterIdsForBehaviours[behaviourType])
            {
                DeInject(behaviour, readerWriterComponentId);
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

        private IReaderWriterInternal Inject(MonoBehaviour behaviour, uint componentId, Entity entity, FieldInfo field)
        {
            var readerWriter = readerWriterFactory.CreateReaderWriter(componentId, entity);
            field.SetValue(behaviour, readerWriter);
            return readerWriter;
        }

        private void DeInject(MonoBehaviour spatialOSBehaviour, uint componentId)
        {
            foreach (var field in fieldInfoCache[spatialOSBehaviour.GetType()][componentId])
            {
                field.SetValue(spatialOSBehaviour, null);
            }
        }

        private void EnsureLoaded(Type behaviourType)
        {
            if (fieldInfoCache.ContainsKey(behaviourType))
            {
                return;
            }

            var fieldInfos = GetFieldsWithRequireAttribute(behaviourType);
            var componentIdsToFieldInfos = new Dictionary<uint, List<FieldInfo>>();
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

                // Store in data structures
                if (!componentIdsToFieldInfos.TryGetValue(componentId, out var fieldInfosForType))
                {
                    fieldInfosForType = new List<FieldInfo>();
                    componentIdsToFieldInfos[componentId] = fieldInfosForType;
                }

                fieldInfosForType.Add(field);

                if (isReader)
                {
                    readerComponentIds.Add(componentId);
                }
                else
                {
                    writerComponentIds.Add(componentId);
                }
            }

            fieldInfoCache[behaviourType] = componentIdsToFieldInfos.ToDictionary
                (kp => kp.Key, kp => kp.Value.ToArray());
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

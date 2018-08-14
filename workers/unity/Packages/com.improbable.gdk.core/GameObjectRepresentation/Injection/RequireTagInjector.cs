using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Retrieves Reader and Writer fields from MonoBehaviours and handles injection into them.
    /// </summary>
    internal class RequireTagInjector
    {
        private readonly Dictionary<Type, Dictionary<uint, FieldInfo[]>> fieldInfoCache
            = new Dictionary<Type, Dictionary<uint, FieldInfo[]>>();
        private readonly Dictionary<Type, List<uint>> componentReaderIdsForBehaviours =
            new Dictionary<Type, List<uint>>();
        private readonly Dictionary<Type, List<uint>> componentWriterIdsForBehaviours =
            new Dictionary<Type, List<uint>>();

        private readonly ILogDispatcher logger;
        private readonly ReaderWriterFactory readerWriterFactory;

        private const string LoggerName = nameof(RequireTagInjector);
        private const string BadRequiredMemberWarning
            = "[Require] attribute found on member that is not Reader or Writer. This member will be ignored.";
        private const string MultipleReadersWritersRequiredError
            = "MonoBehaviour found requesting more than one Reader or Writer for the same component. " +
            "This MonoBehaviour will not be enabled.";
        private const string MalformedReaderOrWriter
            = "Reader or Writer found without a Component ID attribute, this is invalid.";

        public RequireTagInjector(EntityManager entityManager, ILogDispatcher logger)
        {
            this.logger = logger;
            this.readerWriterFactory = new ReaderWriterFactory(entityManager, logger);
        }

        public Dictionary<uint, IReaderWriterInternal[]> InjectAllReadersWriters(MonoBehaviour behaviour, Entity entity)
        {
            var behaviourType = behaviour.GetType();
            EnsureLoaded(behaviourType);
            var createdReaderWriters = new Dictionary<uint, List<IReaderWriterInternal>>();

            foreach (var componentId in componentReaderIdsForBehaviours[behaviourType])
            {
                List<IReaderWriterInternal> readerWritersForComp = new List<IReaderWriterInternal>();
                createdReaderWriters[componentId] = readerWritersForComp;
                Inject(behaviour, componentId, entity, readerWritersForComp);
            }

            foreach (var componentId in componentWriterIdsForBehaviours[behaviourType])
            {
                if (!createdReaderWriters.TryGetValue(componentId, out var readerWritersForComp))
                {
                    readerWritersForComp = new List<IReaderWriterInternal>();
                }

                Inject(behaviour, componentId, entity, readerWritersForComp);
            }

            return createdReaderWriters.ToDictionary(kp => kp.Key, kp => kp.Value.ToArray());
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

        private void Inject(MonoBehaviour behaviour, uint componentId, Entity entity,
            IList<IReaderWriterInternal> store)
        {
            foreach (var field in fieldInfoCache[behaviour.GetType()][componentId])
            {
                var readerWriter = readerWriterFactory.CreateReaderWriter(componentId, entity);
                field.SetValue(behaviour, readerWriter);
                store.Add(readerWriter);
            }
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

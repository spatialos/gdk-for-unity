using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.MonoBehaviours;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class ReaderWriterFactory
    {
        private const string ComponentIdAttributeNotFoundError
            = "ReaderWriterCreator found with no Component ID attribute.";
        private const string NoReaderWriterCreatorFoundForComponentIdError
            = "No ReaderWriterCreator found for componentId.";

        private readonly EntityManager entityManager;
        private readonly ILogDispatcher logger;
        private readonly Dictionary<uint, IReaderWriterCreator> componentIdToReaderWriterCreator = new Dictionary<uint, IReaderWriterCreator>();

        public ReaderWriterFactory(EntityManager entityManager, ILogDispatcher logger)
        {
            this.entityManager = entityManager;
            this.logger = logger;

            FindReaderWriterCreators();
        }

        private void FindReaderWriterCreators()
        {
            var readerWriterCreatorTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IReaderWriterCreator).IsAssignableFrom(type) && type.IsClass)
                .ToList();

            foreach (var readerWriterCreatorType in readerWriterCreatorTypes)
            {
                var componentIdAttribute =
                    (ComponentIdAttribute)Attribute.GetCustomAttribute(readerWriterCreatorType, typeof(ComponentIdAttribute), false);
                if (componentIdAttribute == null)
                {
                    logger.HandleLog(LogType.Error, new LogEvent(ComponentIdAttributeNotFoundError)
                        .WithField(LoggingUtils.LoggerName, GetType())
                        .WithField("ReaderWriterCreatorType", readerWriterCreatorType));
                    continue;
                }
                var componentId = componentIdAttribute.Id;
                var readerWriterCreator = (IReaderWriterCreator)Activator.CreateInstance(readerWriterCreatorType);
                componentIdToReaderWriterCreator[componentId] = readerWriterCreator;
            }
        }

        public object CreateReaderWriter(uint componentId, Entity entity)
        {
            if (!componentIdToReaderWriterCreator.ContainsKey(componentId))
            {
                logger.HandleLog(LogType.Error, new LogEvent(NoReaderWriterCreatorFoundForComponentIdError)
                    .WithField(LoggingUtils.LoggerName, GetType())
                    .WithField("componentId", componentId));
                return null;
            }

            return componentIdToReaderWriterCreator[componentId].CreateReaderWriter(entity, entityManager, logger);
        }
    }
}

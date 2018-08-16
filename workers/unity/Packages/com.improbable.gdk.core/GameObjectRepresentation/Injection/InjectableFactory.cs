using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Creates IInjectables given an InjectableId and an Entity with which it should be associated.
    /// </summary>
    public class InjectableFactory
    {
        private const string InjectableIdAttributeNotFoundError
            = "ReaderWriterCreator found with no Injectable ID attribute.";
        private const string NoReaderWriterCreatorFoundForComponentIdError
            = "No ReaderWriterCreator found for given ComponentId.";

        private readonly EntityManager entityManager;
        private readonly ILogDispatcher logger;
        private readonly Dictionary<InjectableId, IReaderWriterCreator> injectableIdToReaderWriterCreator = new Dictionary<InjectableId, IReaderWriterCreator>();

        public InjectableFactory(EntityManager entityManager, ILogDispatcher logger)
        {
            this.entityManager = entityManager;
            this.logger = logger;

            FindInjectableCreators();
        }

        private void FindInjectableCreators()
        {
            var creatorTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IReaderWriterCreator).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .ToList();

            foreach (var creatorType in creatorTypes)
            {
                var injectableIdAttribute =
                    (InjectableIdAttribute)Attribute.GetCustomAttribute(creatorType, typeof(InjectableIdAttribute), false);
                if (injectableIdAttribute == null)
                {
                    logger.HandleLog(LogType.Error, new LogEvent(InjectableIdAttributeNotFoundError)
                        .WithField(LoggingUtils.LoggerName, GetType())
                        .WithField("ReaderWriterCreatorType", creatorType));
                    continue;
                }

                var injectableId = injectableIdAttribute.Id;
                var readerWriterCreator = (IReaderWriterCreator) Activator.CreateInstance(creatorType);
                injectableIdToReaderWriterCreator[injectableId] = readerWriterCreator;
            }
        }

        internal IInjectable CreateInjectable(InjectableId injectableId, Entity entity)
        {
            if (!injectableIdToReaderWriterCreator.ContainsKey(injectableId))
            {
                logger.HandleLog(LogType.Error, new LogEvent(NoReaderWriterCreatorFoundForComponentIdError)
                    .WithField(LoggingUtils.LoggerName, GetType())
                    .WithField("ComponentId", injectableId));
                return null;
            }

            return injectableIdToReaderWriterCreator[injectableId].CreateReaderWriter(entity, entityManager, logger);
        }
    }
}

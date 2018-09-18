using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Creates IInjectables given an InjectableId and an Entity with which it should be associated.
    /// </summary>
    public class InjectableFactory
    {
        private const string InjectableIdAttributeNotFoundError
            = "InjectableCreator found with no Injectable ID attribute.";

        private const string NoReaderWriterCreatorFoundForInjectableIdError
            = "No InjectableCreator found for given InjectableId.";

        private readonly EntityManager entityManager;
        private readonly ILogDispatcher logger;
        private readonly Dictionary<InjectableId, IInjectableCreator> injectableIdToReaderWriterCreator = new Dictionary<InjectableId, IInjectableCreator>();

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
                .Where(type => typeof(IInjectableCreator).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .ToList();

            foreach (var creatorType in creatorTypes)
            {
                var injectableIdAttribute =
                    (InjectableIdAttribute) Attribute.GetCustomAttribute(creatorType, typeof(InjectableIdAttribute), false);
                if (injectableIdAttribute == null)
                {
                    logger.HandleLog(LogType.Error, new LogEvent(InjectableIdAttributeNotFoundError)
                        .WithField(LoggingUtils.LoggerName, GetType())
                        .WithField("ReaderWriterCreatorType", creatorType));
                    continue;
                }

                var injectableId = injectableIdAttribute.Id;
                var IInjectableCreator = (IInjectableCreator) Activator.CreateInstance(creatorType);
                injectableIdToReaderWriterCreator[injectableId] = IInjectableCreator;
            }
        }

        internal IInjectable CreateInjectable(InjectableId injectableId, Entity entity)
        {
            if (!injectableIdToReaderWriterCreator.ContainsKey(injectableId))
            {
                logger.HandleLog(LogType.Error, new LogEvent(NoReaderWriterCreatorFoundForInjectableIdError)
                    .WithField(LoggingUtils.LoggerName, GetType())
                    .WithField("ComponentId", injectableId));
                return null;
            }

            return injectableIdToReaderWriterCreator[injectableId].CreateInjectable(entity, entityManager, logger);
        }
    }
}

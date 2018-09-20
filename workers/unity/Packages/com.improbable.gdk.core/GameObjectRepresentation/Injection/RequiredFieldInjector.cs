using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Retrieves fields with [Require] tags from MonoBehaviours and handles injection into them.
    /// </summary>
    public class RequiredFieldInjector
    {
        private readonly Dictionary<Type, Dictionary<InjectableId, FieldInfo[]>> fieldInfoCache
            = new Dictionary<Type, Dictionary<InjectableId, FieldInfo[]>>();

        private readonly Dictionary<Type, List<uint>> componentPresentRequirementsForBehaviours =
            new Dictionary<Type, List<uint>>();

        private readonly Dictionary<Type, List<uint>> componentAuthRequirementsForBehaviours =
            new Dictionary<Type, List<uint>>();
        private readonly Dictionary<Type, string[]> workerTypeRequirementsForBehaviours =
            new Dictionary<Type, string[]>();

        private readonly ILogDispatcher logger;
        private readonly InjectableFactory injectableFactory;

        private const string LoggerName = nameof(RequiredFieldInjector);

        private const string BadRequiredMemberWarning
            = "[Require] attribute found on member that is not Injectable. This member will be ignored. "
            + "Please make sure that types marked with [Require] are located in the <component name>.Requirable or WorldsCommands.Requirable namespace.";

        private const string MalformedInjectable
            = "Injectable found without required attributes, this is invalid.";

        private const string RequirableFieldDoesNotInheritRequirableBase
            = "[Require] field element does not inherit RequirableBase. This is most likely a bug in the SpatialOS GDK.";

        public RequiredFieldInjector(EntityManager entityManager, ILogDispatcher logger)
        {
            this.logger = logger;
            injectableFactory = new InjectableFactory(entityManager, logger);
        }

        public bool IsSpatialOSBehaviour(Type behaviourType)
        {
            EnsureLoaded(behaviourType);
            return workerTypeRequirementsForBehaviours[behaviourType] != null || fieldInfoCache[behaviourType].Count > 0;
        }

        public Dictionary<InjectableId, IInjectable[]> InjectAllRequiredFields(MonoBehaviour behaviour, Entity entity)
        {
            var behaviourType = behaviour.GetType();
            EnsureLoaded(behaviourType);
            var createdInjectables = new Dictionary<InjectableId, IInjectable[]>();
            foreach (var idToFields in fieldInfoCache[behaviourType])
            {
                var id = idToFields.Key;
                var fields = idToFields.Value;
                var injectables = new IInjectable[fields.Length];
                for (var i = 0; i < fields.Length; i++)
                {
                    injectables[i] = Inject(behaviour, id, entity, fields[i]);
                }

                createdInjectables[id] = injectables;
            }

            return createdInjectables;
        }

        private IInjectable Inject(MonoBehaviour behaviour, InjectableId injectableId, Entity entity, FieldInfo field)
        {
            var injectable = injectableFactory.CreateInjectable(injectableId, entity);
            field.SetValue(behaviour, injectable);
            return injectable;
        }

        public void DisposeAllRequiredFields(MonoBehaviour behaviour)
        {
            var behaviourType = behaviour.GetType();
            EnsureLoaded(behaviourType);
            foreach (var fieldsToComponents in fieldInfoCache[behaviourType])
            {
                var fields = fieldsToComponents.Value;
                foreach (var field in fields)
                {
                    var requirableToBeDisposed = field.GetValue(behaviour) as RequirableBase;
                    if (requirableToBeDisposed == null)
                    {
                        logger.HandleLog(LogType.Error, new LogEvent(RequirableFieldDoesNotInheritRequirableBase)
                            .WithField("Behaviour", behaviour)
                            .WithField("Field", field.Name));
                    }
                    else
                    {
                        requirableToBeDisposed.Dispose();
                    }
                }
            }
        }

        public void DeInjectAllRequiredFields(MonoBehaviour behaviour)
        {
            var behaviourType = behaviour.GetType();
            EnsureLoaded(behaviourType);
            foreach (var fieldsToComponents in fieldInfoCache[behaviourType])
            {
                var fields = fieldsToComponents.Value;
                foreach (var field in fields)
                {
                    field.SetValue(behaviour, null);
                }
            }
        }

        public List<uint> GetComponentPresenceRequirements(Type behaviourType)
        {
            EnsureLoaded(behaviourType);
            return componentPresentRequirementsForBehaviours[behaviourType];
        }

        public List<uint> GetComponentAuthorityRequirements(Type behaviourType)
        {
            EnsureLoaded(behaviourType);
            return componentAuthRequirementsForBehaviours[behaviourType];
        }

        public string[] GetComponentWorkerTypeRequirementsForBehaviours(Type behaviourType)
        {
            EnsureLoaded(behaviourType);
            return workerTypeRequirementsForBehaviours[behaviourType];
        }

        private void EnsureLoaded(Type behaviourType)
        {
            if (fieldInfoCache.ContainsKey(behaviourType))
            {
                return;
            }

            var fieldInfos = GetFieldsWithRequireAttribute(behaviourType);
            var injectableIdsToFieldInfos = new Dictionary<InjectableId, List<FieldInfo>>();
            var componentsRequiredPresent = new HashSet<uint>();
            var componentsRequiredWithAuthority = new HashSet<uint>();
            foreach (var field in fieldInfos)
            {
                // Confirm it's IInjectable
                Type requiredType = field.FieldType;
                if (!typeof(IInjectable).IsAssignableFrom(requiredType))
                {
                    logger.HandleLog(LogType.Warning, new LogEvent(BadRequiredMemberWarning)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("MonoBehaviour", behaviourType.Name)
                        .WithField("RequiredType", requiredType.Name));
                    continue;
                }

                // Get injectable ID and injection condition
                var injectableIdAttribute =
                    (InjectableIdAttribute) Attribute.GetCustomAttribute(requiredType, typeof(InjectableIdAttribute),
                        false);
                var conditionAttribute =
                    (InjectionConditionAttribute) Attribute.GetCustomAttribute(requiredType,
                        typeof(InjectionConditionAttribute), false);
                if (injectableIdAttribute == null || conditionAttribute == null)
                {
                    logger.HandleLog(LogType.Error, new LogEvent(MalformedInjectable)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("MonoBehaviour", behaviourType.Name)
                        .WithField("RequiredType", requiredType.Name));
                    continue;
                }

                // Store in data structures
                var injectableId = injectableIdAttribute.Id;

                if (!injectableIdsToFieldInfos.TryGetValue(injectableId, out var fieldInfosForType))
                {
                    fieldInfosForType = new List<FieldInfo>();
                    injectableIdsToFieldInfos[injectableId] = fieldInfosForType;
                }

                fieldInfosForType.Add(field);

                switch (conditionAttribute.condition)
                {
                    case InjectionCondition.RequireComponentPresent:
                        componentsRequiredPresent.Add(injectableId.componentId);
                        break;
                    case InjectionCondition.RequireComponentWithAuthority:
                        componentsRequiredWithAuthority.Add(injectableId.componentId);
                        break;
                }
            }

            fieldInfoCache[behaviourType] = injectableIdsToFieldInfos.ToDictionary
                (kp => kp.Key, kp => kp.Value.ToArray());

            // Only store stronger requirement
            foreach (var authorityRequirement in componentsRequiredWithAuthority)
            {
                componentsRequiredPresent.Remove(authorityRequirement);
            }

            componentPresentRequirementsForBehaviours[behaviourType] = componentsRequiredPresent.ToList();
            componentAuthRequirementsForBehaviours[behaviourType] = componentsRequiredWithAuthority.ToList();

            var workerTypeAttribute = behaviourType.GetCustomAttribute<WorkerTypeAttribute>();
            workerTypeRequirementsForBehaviours[behaviourType] =
                workerTypeAttribute?.WorkerTypes;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Improbable.Gdk.Subscriptions
{
    internal static class RequiredSubscriptionsDatabase
    {
        private static readonly Dictionary<Type, RequiredSubscriptionsInfo> typeToRequiredSubscriptionsInfo;
        private static readonly Dictionary<Type, WorkerTypeAttribute> typeToWorkerTypeRequirement;

        static RequiredSubscriptionsDatabase()
        {
            typeToRequiredSubscriptionsInfo = new Dictionary<Type, RequiredSubscriptionsInfo>();
            typeToWorkerTypeRequirement = new Dictionary<Type, WorkerTypeAttribute>();
        }

        public static RequiredSubscriptionsInfo GetOrCreateRequiredSubscriptionsInfo(Type type)
        {
            if (!typeToRequiredSubscriptionsInfo.TryGetValue(type, out var info))
            {
                info = new RequiredSubscriptionsInfo(type);
                if (info.RequiredFields.Count == 0)
                {
                    info = null;
                }

                typeToRequiredSubscriptionsInfo.Add(type, info);
            }

            return info;
        }

        public static WorkerTypeAttribute GetOrCreateWorkerTypeRequirement(Type type)
        {
            if (!typeToWorkerTypeRequirement.TryGetValue(type, out var workerTypeRequirement))
            {
                workerTypeRequirement = type.GetCustomAttribute<WorkerTypeAttribute>();
                typeToWorkerTypeRequirement.Add(type, workerTypeRequirement);
            }

            return workerTypeRequirement;
        }

        public static bool HasRequiredSubscriptions(Type type)
        {
            var info = GetOrCreateRequiredSubscriptionsInfo(type);
            return info != null;
        }

        public static bool HasWorkerTypeRequirement(Type type)
        {
            return GetOrCreateWorkerTypeRequirement(type) != null;
        }

        public static bool WorkerTypeMatchesRequirements(string workerType, Type type)
        {
            var requiredTypes = GetOrCreateWorkerTypeRequirement(type);
            if (requiredTypes == null)
            {
                return true;
            }

            return requiredTypes.WorkerTypes.Contains(workerType);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Improbable.Gdk.Subscriptions
{
    internal static class RequiredSubscriptionsDatabase
    {
        private static Dictionary<Type, RequiredSubscriptionsInfo> typeToRequiredSubscriptionsInfo;

        static RequiredSubscriptionsDatabase()
        {
            typeToRequiredSubscriptionsInfo = new Dictionary<Type, RequiredSubscriptionsInfo>();
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

        public static bool HasRequiredSubscriptions(Type type)
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

            return info != null;
        }

        public static bool WorkerTypeMatchesRequirements(string workerType, Type type)
        {
            var requiredTypes = type.GetCustomAttribute<WorkerTypeAttribute>();
            if (requiredTypes == null)
            {
                return true;
            }

            return requiredTypes.WorkerTypes.Contains(workerType);
        }
    }
}

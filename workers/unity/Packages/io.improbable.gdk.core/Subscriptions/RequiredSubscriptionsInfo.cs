using System;
using System.Collections.Generic;
using System.Reflection;

namespace Improbable.Gdk.Subscriptions
{
    internal class RequiredSubscriptionsInfo
    {
        public List<FieldInfo> RequiredFields { get; }
        public Type[] RequiredTypes { get; }

        public RequiredSubscriptionsInfo(Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField |
                BindingFlags.NonPublic);

            RequiredFields = new List<FieldInfo>();

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<RequireAttribute>() != null)
                {
                    RequiredFields.Add(field);
                }
            }

            RequiredTypes = new Type[RequiredFields.Count];
            for (int i = 0; i < RequiredFields.Count; ++i)
            {
                RequiredTypes[i] = RequiredFields[i].FieldType;
            }
        }
    }
}

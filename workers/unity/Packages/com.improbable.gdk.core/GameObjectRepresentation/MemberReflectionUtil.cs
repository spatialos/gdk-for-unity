using System;
using System.Collections.Generic;
using System.Reflection;

namespace Improbable.Gdk.Core
{
    class MemberReflectionUtil
    {
        private readonly Type attributeType;

        private const BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.NonPublic |
                                                BindingFlags.Public | BindingFlags.DeclaredOnly;

        private const BindingFlags PropertyFlags =
            FieldFlags | BindingFlags.SetProperty | BindingFlags.GetProperty;

        internal MemberReflectionUtil(Type attributeType)
        {
            this.attributeType = attributeType;
        }

        /// <returns>
        ///     A MemberAdapter list of properties and fields declared in given type that match at least one of given attributes.
        /// </returns>
        internal List<IMemberAdapter> GetMembersWithMatchingAttributes(Type targetType)
        {
            List<IMemberAdapter> result = new List<IMemberAdapter>();
            ProcessMembers(result, targetType.GetProperties(PropertyFlags), CreatePropertyAdapter);
            ProcessMembers(result, targetType.GetFields(FieldFlags), CreateFieldAdapter);
            return result;
        }

        private void ProcessMembers<T>(List<IMemberAdapter> resultList, IList<T> members, Func<T, IMemberAdapter> adapterFactory) where T : MemberInfo
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (Attribute.IsDefined(members[i], attributeType, false))
                {
                    resultList.Add(adapterFactory(members[i]));
                }
            }
        }

        private static FieldInfoAdapter CreateFieldAdapter(FieldInfo member)
        {
            return new FieldInfoAdapter(member);
        }

        private static PropertyInfoAdapter CreatePropertyAdapter(PropertyInfo member)
        {
            return new PropertyInfoAdapter(member);
        }
    }
}

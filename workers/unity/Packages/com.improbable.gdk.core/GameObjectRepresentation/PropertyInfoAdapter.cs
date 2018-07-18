using System;
using System.Reflection;

namespace Improbable.Gdk.Core
{
    public class PropertyInfoAdapter : IMemberAdapter
    {
        private readonly PropertyInfo member;

        public MemberInfo Member
        {
            get { return member; }
        }

        public PropertyInfoAdapter(PropertyInfo member)
        {
            this.member = member;
        }

        public void SetValue(object obj, object value)
        {
            member.SetValue(obj, value, null);
        }

        public object GetValue(object obj)
        {
            return member.GetValue(obj, null);
        }

        public Type TypeOfMember
        {
            get { return member.PropertyType; }
        }
    }
}


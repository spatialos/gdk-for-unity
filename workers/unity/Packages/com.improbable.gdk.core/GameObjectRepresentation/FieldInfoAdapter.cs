using System;
using System.Reflection;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     A handle on a field of a type obtained through reflection, for getting/setting it on instances conveniently.
    /// </summary>
    internal class FieldInfoAdapter : IMemberAdapter
    {
        private readonly FieldInfo member;

        public MemberInfo Member
        {
            get { return member; }
        }

        public FieldInfoAdapter(FieldInfo member)
        {
            this.member = member;
        }

        public void SetValue(object obj, object value)
        {
            member.SetValue(obj, value);
        }

        public object GetValue(object obj)
        {
            return member.GetValue(obj);
        }

        public Type TypeOfMember
        {
            get { return member.FieldType; }
        }
    }
}


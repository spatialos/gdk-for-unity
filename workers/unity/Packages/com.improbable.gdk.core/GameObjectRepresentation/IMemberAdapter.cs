using System;
using System.Reflection;

namespace Improbable.Gdk.Core
{
    public interface IMemberAdapter
    {
        void SetValue(object obj, object value);
        object GetValue(object obj);
        Type TypeOfMember { get; }
        MemberInfo Member { get; }
    }
}


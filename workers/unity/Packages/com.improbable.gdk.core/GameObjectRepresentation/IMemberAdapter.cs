using System;
using System.Reflection;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     A handle on a field/property of a type obtained through reflection, for getting/setting it on instances conveniently.
    /// </summary>
    public interface IMemberAdapter
    {
        void SetValue(object obj, object value);
        object GetValue(object obj);
        Type TypeOfMember { get; }
        MemberInfo Member { get; }
    }
}


using System;
using UnityEditor;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     For tagging specific IInjectable types and IInjectableCreators with the appropriate InjectableId.
    /// </summary>
    public class InjectableIdAttribute : Attribute
    {
        public readonly InjectableId Id;

        public InjectableIdAttribute(InjectableType type, uint componentId)
        {
            Id = new InjectableId(type, componentId);
        }
    }
}

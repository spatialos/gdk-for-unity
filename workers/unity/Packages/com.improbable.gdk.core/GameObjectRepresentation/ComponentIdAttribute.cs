using System;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Associates a unique component Id with a specific interface or class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class ComponentIdAttribute : Attribute
    {
        /// <summary>The unique identifier.</summary>
        public readonly uint Id;

        /// <summary>Creates an instance of the attribute.</summary>
        public ComponentIdAttribute(uint id)
        {
            this.Id = id;
        }
    }
}

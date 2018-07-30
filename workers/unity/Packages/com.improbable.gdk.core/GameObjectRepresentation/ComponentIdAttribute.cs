using System;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    /// <summary>
    ///     Associates a unique component Id with a specific interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
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

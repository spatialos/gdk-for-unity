using System;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Used to denote when a specific IInjectable type is ready to be created.
    /// </summary>
    internal enum InjectionCondition
    {
        RequireNothing,
        RequireComponentPresent,
        RequireComponentWithAuthority
    }

    /// <summary>
    ///     For tagging specific IInjectable types with the appropriate InjectionCondition.
    /// </summary>
    internal class InjectionConditionAttribute : Attribute
    {
        public readonly InjectionCondition condition;

        public InjectionConditionAttribute(InjectionCondition condition)
        {
            this.condition = condition;
        }
    }
}

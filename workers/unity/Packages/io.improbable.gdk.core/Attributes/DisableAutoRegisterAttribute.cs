using System;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     This attribute denotes that a CodegenAdapter class should not be automatically registered with
    ///     that which consumes them. The intended use for this attribute is testing.
    /// </summary>
    internal class DisableAutoRegisterAttribute : Attribute
    {
    }
}

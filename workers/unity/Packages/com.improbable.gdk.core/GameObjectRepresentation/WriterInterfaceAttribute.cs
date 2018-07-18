using System;

namespace Improbable.Gdk.Core
{
    /// <summary>
    /// Marks an interface as the Writer Interface for use by visualizers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class WriterInterfaceAttribute : Attribute
    {
    }
}

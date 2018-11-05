using System;

namespace Improbable.Gdk.Subscriptions
{
    /// <summary>
    ///     Marks MonoBehaviours which want to be enabled only for particular worker types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class WorkerTypeAttribute : Attribute
    {
        public readonly string[] WorkerTypes;

        public WorkerTypeAttribute(params string[] workerTypes)
        {
            WorkerTypes = workerTypes;
        }
    }
}

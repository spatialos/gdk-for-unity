using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal class EventComparer<T> : IComparer<ComponentEventReceived<T>> where T : IEvent
    {
        public int Compare(ComponentEventReceived<T> x, ComponentEventReceived<T> y)
        {
            var entityIdCompare = x.EntityId.Id.CompareTo(y.EntityId.Id);
            if (entityIdCompare == 0)
            {
                return x.UpdateId.CompareTo(y.UpdateId);
            }

            return entityIdCompare;
        }
    }
}

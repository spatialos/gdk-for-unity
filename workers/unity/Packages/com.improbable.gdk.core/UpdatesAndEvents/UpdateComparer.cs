using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal class UpdateComparer<T> : IComparer<ComponentUpdateReceived<T>> where T : ISpatialComponentUpdate
    {
        public int Compare(ComponentUpdateReceived<T> x, ComponentUpdateReceived<T> y)
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

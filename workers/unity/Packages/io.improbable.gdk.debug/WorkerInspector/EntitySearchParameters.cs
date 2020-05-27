using Improbable.Gdk.Core;

namespace Improbable.Gdk.Debug.WorkerInspector
{
    internal readonly struct EntitySearchParameters
    {
        public readonly EntityId? EntityId;
        public readonly string SearchFragment;

        private EntitySearchParameters(EntityId entityId)
        {
            EntityId = entityId;
            SearchFragment = null;
        }

        private EntitySearchParameters(string stringFragment)
        {
            EntityId = null;
            SearchFragment = stringFragment;
        }

        public static EntitySearchParameters FromSearchString(string searchValue)
        {
            // EntityID values are strictly positive
            if (long.TryParse(searchValue, out var value) && value > 0)
            {
                return new EntitySearchParameters(new EntityId(value));
            }

            return new EntitySearchParameters(searchValue.Trim().ToLower());
        }
    }
}

namespace Improbable.Gdk.Core
{
    public class ComponentSet
    {
        public uint ComponentSetId { get; }
        public uint[] ComponentIds { get; }

        // Hide default constructor, only code gen should be able to make these.
        private ComponentSet()
        {
        }

        internal ComponentSet(uint componentSetId, uint[] componentIds)
        {
            ComponentSetId = componentSetId;
            ComponentIds = componentIds;
        }
    }

    internal interface IComponentSetManager
    {
        bool TryGetComponentSet(uint componentSetId, out ComponentSet componentSet);
    }
}

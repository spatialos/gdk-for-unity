using System.Linq;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class ComponentSetDetails : Details
    {
        public uint ComponentSetId { get; }
        public uint[] ComponentIds { get; }

        public ComponentSetDetails(ComponentSetDefinition componentSetDefinition, DetailsStore detailsStore) : base(componentSetDefinition)
        {
            ComponentSetId = componentSetDefinition.ComponentSetId;
            ComponentIds = componentSetDefinition.ComponentList.Components
                .Select(componentRef => detailsStore.Components[componentRef.Component].ComponentId).ToArray();
        }
    }
}

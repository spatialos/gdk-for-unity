using System.Linq;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class ComponentSetDetails : Details
    {
        public uint ComponentSetId { get; }

        public string[] ComponentIdReferences { get; }

        public ComponentSetDetails(ComponentSetDefinition componentSetDefinition, DetailsStore detailsStore) : base(componentSetDefinition)
        {
            ComponentSetId = componentSetDefinition.ComponentSetId;

            ComponentIdReferences = componentSetDefinition.ComponentList.Components
                .Select(componentRef => $"{detailsStore.Components[componentRef.Component].FullyQualifiedName}.ComponentId")
                .ToArray();
        }
    }
}

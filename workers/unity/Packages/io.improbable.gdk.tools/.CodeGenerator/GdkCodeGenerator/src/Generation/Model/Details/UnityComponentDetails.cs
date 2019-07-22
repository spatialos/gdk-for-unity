using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityComponentDetails
    {
        public string Package { get; }
        public string ComponentName { get; }
        public uint ComponentId { get; }

        public bool IsBlittable { get; }

        public IReadOnlyList<UnityFieldDetails> FieldDetails { get; private set; }
        public IReadOnlyList<UnityCommandDetails> CommandDetails { get; }
        public IReadOnlyList<UnityEventDetails> EventDetails { get; }

        private ComponentDefinition raw;

        public UnityComponentDetails(string package, ComponentDefinition componentDefinitionRaw, DetailsStore store)
        {
            Package = package;
            ComponentName = componentDefinitionRaw.Name;
            ComponentId = componentDefinitionRaw.ComponentId;
            IsBlittable = store.BlittableSet.Contains(componentDefinitionRaw.QualifiedName);

            CommandDetails = componentDefinitionRaw.Commands
                .Select(command => new UnityCommandDetails(command))
                .ToList()
                .AsReadOnly();

            EventDetails = componentDefinitionRaw.Events
                .Select(ev => new UnityEventDetails(ev))
                .ToList()
                .AsReadOnly();

            raw = componentDefinitionRaw;
        }

        public void PopulateFields(DetailsStore store)
        {
            if (!string.IsNullOrEmpty(raw.DataDefinition))
            {
                FieldDetails = store.Types[raw.DataDefinition].FieldDetails;
            }
            else
            {
                FieldDetails = raw.Fields
                    .Where(field => !UnityTypeMappings.IsEntity(field))
                    .Select(field => new UnityFieldDetails(field, store))
                    .ToList()
                    .AsReadOnly();
            }
        }
    }
}

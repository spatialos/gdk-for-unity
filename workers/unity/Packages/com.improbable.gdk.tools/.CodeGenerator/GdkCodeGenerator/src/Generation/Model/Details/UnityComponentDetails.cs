using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityComponentDetails
    {
        public string ComponentName { get; }
        public uint ComponentId { get; }

        public bool IsBlittable { get; }

        public IReadOnlyList<UnityFieldDetails> FieldDetails { get; private set; }
        public IReadOnlyList<UnityCommandDetails> CommandDetails { get; }
        public IReadOnlyList<UnityEventDetails> EventDetails { get; }

        public Identifier Identifier { get; }

        private ComponentDefinitionRaw raw;

        public UnityComponentDetails(ComponentDefinitionRaw componentDefinitionRaw, DetailsStore store)
        {
            ComponentName = componentDefinitionRaw.Identifier.Name;
            ComponentId = componentDefinitionRaw.ComponentId;
            IsBlittable = store.BlittableMap.Contains(componentDefinitionRaw.Identifier);

            CommandDetails = componentDefinitionRaw.Commands
                .Select(command => new UnityCommandDetails(command))
                .ToList()
                .AsReadOnly();

            EventDetails = componentDefinitionRaw.Events
                .Select(ev => new UnityEventDetails(ev))
                .ToList()
                .AsReadOnly();

            Identifier = componentDefinitionRaw.Identifier;
            raw = componentDefinitionRaw;
        }

        public void PopulateFields(DetailsStore store)
        {
            if (raw.Data != null)
            {
                FieldDetails = store.Types[CommonDetailsUtils.CreateIdentifier(raw.Data.QualifiedName)].FieldDetails;
            }
            else
            {
                FieldDetails = raw.Fields
                    .Select(field => new UnityFieldDetails(field, store))
                    .ToList()
                    .AsReadOnly();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityComponentDetails
    {
        public string Package { get; }
        public string ComponentName { get; }
        public uint ComponentId { get; }

        public bool IsBlittable { get; }

        public IReadOnlyList<UnityFieldDetails> FieldDetails { get; private set; }
        public IReadOnlyList<UnityCommandDetails> CommandDetails { get; private set; }
        public IReadOnlyList<UnityEventDetails> EventDetails { get; private set; }

        private ComponentDefinition raw;

        public UnityComponentDetails(string package, ComponentDefinition componentDefinitionRaw, DetailsStore store)
        {
            Package = package;
            ComponentName = componentDefinitionRaw.Name;
            ComponentId = componentDefinitionRaw.ComponentId;
            IsBlittable = store.BlittableSet.Contains(componentDefinitionRaw.QualifiedName);

            CommandDetails = componentDefinitionRaw.Commands
                .Select(command => new UnityCommandDetails(command))
                .Where(commandDetail =>
                {
                    // Return true to keep commands that do not have a name clash with the component
                    if (!commandDetail.CommandName.Equals(ComponentName))
                    {
                        return true;
                    }

                    Console.Error.WriteLine($"Error in component \"{ComponentName}\". Command \"{commandDetail.RawCommandName}\" clashes with component name.");
                    return false;
                })
                .ToList()
                .AsReadOnly();

            EventDetails = componentDefinitionRaw.Events
                .Select(ev => new UnityEventDetails(ev))
                .Where(eventDetail =>
                {
                    // Return true to keep events that do not have a name clash with the component
                    if (!eventDetail.EventName.Equals(ComponentName))
                    {
                        return true;
                    }

                    Console.Error.WriteLine($"Error in component \"{ComponentName}\". Event \"{eventDetail.RawEventName}\" clashes with component name.");
                    return false;
                })
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
                    .Select(field => new UnityFieldDetails(field, store))
                    .ToList()
                    .AsReadOnly();
            }
        }
    }
}

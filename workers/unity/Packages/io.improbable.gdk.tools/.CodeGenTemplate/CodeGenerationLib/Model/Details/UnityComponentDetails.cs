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
                    .Select(field => new UnityFieldDetails(field, store))
                    .ToList()
                    .AsReadOnly();
            }
        }

        public bool IsValid()
        {
            var isValid = true;
            var componentName = ComponentName;

            var clashingCommands = CommandDetails
                .Where(commandDetail => commandDetail.CommandName.Equals(componentName));
            foreach (var clashingCommand in clashingCommands)
            {
                isValid = false;
                Console.Error.WriteLine(
                    $"Error in component \"{componentName}\". " +
                    $"Command \"{clashingCommand.RawCommandName}\" clashes with component name.");
            }

            var clashingEvents = EventDetails
                .Where(eventDetail => eventDetail.EventName.Equals(componentName));
            foreach (var clashingEvent in clashingEvents)
            {
                isValid = false;
                Console.Error.WriteLine(
                    $"Error in component \"{componentName}\". " +
                    $"Event \"{clashingEvent.RawEventName}\" clashes with component name.");
            }

            return isValid;
        }
    }
}

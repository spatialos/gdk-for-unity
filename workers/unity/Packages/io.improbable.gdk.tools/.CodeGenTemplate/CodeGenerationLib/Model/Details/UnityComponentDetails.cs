using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Utils;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityComponentDetails
    {
        public readonly string Name;
        public readonly string Namespace;

        public readonly uint ComponentId;
        public readonly bool IsBlittable;

        public IReadOnlyList<UnityFieldDetails> FieldDetails { get; private set; }
        public readonly IReadOnlyList<UnityCommandDetails> CommandDetails;
        public readonly IReadOnlyList<UnityEventDetails> EventDetails;

        private readonly ComponentDefinition rawComponentDefinition;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UnityComponentDetails(string package, ComponentDefinition rawComponentDefinition, DetailsStore store)
        {
            Name = rawComponentDefinition.Name;
            Namespace = Formatting.CapitaliseQualifiedNameParts(package);

            ComponentId = rawComponentDefinition.ComponentId;

            IsBlittable = store.BlittableSet.Contains(rawComponentDefinition.QualifiedName);

            Logger.Trace($"Populating command details for component {rawComponentDefinition.QualifiedName}.");
            CommandDetails = rawComponentDefinition.Commands
                .Select(command => new UnityCommandDetails(command))
                .Where(commandDetail =>
                {
                    // Return true to keep commands that do not have a name clash with the component
                    if (!commandDetail.CommandName.Equals(Name))
                    {
                        return true;
                    }

                    Logger.Error($"Error in component \"{Name}\". Command \"{commandDetail.RawCommandName}\" clashes with component name.");
                    return false;
                })
                .ToList()
                .AsReadOnly();

            Logger.Trace($"Populating event details for component {rawComponentDefinition.QualifiedName}.");
            EventDetails = rawComponentDefinition.Events
                .Select(ev => new UnityEventDetails(ev))
                .Where(eventDetail =>
                {
                    // Return true to keep events that do not have a name clash with the component
                    if (!eventDetail.EventName.Equals(Name))
                    {
                        return true;
                    }

                    Logger.Error($"Error in component \"{Name}\". Event \"{eventDetail.RawEventName}\" clashes with component name.");
                    return false;
                })
                .ToList()
                .AsReadOnly();

            this.rawComponentDefinition = rawComponentDefinition;
        }

        public void PopulateFields(DetailsStore store)
        {
            Logger.Trace($"Populating field details for component {rawComponentDefinition.QualifiedName}.");
            if (!string.IsNullOrEmpty(rawComponentDefinition.DataDefinition))
            {
                FieldDetails = store.Types[rawComponentDefinition.DataDefinition].FieldDetails;
            }
            else
            {
                FieldDetails = rawComponentDefinition.Fields
                    .Select(field => new UnityFieldDetails(field, store))
                    .ToList()
                    .AsReadOnly();
            }
        }
    }
}

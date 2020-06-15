using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityComponentDetails : GeneratorInputDetails
    {
        public readonly uint ComponentId;
        public readonly bool IsBlittable;

        public IReadOnlyList<UnityFieldDetails> FieldDetails { get; private set; }
        public readonly IReadOnlyList<UnityCommandDetails> CommandDetails;
        public readonly IReadOnlyList<UnityEventDetails> EventDetails;
        public readonly IReadOnlyDictionary<string, List<Annotation>> Annotations;

        private readonly ComponentDefinition rawComponentDefinition;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UnityComponentDetails(string package, ComponentDefinition rawComponentDefinition, DetailsStore store)
            : base(package, rawComponentDefinition)
        {
            ComponentId = rawComponentDefinition.ComponentId;

            IsBlittable = store.BlittableSet.Contains(rawComponentDefinition.QualifiedName);

            Logger.Trace($"Populating command details for component {rawComponentDefinition.QualifiedName}.");
            CommandDetails = rawComponentDefinition.Commands
                .Select(command => new UnityCommandDetails(command))
                .Where(commandDetail =>
                {
                    // Return true to keep commands that do not have a name clash with the component
                    if (!commandDetail.PascalCaseName.Equals(Name))
                    {
                        return true;
                    }

                    Logger.Error($"Error in component \"{Name}\". Command \"{commandDetail.PascalCaseName}\" clashes with component name.");
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
                    if (!eventDetail.PascalCaseName.Equals(Name))
                    {
                        return true;
                    }

                    Logger.Error($"Error in component \"{Name}\". Event \"{eventDetail.PascalCaseName}\" clashes with component name.");
                    return false;
                })
                .ToList()
                .AsReadOnly();

            var annotations = new Dictionary<string, List<Annotation>>();

            foreach (var annotation in rawComponentDefinition.Annotations)
            {
                if (!annotations.TryGetValue(annotation.TypeValue.Type, out var list))
                {
                    list = new List<Annotation>();
                    annotations[annotation.TypeValue.Type] = list;
                }

                list.Add(annotation);
            }

            Annotations = annotations;

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

using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     This class contains the data required to fill out the "UnityCommandSenderReceiverGenerator.tt" templates.
    ///     This generates the command senders and receivers as well as their subscriptions and callback managers.
    /// </summary>
    public partial class UnityCommandSenderReceiverGenerator
    {
        private string qualifiedNamespace;
        private string spatialNamespace;
        private UnityComponentDefinition unityComponentDefinition;
        private HashSet<string> enumSet;

        public string Generate(UnityComponentDefinition unityComponentDefinition, string package,
            HashSet<string> enumSet)
        {
            qualifiedNamespace = package;
            spatialNamespace = package;
            this.unityComponentDefinition = unityComponentDefinition;
            this.enumSet = enumSet;

            return TransformText();
        }

        private UnityComponentDetails GetComponentDetails()
        {
            return new UnityComponentDetails(unityComponentDefinition);
        }

        private List<UnityFieldDetails> GetFieldDetailsList()
        {
            return unityComponentDefinition.DataDefinition.typeDefinition.FieldDefinitions
                .Select(fieldDefinition =>
                    new UnityFieldDetails(fieldDefinition.RawFieldDefinition, fieldDefinition.IsBlittable, enumSet))
                .ToList();
        }

        private List<UnityCommandDetails> GetCommandDetailsList()
        {
            return unityComponentDefinition.CommandDefinitions
                .Select(commandDefinition => new UnityCommandDetails(commandDefinition)).ToList();
        }

        private List<UnityEventDetails> GetEventDetailsList()
        {
            return unityComponentDefinition.EventDefinitions
                .Select(eventDefinition => new UnityEventDetails(eventDefinition)).ToList();
        }
    }
}


using System.Collections.Generic;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     This static class adds markers to user defined types by traversing the corpus of processed schema files.
    ///     These markers indicate whether a type is an Event payload, Command request, or Command response.
    /// </summary>
    public static class PayloadMarker
    {
        public static void MarkPayloadTypes(IEnumerable<UnitySchemaFile> processedSchemaFiles)
        {
            var commandRequestTypes = new HashSet<string>();
            var commandResponseTypes = new HashSet<string>();
            var eventPayloadTypes = new HashSet<string>();
            PopulatePayloadTypes(commandRequestTypes, commandResponseTypes, eventPayloadTypes, processedSchemaFiles);
            ProcessPayloadTypes(commandResponseTypes, commandResponseTypes, eventPayloadTypes, processedSchemaFiles);
        }

        private static void PopulatePayloadTypes(HashSet<string> commandRequestTypes,
            HashSet<string> commandResponseTypes, HashSet<string> eventPayloadTypes,
            IEnumerable<UnitySchemaFile> schemaFiles)
        {
            foreach (var schemaFile in schemaFiles)
            {
                foreach (var componentDefinition in schemaFile.ComponentDefinitions)
                {
                    foreach (var commandDefinition in componentDefinition.CommandDefinitions)
                    {
                        if (!commandDefinition.RawRequestType.IsBuiltInType)
                        {
                            commandRequestTypes.Add(commandDefinition.RequestType.typeDefinition.QualifiedName);
                        }

                        if (!commandDefinition.RawResponseType.IsBuiltInType)
                        {
                            commandResponseTypes.Add(commandDefinition.ResponseType.typeDefinition.QualifiedName);
                        }
                    }

                    foreach (var eventDefinition in componentDefinition.EventDefinitions)
                    {
                        if (!eventDefinition.RawType.IsBuiltInType)
                        {
                            eventPayloadTypes.Add(eventDefinition.Type.typeDefinition.QualifiedName);
                        }
                    }
                }
            }
        }

        private static void ProcessPayloadTypes(HashSet<string> commandRequestTypes,
            HashSet<string> commandResponseTypes, HashSet<string> eventPayloadTypes,
            IEnumerable<UnitySchemaFile> schemaFiles)
        {
            foreach (var schemaFile in schemaFiles)
            {
                foreach (var typeDefinition in schemaFile.TypeDefinitions)
                {
                    var qualifedName = typeDefinition.QualifiedName;

                    if (commandRequestTypes.Contains(qualifedName))
                    {
                        typeDefinition.IsCommandRequestPayload = true;
                    }

                    if (commandResponseTypes.Contains(qualifedName))
                    {
                        typeDefinition.IsCommandResponsePayload = true;
                    }

                    if (eventPayloadTypes.Contains(qualifedName))
                    {
                        typeDefinition.IsEventPayload = true;
                    }
                }
            }
        }
    }
}

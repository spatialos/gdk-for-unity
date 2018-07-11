using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.CodeGeneration.FileHandling;
using Improbable.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnitySchemaProcessor
    {
        public ICollection<UnitySchemaFile> ProcessedSchemaFiles { get; }
        private readonly IFileSystem fileSystem = new FileSystem();

        public UnitySchemaProcessor(IEnumerable<SchemaFileRaw> schemaFiles)
        {
            ProcessedSchemaFiles = schemaFiles.Select(rawSchemaFile =>
            {
                var fileInfo = fileSystem.GetFileInfo(rawSchemaFile.completePath);
                return new UnitySchemaFile(rawSchemaFile, fileInfo.LastWriteTime);
            }).ToList();

            var typeReferences = new Dictionary<string, UnityTypeReference>();
            FindTypeReferences(typeReferences, ProcessedSchemaFiles);
            PopulateTypeReferences(typeReferences, ProcessedSchemaFiles);

            BlittableFlagger.SetBlittableFlags(ProcessedSchemaFiles);
            PayloadMarker.MarkPayloadTypes(ProcessedSchemaFiles);
        }

        private void FindTypeReferences(Dictionary<string, UnityTypeReference> typeReferences,
            IEnumerable<UnitySchemaFile> schemaFiles)
        {
            foreach (var schemaFile in schemaFiles)
            {
                FindTypeReferences(typeReferences, schemaFile.EnumDefinitions);
                FindTypeReferences(typeReferences, schemaFile.TypeDefinitions);
            }
        }

        private void FindTypeReferences(Dictionary<string, UnityTypeReference> typeReferences,
            IEnumerable<EnumDefinitionRaw> enumDefinitions)
        {
            foreach (var enumDefinition in enumDefinitions)
            {
                typeReferences.Add(enumDefinition.qualifiedName, new UnityTypeReference(null, enumDefinition, null));
            }
        }

        private void FindTypeReferences(Dictionary<string, UnityTypeReference> typeReferences,
            IEnumerable<UnityTypeDefinition> typeDefinitions)
        {
            foreach (var typeDefinition in typeDefinitions)
            {
                typeReferences.Add(typeDefinition.QualifiedName, new UnityTypeReference(null, null, typeDefinition));
                FindTypeReferences(typeReferences, typeDefinition.EnumDefinitions);
                FindTypeReferences(typeReferences, typeDefinition.TypeDefinitions);
            }
        }

        private void PopulateTypeReferences(Dictionary<string, UnityTypeReference> typeReferences,
            IEnumerable<UnitySchemaFile> schemaFiles)
        {
            foreach (var schemaFile in schemaFiles)
            {
                PopulateTypeReferences(typeReferences, schemaFile.TypeDefinitions);
                PopulateTypeReferences(typeReferences, schemaFile.ComponentDefinitions);
            }
        }

        private void PopulateTypeReferences(Dictionary<string, UnityTypeReference> typeReferences,
            IEnumerable<UnityTypeDefinition> typeDefinitions)
        {
            foreach (var typeDefinition in typeDefinitions)
            {
                PopulateTypeReferences(typeReferences, typeDefinition.TypeDefinitions);
                foreach (var fieldDefinition in typeDefinition.FieldDefinitions)
                {
                    if (fieldDefinition.RawKeyType != null &&
                        !string.IsNullOrEmpty(fieldDefinition.RawKeyType.userType) && fieldDefinition.KeyType == null)
                    {
                        if (!typeReferences.ContainsKey(fieldDefinition.RawKeyType.userType))
                        {
                            throw new Exception("No type reference found for " + fieldDefinition.RawKeyType.userType);
                        }

                        fieldDefinition.KeyType = typeReferences[fieldDefinition.RawKeyType.userType];
                    }

                    if (fieldDefinition.RawValueType != null &&
                        !string.IsNullOrEmpty(fieldDefinition.RawValueType.userType) &&
                        fieldDefinition.ValueType == null)
                    {
                        if (!typeReferences.ContainsKey(fieldDefinition.RawValueType.userType))
                        {
                            throw new Exception("No type reference found for " + fieldDefinition.RawValueType.userType);
                        }

                        fieldDefinition.ValueType = typeReferences[fieldDefinition.RawValueType.userType];
                    }
                }
            }
        }

        private void PopulateTypeReferences(Dictionary<string, UnityTypeReference> typeReferences,
            IEnumerable<UnityComponentDefinition> componentDefinitions)
        {
            foreach (var componentDefinition in componentDefinitions)
            {
                if (componentDefinition.RawDataDefinition != null &&
                    !string.IsNullOrEmpty(componentDefinition.RawDataDefinition.userType) &&
                    componentDefinition.DataDefinition == null)
                {
                    if (!typeReferences.ContainsKey(componentDefinition.RawDataDefinition.userType))
                    {
                        throw new Exception("No type reference found for " +
                            componentDefinition.RawDataDefinition.userType);
                    }

                    componentDefinition.DataDefinition = typeReferences[componentDefinition.RawDataDefinition.userType];
                }

                foreach (var eventDefinition in componentDefinition.EventDefinitions)
                {
                    if (eventDefinition.RawType != null && !string.IsNullOrEmpty(eventDefinition.RawType.userType) &&
                        eventDefinition.Type == null)
                    {
                        if (!typeReferences.ContainsKey(eventDefinition.RawType.userType))
                        {
                            throw new Exception("No type reference found for " + eventDefinition.RawType.userType);
                        }

                        eventDefinition.Type = typeReferences[eventDefinition.RawType.userType];
                    }
                }

                foreach (var commandDefinition in componentDefinition.CommandDefinitions)
                {
                    if (commandDefinition.RawRequestType != null &&
                        !string.IsNullOrEmpty(commandDefinition.RawRequestType.userType) &&
                        commandDefinition.RequestType == null)
                    {
                        if (!typeReferences.ContainsKey(commandDefinition.RawRequestType.userType))
                        {
                            throw new Exception("No type reference found for " +
                                commandDefinition.RawRequestType.userType);
                        }

                        commandDefinition.RequestType = typeReferences[commandDefinition.RawRequestType.userType];
                    }

                    if (commandDefinition.RawResponseType != null &&
                        !string.IsNullOrEmpty(commandDefinition.RawResponseType.userType) &&
                        commandDefinition.ResponseType == null)
                    {
                        if (!typeReferences.ContainsKey(commandDefinition.RawResponseType.userType))
                        {
                            throw new Exception("No type reference found for " +
                                commandDefinition.RawResponseType.userType);
                        }

                        commandDefinition.ResponseType = typeReferences[commandDefinition.RawResponseType.userType];
                    }
                }
            }
        }
    }
}

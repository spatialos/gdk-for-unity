using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Improbable.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     Represents a schema file.
    /// </summary>
    [DebuggerDisplay("{CanonicalName}")]
    public class UnitySchemaFile
    {
        public readonly string CanonicalName;
        public readonly string Package;
        public readonly EnumDefinitionRaw[] EnumDefinitions;
        public readonly List<UnityTypeDefinition> TypeDefinitions;
        public readonly List<UnityComponentDefinition> ComponentDefinitions;

        public DateTime LastWriteTime { get; private set; }

        public string CompletePath { get; private set; }

        internal UnitySchemaFile(SchemaFileRaw rawSchemaFile, DateTime lastWriteTime)
        {
            CompletePath = rawSchemaFile.completePath;
            CanonicalName = rawSchemaFile.canonicalName;
            Package = rawSchemaFile.package;
            EnumDefinitions = rawSchemaFile.enumDefinitions;
            TypeDefinitions = rawSchemaFile.typeDefinitions != null
                ? rawSchemaFile.typeDefinitions.Select(rawTypeDefinition => new UnityTypeDefinition(rawTypeDefinition))
                    .ToList()
                : new List<UnityTypeDefinition>();
            ComponentDefinitions = rawSchemaFile.componentDefinitions != null
                ? rawSchemaFile.componentDefinitions
                    .Select(rawComponentDefinition => new UnityComponentDefinition(rawComponentDefinition)).ToList()
                : new List<UnityComponentDefinition>();
            LastWriteTime = lastWriteTime;
        }
    }

    /// <summary>
    ///     Represents a type definition in the schema.
    /// </summary>
    public class UnityTypeDefinition
    {
        public readonly string Name;
        public readonly string QualifiedName;
        public readonly EnumDefinitionRaw[] EnumDefinitions;
        public readonly List<UnityTypeDefinition> TypeDefinitions;
        public readonly List<UnityFieldDefinition> FieldDefinitions;
        public readonly SourceReferenceRaw SourceReference;
        public bool IsBlittable;
        public bool IsEventPayload;
        public bool IsCommandRequestPayload;
        public bool IsCommandResponsePayload;

        internal UnityTypeDefinition(TypeDefinitionRaw rawTypeDefinition)
        {
            Name = rawTypeDefinition.name;
            QualifiedName = rawTypeDefinition.qualifiedName;
            EnumDefinitions = rawTypeDefinition.enumDefinitions;
            TypeDefinitions = rawTypeDefinition.typeDefinitions != null
                ? rawTypeDefinition.typeDefinitions.Select(raw => new UnityTypeDefinition(raw)).ToList()
                : new List<UnityTypeDefinition>();
            FieldDefinitions = rawTypeDefinition.fieldDefinitions != null
                ? rawTypeDefinition.fieldDefinitions
                    .Select(rawFieldDefinition => new UnityFieldDefinition(rawFieldDefinition)).ToList()
                : new List<UnityFieldDefinition>();
            SourceReference = rawTypeDefinition.sourceReference;
            IsBlittable = false;
            IsEventPayload = false;
            IsCommandRequestPayload = false;
            IsCommandResponsePayload = false;
        }
    }

    /// <summary>
    ///     Represents a field definition in the schema.
    /// </summary>
    public class UnityFieldDefinition
    {
        public readonly string Name;
        public readonly int Number;
        public readonly bool IsOption;
        public readonly bool IsList;
        public readonly bool IsMap;
        internal readonly TypeReferenceRaw RawKeyType;
        internal readonly TypeReferenceRaw RawValueType;
        public UnityTypeReference KeyType;
        public UnityTypeReference ValueType;

        public bool IsBlittable;

        public readonly FieldDefinitionRaw RawFieldDefinition;

        internal UnityFieldDefinition(FieldDefinitionRaw rawFieldDefinition)
        {
            this.RawFieldDefinition = rawFieldDefinition;
            Name = rawFieldDefinition.name;
            Number = rawFieldDefinition.Number;
            if (rawFieldDefinition.IsOption())
            {
                RawValueType = rawFieldDefinition.optionType.valueType;
                IsOption = true;
            }
            else if (rawFieldDefinition.IsList())
            {
                RawValueType = rawFieldDefinition.listType.valueType;
                IsList = true;
            }
            else if (rawFieldDefinition.IsMap())
            {
                RawKeyType = rawFieldDefinition.mapType.keyType;
                RawValueType = rawFieldDefinition.mapType.valueType;
                IsMap = true;
            }
            else
            {
                RawValueType = rawFieldDefinition.singularType;
            }

            if (RawKeyType != null && RawKeyType.IsBuiltInType)
            {
                KeyType = new UnityTypeReference(RawKeyType.TypeName, null, null);
            }

            if (RawValueType != null && RawValueType.IsBuiltInType)
            {
                ValueType = new UnityTypeReference(RawValueType.TypeName, null, null);
            }
        }
    }

    /// <summary>
    ///     Represents a component definition in the schema.
    /// </summary>
    public class UnityComponentDefinition
    {
        /// <summary>
        ///     Represents an event definition in the schema.
        /// </summary>
        public class UnityEventDefinition
        {
            public readonly string Name;
            internal readonly TypeReferenceRaw RawType;
            public UnityTypeReference Type;
            public uint EventIndex;

            internal UnityEventDefinition(ComponentDefinitionRaw.EventDefinitionRaw rawEventDefinition)
            {
                Name = rawEventDefinition.name;
                RawType = rawEventDefinition.type;

                if (RawType.IsBuiltInType)
                {
                    Type = new UnityTypeReference(RawType.TypeName, null, null);
                }

                EventIndex = rawEventDefinition.eventIndex;
            }
        }

        /// <summary>
        ///     Represents a command definition in the schema.
        /// </summary>
        public class UnityCommandDefinition
        {
            public readonly string Name;
            internal readonly TypeReferenceRaw RawRequestType;
            internal readonly TypeReferenceRaw RawResponseType;
            public UnityTypeReference RequestType;
            public UnityTypeReference ResponseType;
            public uint CommandIndex;

            internal UnityCommandDefinition(ComponentDefinitionRaw.CommandDefinitionRaw rawCommandDefinition)
            {
                Name = rawCommandDefinition.name;
                RawRequestType = rawCommandDefinition.requestType;
                RawResponseType = rawCommandDefinition.responseType;

                if (RawRequestType != null && RawRequestType.IsBuiltInType)
                {
                    RequestType = new UnityTypeReference(RawRequestType.TypeName, null, null);
                }

                if (RawResponseType != null && RawResponseType.IsBuiltInType)
                {
                    ResponseType = new UnityTypeReference(RawResponseType.TypeName, null, null);
                }

                CommandIndex = rawCommandDefinition.commandIndex;
            }
        }

        public readonly string Name;
        public readonly string QualifiedName;
        public readonly int Id;
        internal readonly TypeReferenceRaw RawDataDefinition;
        public UnityTypeReference DataDefinition;
        public readonly List<UnityEventDefinition> EventDefinitions;
        public readonly List<UnityCommandDefinition> CommandDefinitions;
        public bool IsBlittable;

        internal UnityComponentDefinition(ComponentDefinitionRaw rawComponentDefinition)
        {
            Name = rawComponentDefinition.name;
            QualifiedName = rawComponentDefinition.qualifiedName;
            Id = rawComponentDefinition.Id;
            RawDataDefinition = rawComponentDefinition.dataDefinition;
            if (RawDataDefinition != null && RawDataDefinition.IsBuiltInType)
            {
                DataDefinition = new UnityTypeReference(RawDataDefinition.TypeName, null, null);
            }

            EventDefinitions = rawComponentDefinition.eventDefinitions != null
                ? rawComponentDefinition.eventDefinitions
                    .Select(rawEventDefinition => new UnityEventDefinition(rawEventDefinition)).ToList()
                : new List<UnityEventDefinition>();
            CommandDefinitions = rawComponentDefinition.commandDefinitions != null
                ? rawComponentDefinition.commandDefinitions
                    .Select(rawCommandDefinition => new UnityCommandDefinition(rawCommandDefinition)).ToList()
                : new List<UnityCommandDefinition>();
        }
    }
}

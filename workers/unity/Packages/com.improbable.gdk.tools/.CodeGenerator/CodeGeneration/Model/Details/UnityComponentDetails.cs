using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityComponentDetails
    {
        public readonly string PascalCaseName;
        public readonly string CamelCaseName;
        public readonly string FullyQualifiedName;

        public readonly uint ComponentId;

        public readonly List<UnityFieldDetails> Fields;
        public readonly List<UnityEventDetails> Events;
        public readonly List<UnityCommandDetails> Commands;

        public UnityComponentDetails(ComponentDefinitionRaw componentDefinitionRaw, SchemaBundle.Bundle bundle)
        {
            (PascalCaseName, CamelCaseName, FullyQualifiedName) = componentDefinitionRaw.Identifier.GetNameSet();
            ComponentId = componentDefinitionRaw.ComponentId;

            var underlyingType = bundle.GetSchemaType(componentDefinitionRaw.Data.Type.QualifiedName);
            if (underlyingType == null)
            {
                throw new ArgumentException(
                    $"Cannot find type definition '{componentDefinitionRaw.Data.Type.QualifiedName}' in the bundle.");
            }

            Fields = underlyingType.Fields.Select(field => new UnityFieldDetails(field, bundle)).ToList();
            Events = componentDefinitionRaw.Events.Select(ev => new UnityEventDetails(ev, bundle)).ToList();
            Commands = componentDefinitionRaw.Commands.Select(command => new UnityCommandDetails(command, bundle)).ToList();
        }
    }
}

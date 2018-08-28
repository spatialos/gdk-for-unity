using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityTypeContent
    {
        private UnityTypeDefinition typeDefinition;
        private HashSet<string> enumSet;
        private List<UnityTypeDefinition> nestedTypes;
        private List<EnumDefinitionRaw> nestedEnums;

        public string Generate(UnityTypeDefinition typeDefinition, HashSet<string> enumSet)
        {
            this.typeDefinition = typeDefinition;
            this.enumSet = enumSet;
            this.nestedTypes = typeDefinition.TypeDefinitions;
            this.nestedEnums = typeDefinition.EnumDefinitions.ToList();
            return TransformText();
        }

        private UnityTypeDetails GetTypeDetails()
        {
            return new UnityTypeDetails(typeDefinition);
        }

        private List<UnityFieldDetails> GetFieldDetailsList()
        {
            return typeDefinition.FieldDefinitions
                .Select((fieldDefinition) =>
                    new UnityFieldDetails(fieldDefinition.RawFieldDefinition, fieldDefinition.IsBlittable, enumSet))
                .ToList();
        }

        private string GetConstructorArgs()
        {
            var constructorArgsList = GetFieldDetailsList().Select(fieldDetails => $"{fieldDetails.Type} {fieldDetails.CamelCaseName}");
            return string.Join(", ", constructorArgsList);
        }
    }
}

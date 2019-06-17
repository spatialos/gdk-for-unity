using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityEnumDetails
    {
        public string TypeName;
        public string FqnTypeName;

        public List<(uint, string)> Values;

        public Identifier Identifier;

        public UnityEnumDetails(EnumDefinitionRaw enumDefinitionRaw)
        {
            TypeName = enumDefinitionRaw.EnumIdentifier.Name;
            FqnTypeName = CommonDetailsUtils.GetCapitalisedFqnTypename(enumDefinitionRaw.EnumIdentifier.QualifiedName);
            Identifier = enumDefinitionRaw.EnumIdentifier;

            Values = enumDefinitionRaw.Values.Select(value => (value.EnumValue, value.Identifier.Name)).ToList();
        }
    }
}

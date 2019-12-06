using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEnumDetails
    {
        public string Package;
        public string TypeName;
        public string QualifiedName;
        public string FqnTypeName;

        public List<(uint, string)> Values;

        public UnityEnumDetails(string package, EnumDefinition enumDefinitionRaw)
        {
            Package = package;
            TypeName = enumDefinitionRaw.Name;
            QualifiedName = enumDefinitionRaw.QualifiedName;
            FqnTypeName = CommonDetailsUtils.GetCapitalisedFqnTypename(enumDefinitionRaw.QualifiedName);
            Values = enumDefinitionRaw.Values.Select(value => (value.Value, value.Name)).ToList();
        }
    }
}

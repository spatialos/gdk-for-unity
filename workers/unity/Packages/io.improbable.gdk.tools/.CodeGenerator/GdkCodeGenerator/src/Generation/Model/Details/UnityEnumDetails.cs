using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityEnumDetails
    {
        public string Package;
        public string TypeName;
        public string FqnTypeName;

        public List<(uint, string)> Values;

        public UnityEnumDetails(string package, EnumDefinition enumDefinitionRaw)
        {
            Package = package;
            TypeName = enumDefinitionRaw.Name;
            FqnTypeName = CommonDetailsUtils.GetCapitalisedFqnTypename(enumDefinitionRaw.QualifiedName);
            Values = enumDefinitionRaw.Values.Select(value => (value.Value, value.Name)).ToList();
        }
    }
}

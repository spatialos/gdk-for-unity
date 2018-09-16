using System.Collections.Generic;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityTypeGenerator
    {
        private string qualifiedNamespace;
        private UnityTypeDefinition typeDefinition;
        private HashSet<string> enumSet;

        public string Generate(UnityTypeDefinition typeDefinition, string package, HashSet<string> enumSet)
        {
            qualifiedNamespace = package;
            this.typeDefinition = typeDefinition;
            this.enumSet = enumSet;
            return TransformText();
        }
    }
}

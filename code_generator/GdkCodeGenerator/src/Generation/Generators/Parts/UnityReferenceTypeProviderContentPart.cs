using System;
using System.Collections.Generic;
using System.Text;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityReferenceTypeProviderContent
    {
        private String Name;
        private String TypeName;

        public string Generate(string name, string typeName)
        {
            Name = name;
            TypeName = typeName;

            return TransformText();
        }
    }
}

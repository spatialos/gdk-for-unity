using System;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityReferenceTypeProviderContent
    {
        private String Name;
        private String TypeName;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public string Generate(string name, string typeName)
        {
            Name = name;
            TypeName = typeName;

            return TransformText();
        }
    }
}

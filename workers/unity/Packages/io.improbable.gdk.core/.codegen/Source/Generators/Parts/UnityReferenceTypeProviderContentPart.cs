using System;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityReferenceTypeProviderContent
    {
        private String Name;
        private String TypeName;
        private String QualifiedNamespace;
        private String ComponentName;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public UnityReferenceTypeProviderContent()
        {
            logger.Trace($"Constructing {GetType()}");
        }

        public string Generate(string name, string typeName, string qualifiedNamespace, string componentName)
        {
            Name = name;
            TypeName = typeName;
            QualifiedNamespace = qualifiedNamespace;
            ComponentName = componentName;

            return TransformText();
        }
    }
}

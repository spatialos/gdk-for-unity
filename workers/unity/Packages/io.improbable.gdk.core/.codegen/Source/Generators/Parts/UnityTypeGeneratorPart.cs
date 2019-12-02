using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityTypeGenerator
    {
        private string qualifiedNamespace;
        private UnityTypeDetails details;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public string Generate(UnityTypeDetails details, string package)
        {
            qualifiedNamespace = package;
            this.details = details;

            return TransformText();
        }
    }
}

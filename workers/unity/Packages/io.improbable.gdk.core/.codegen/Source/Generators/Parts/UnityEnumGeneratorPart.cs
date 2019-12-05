using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEnumGenerator
    {
        private string qualifiedNamespace;
        private UnityEnumDetails details;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public UnityEnumGenerator()
        {
            logger.Trace($"Constructing {GetType()}");
        }

        public string Generate(UnityEnumDetails details, string package)
        {
            qualifiedNamespace = package;
            this.details = details;

            return TransformText();
        }

        public UnityEnumDetails GetEnumDetails()
        {
            return details;
        }
    }
}

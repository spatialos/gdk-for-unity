using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEnumContent
    {
        private UnityEnumDetails details;
        private string enumNamespace;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public string Generate(UnityEnumDetails details, string enumNamespace)
        {
            this.details = details;
            this.enumNamespace = enumNamespace;
            return TransformText();
        }

        public UnityEnumDetails GetEnumDetails()
        {
            return details;
        }
    }
}

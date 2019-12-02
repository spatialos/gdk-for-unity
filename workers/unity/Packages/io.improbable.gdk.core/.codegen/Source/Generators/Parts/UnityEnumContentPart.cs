using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEnumContent
    {
        private UnityEnumDetails details;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public string Generate(UnityEnumDetails details)
        {
            this.details = details;
            return TransformText();
        }

        public UnityEnumDetails GetEnumDetails()
        {
            return details;
        }
    }
}

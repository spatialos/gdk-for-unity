using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEnumContent
    {
        private UnityEnumDetails details;
        private string preamble;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public string Generate(UnityEnumDetails details, string preamble)
        {
            this.details = details;
            this.preamble = preamble;
            return TransformText();
        }

        public UnityEnumDetails GetEnumDetails()
        {
            return details;
        }
    }
}

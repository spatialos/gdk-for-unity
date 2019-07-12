
namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEnumGenerator
    {
        private string qualifiedNamespace;
        private UnityEnumDetails details;

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

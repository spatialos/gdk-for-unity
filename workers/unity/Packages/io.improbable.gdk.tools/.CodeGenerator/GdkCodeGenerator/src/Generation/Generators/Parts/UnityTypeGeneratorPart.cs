namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityTypeGenerator
    {
        private string qualifiedNamespace;
        private UnityTypeDetails details;

        public string Generate(UnityTypeDetails details, string package)
        {
            qualifiedNamespace = package;
            this.details = details;

            return TransformText();
        }
    }
}

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEnumContent
    {
        private UnityEnumDetails details;

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

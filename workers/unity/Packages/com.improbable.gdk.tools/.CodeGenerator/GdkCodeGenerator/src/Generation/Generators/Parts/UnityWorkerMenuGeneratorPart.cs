using System.Collections.Generic;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityWorkerMenuGenerator
    {
        private string qualifiedNamespace;
        private List<string> workerTypes;

        public string Generate(List<string> workerTypes, string package)
        {
            qualifiedNamespace = package;
            this.workerTypes = workerTypes;

            return TransformText();
        }

        private List<string> GetWorkerTypes()
        {
            return workerTypes;
        }
    }
}

using System.Collections.Generic;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityWorkerMenuGenerator
    {
        private List<string> workerTypes;

        public string Generate(List<string> workerTypes)
        {
            this.workerTypes = workerTypes;

            return TransformText();
        }

        private List<string> GetWorkerTypes()
        {
            return workerTypes;
        }
    }
}

using System.Collections.Generic;

namespace Playground
{
    public static class TemplateUtils
    {
        public static readonly List<string> AllWorkerAttributes =
            new List<string>
            {
                WorkerUtils.UnityGameLogic,
                WorkerUtils.UnityClient
            };
    }
}

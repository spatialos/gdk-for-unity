using System.Collections.Generic;

namespace Playground
{
    public static class EntityTemplateUtils
    {
        public static readonly List<string> AllWorkerAttributes =
            new List<string>
            {
                WorkerUtils.UnityGameLogic,
                WorkerUtils.UnityClient
            };
    }
}

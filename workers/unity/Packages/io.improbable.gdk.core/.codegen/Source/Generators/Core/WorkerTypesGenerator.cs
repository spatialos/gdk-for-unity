using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter;

namespace Improbable.Gdk.CodeGenerator
{
    public static class WorkerTypesGenerator
    {
        public static CodeWriter Generate(IReadOnlyList<string> workerTypes)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System.Collections.Generic"
                );

                cgw.Namespace("Improbable.Generated", ns =>
                {
                    ns.Type("public static class WorkerTypes", tb =>
                    {
                        foreach (var workerType in workerTypes)
                        {
                            tb.Line($"public const string {workerType} = \"{workerType}\";");
                        }

                        tb.Initializer("public static readonly IReadOnlyList<string> AllWorkerTypes = new List<string>",
                            () => workerTypes.Select(workerType => $"\"{workerType}\""));
                    });
                });
            });
        }
    }
}

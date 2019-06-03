using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Tools;

namespace Improbable.Gdk.DeploymentLauncher.Commands
{
    internal static class Assembly
    {
        public static WrappedTask<RedirectedProcessResult, AssemblyConfig> UploadAsync(AssemblyConfig config)
        {
            var source = new CancellationTokenSource();
            var token = source.Token;

            var args = new List<string>
            {
                "cloud",
                "upload",
                config.AssemblyName,
                "--project_name",
                config.ProjectName,
                "--json_output",
                "--enable_pre_upload_check=false",
            };

            if (config.ShouldForceUpload)
            {
                args.Add("--force");
            }

            var task = Task.Run(async () => await RedirectedProcess.Command(Tools.Common.SpatialBinary)
                .InDirectory(Tools.Common.SpatialProjectRootDir)
                .WithArgs(args.ToArray())
                .RedirectOutputOptions(OutputRedirectBehaviour.RedirectStdOut |
                    OutputRedirectBehaviour.RedirectStdErr | OutputRedirectBehaviour.ProcessSpatialOutput)
                .RunAsync(token));

            return new WrappedTask<RedirectedProcessResult, AssemblyConfig>
            {
                Task = task,
                CancelSource = source,
                Context = config.DeepCopy()
            };
        }
    }
}

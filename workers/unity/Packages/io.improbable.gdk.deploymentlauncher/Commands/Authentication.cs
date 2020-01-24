using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Tools;

namespace Improbable.Gdk.DeploymentLauncher.Commands
{
    internal static class Authentication
    {
        public static WrappedTask<RedirectedProcessResult, int> Authenticate()
        {
            var source = new CancellationTokenSource();
            var token = source.Token;

            var task = Task.Run(async () => await RedirectedProcess
                .Spatial("auth", "login")
                .WithArgs("--json_output")
                .InDirectory(Tools.Common.SpatialProjectRootDir)
                .RedirectOutputOptions(OutputRedirectBehaviour.RedirectStdOut |
                    OutputRedirectBehaviour.RedirectStdErr | OutputRedirectBehaviour.ProcessSpatialOutput)
                .RunAsync(token));

            return new WrappedTask<RedirectedProcessResult, int>
            {
                Task = task,
                CancelSource = source,
                Context = 0
            };
        }
    }
}

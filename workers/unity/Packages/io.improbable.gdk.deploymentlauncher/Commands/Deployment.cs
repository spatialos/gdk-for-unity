using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Core.Collections;
using Improbable.Gdk.Tools;

namespace Improbable.Gdk.DeploymentLauncher.Commands
{
    internal static class Deployment
    {
        private static string DeploymentLauncherProjectPath = Path.GetFullPath(Path.Combine(
            Tools.Common.GetPackagePath("com.improbable.gdk.deploymentlauncher"),
            ".DeploymentLauncher/DeploymentLauncher.csproj"));

        public static WrappedTask<Result<RedirectedProcessResult, Ipc.Error>, (string, string, BaseDeploymentConfig)> LaunchAsync(string projectName, string assemblyName, BaseDeploymentConfig config)
        {
            var source = new CancellationTokenSource();
            var token = source.Token;

            source.CancelAfter(TimeSpan.FromMinutes(25));

            var args = config.GetCreateArguments();
            args.Add($"--project_name={projectName}");
            args.Add($"--assembly_name={assemblyName}");

            return new WrappedTask<Result<RedirectedProcessResult, Ipc.Error>, (string, string, BaseDeploymentConfig)>
            {
                Task = RunDeploymentLauncher(args,
                    OutputRedirectBehaviour.None, token,
                    RetrieveIpcError),
                CancelSource = source,
                Context = (projectName, assemblyName, config.DeepCopy())
            };
        }

        public static WrappedTask<Result<RedirectedProcessResult, Ipc.Error>, DeploymentInfo> StopAsync(DeploymentInfo info)
        {
            var source = new CancellationTokenSource();
            var token = source.Token;

            source.CancelAfter(TimeSpan.FromMinutes(25));

            var args = new[]
            {
                "stop",
                "--project_name",
                info.ProjectName,
                "--deployment_id",
                info.Id
            };

            return new WrappedTask<Result<RedirectedProcessResult, Ipc.Error>, DeploymentInfo>
            {
                Task = RunDeploymentLauncher(args,
                    OutputRedirectBehaviour.RedirectStdOut | OutputRedirectBehaviour.RedirectStdErr,
                    token,
                    RetrieveIpcError),
                CancelSource = source,
                Context = info
            };
        }

        public static WrappedTask<Result<List<DeploymentInfo>, Ipc.Error>, string> ListAsync(string projectName)
        {
            var source = new CancellationTokenSource();
            var token = source.Token;

            source.CancelAfter(TimeSpan.FromMinutes(25));

            var args = new[]
            {
                "list",
                "--project_name",
                projectName
            };

            return new WrappedTask<Result<List<DeploymentInfo>, Ipc.Error>, string>
            {
                Task = RunDeploymentLauncher(args, OutputRedirectBehaviour.None, token,
                    res => RetrieveDeploymentList(res, projectName)),
                CancelSource = source,
                Context = projectName
            };
        }

        private static async Task<T> RunDeploymentLauncher<T>(IEnumerable<string> programArgs,
            OutputRedirectBehaviour redirectBehaviour,
            CancellationToken token,
            Func<RedirectedProcessResult, T> resultHandler)
        {
            var wrappedArgs = new[] { "run", "-p", $"\"{DeploymentLauncherProjectPath}\"" }
                .Concat(programArgs)
                .ToArray();

            var result = await RedirectedProcess.Command(Tools.Common.DotNetBinary)
                .WithArgs(wrappedArgs)
                .RedirectOutputOptions(redirectBehaviour)
                .RunAsync(token)
                .ConfigureAwait(false);

            return resultHandler(result);
        }

        private static Result<RedirectedProcessResult, Ipc.Error> RetrieveIpcError(RedirectedProcessResult result)
        {
            return result.ExitCode == 0
                ? Result<RedirectedProcessResult, Ipc.Error>.Ok(result)
                : Result<RedirectedProcessResult, Ipc.Error>.Error(Ipc.Error.FromStderr(result.Stderr));
        }

        private static Result<List<DeploymentInfo>, Ipc.Error> RetrieveDeploymentList(
            RedirectedProcessResult result, string projectName)
        {
            if (result.ExitCode == 0)
            {
                return Ipc.GetDeploymentInfo(result.Stdout, projectName);
            }

            return Result<List<DeploymentInfo>, Ipc.Error>.Error(Ipc.Error.FromStderr(result.Stderr));
        }
    }
}

using CommandLine;

namespace Improbable.Gdk.DeploymentLauncher
{
    internal class Program
    {
        public const int SuccessExitCode = 0;
        public const int ErrorExitCode = 1;

        private static int Main(string[] args)
        {
            try
            {
                // Note that the ordering of `Options.CreateSimulated` and `Options.Create` in the map is important.
                // The map operation only checks if the parsed options is T rather than checks if the parsed options
                // most child class is T.
                return Parser.Default
                    .ParseArguments<Options.Create, Options.CreateSimulated, Options.Stop, Options.List>(args)
                    .MapResult(
                        (Options.CreateSimulated createOptions) =>
                            Commands.Create.CreateSimulatedPlayerDeployment(createOptions),
                        (Options.Create createOptions) => Commands.Create.CreateDeployment(createOptions),
                        (Options.Stop stopOptions) => Commands.Stop.StopDeployment(stopOptions),
                        (Options.List listOptions) => Commands.List.ListDeployments(listOptions),
                        errs => ErrorExitCode
                    );
            }
            catch (Grpc.Core.RpcException e)
            {
                if (e.Status.StatusCode == Grpc.Core.StatusCode.Unauthenticated)
                {
                    Ipc.WriteError(Ipc.ErrorCode.Unauthenticated, "");
                    return ErrorExitCode;
                }

                Ipc.WriteError(Ipc.ErrorCode.UnknownGrpcError, e.ToString());
                return ErrorExitCode;
            }
            catch (Google.LongRunning.OperationFailedException e)
            {
                if (e.Status.Code == (int) Google.Rpc.Code.Cancelled)
                {
                    Ipc.WriteError(Ipc.ErrorCode.OperationCancelled, e.Message);
                    return ErrorExitCode;
                }

                Ipc.WriteError(Ipc.ErrorCode.UnknownGrpcError, e.ToString());
                return ErrorExitCode;
            }
            catch (SpatialOS.Platform.Common.NoRefreshTokenFoundException)
            {
                Ipc.WriteError(Ipc.ErrorCode.Unauthenticated, "");
                return ErrorExitCode;
            }
        }
    }
}

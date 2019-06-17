using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Collections;
using Improbable.Gdk.Tools.MiniJSON;

namespace Improbable.Gdk.DeploymentLauncher
{
    internal static class Ipc
    {
        public enum ErrorCode : uint
        {
            Unauthenticated = 1,
            NotFound = 2,
            UnknownGrpcError = 3,
            SnapshotUploadFailed = 4,
            OperationCancelled = 5,
            Unknown = 6,

            // Additional error code used by the parsing logic.
            CannotParseOutput = 7
        }

        public class Error
        {
            public ErrorCode Code;
            public string Message;

            public static Error FromStderr(IReadOnlyList<string> stderr)
            {
                if (stderr == null || stderr.Count == 0)
                {
                    throw new ArgumentException("Cannot parse error from empty stderr.");
                }

                try
                {
                    // We expect only the first line to be valid.
                    var deserialized = Json.Deserialize(stderr[0]);

                    if (deserialized == null)
                    {
                        return new Error
                        {
                            Code = ErrorCode.CannotParseOutput,
                            Message = $"Unable to parse the standard error. Raw standard error: {string.Join("\n", stderr)}"
                        };
                    }

                    return new Error
                    {
                        Code = (ErrorCode) Convert.ToUInt32(deserialized["Code"]),
                        Message = (string) deserialized["Message"]
                    };
                }
                catch (InvalidCastException e)
                {
                    return new Error
                    {
                        Code = ErrorCode.CannotParseOutput,
                        Message = $"Unable to parse the standard error. Raw exception: {e}\nRaw standard error: {string.Join("\n", stderr)}"
                    };
                }
                catch (KeyNotFoundException e)
                {
                    return new Error
                    {
                        Code = ErrorCode.CannotParseOutput,
                        Message = $"Unable to parse the standard error. Raw exception: {e}. Raw standard error: {string.Join("\n", stderr)}"
                    };
                }
            }
        }

        public static Result<List<DeploymentInfo>, Error> GetDeploymentInfo(IReadOnlyList<string> stdout, string projectName)
        {
            if (stdout.Count == 0)
            {
                throw new ArgumentException("Cannot parse deployment list from empty stdout.");
            }

            try
            {
                // We expect the first line to have all the JSON.
                var deserialized = Json.Deserialize(stdout[0]);
                var deployments = (List<object>) deserialized["Deployments"];

                return Result<List<DeploymentInfo>, Error>.Ok(deployments.Select(depl =>
                {
                    var json = (Dictionary<string, object>) depl;

                    return DeploymentInfo.FromJson(projectName, json);
                }).ToList());
            }
            catch (InvalidCastException e)
            {
                return Result<List<DeploymentInfo>, Error>.Error(new Error
                {
                    Code = ErrorCode.CannotParseOutput,
                    Message = $"Unable to parse the standard output.  Raw exception: {e}\nRaw standard output: {string.Join("\n", stdout)}"
                });
            }
            catch (KeyNotFoundException e)
            {
                return Result<List<DeploymentInfo>, Error>.Error(new Error
                {
                    Code = ErrorCode.CannotParseOutput,
                    Message = $"Unable to parse the standard output.  Raw exception: {e}\nRaw standard output: {string.Join("\n", stdout)}"
                });
            }
        }
    }
}

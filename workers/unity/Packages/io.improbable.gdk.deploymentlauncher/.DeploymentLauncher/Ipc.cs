using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.SpatialOS.Deployment.V1Alpha1;
using Newtonsoft.Json;

namespace Improbable.Gdk.DeploymentLauncher
{
    /// <summary>
    ///     Methods for inter-process communication with the parent process (Unity). All via JSON.
    /// </summary>
    public static class Ipc
    {
        public enum ErrorCode : uint
        {
            Unauthenticated = 1,
            NotFound = 2,
            UnknownGrpcError = 3,
            SnapshotUploadFailed = 4,
            OperationCancelled = 5,
            Unknown = 6
        }

        public static void WriteError(ErrorCode code, string message)
        {
            var json = JsonConvert.SerializeObject(new Error(code, message));
            Console.Error.WriteLine(json);
        }

        public static void WriteDeploymentInfo(IEnumerable<Deployment> deployments)
        {
            var wrapper = new DeploymentListWrapper
            {
                Deployments = deployments.Select(depl => new InternalDeployment(depl)).ToList()
            };
            var json = JsonConvert.SerializeObject(wrapper);
            Console.WriteLine(json);
        }

        private struct Error
        {
            public uint Code;
            public string Message;

            public Error(ErrorCode code, string message)
            {
                Code = (uint) code;
                Message = message;
            }
        }

        private struct InternalDeployment
        {
            public string Id;
            public string Name;
            public long StartTime;
            public string Region;
            public List<string> Tags;
            public Dictionary<string, int> Workers;

            public InternalDeployment(Deployment deployment)
            {
                Id = deployment.Id;
                Name = deployment.Name;
                StartTime = deployment.StartTime.Seconds;
                Region = deployment.RegionCode;
                Tags = deployment.Tag.ToList();
                Workers = deployment.WorkerConnectionCapacities
                    .Select(capacity => (capacity.WorkerType, capacity.MaxCapacity - capacity.RemainingCapacity))
                    .ToDictionary(pair => pair.WorkerType, pair => pair.Item2);
            }
        }

        // Force Newtonsoft to _not_ serialize the list as the root object.
        private struct DeploymentListWrapper
        {
            public List<InternalDeployment> Deployments;
        }
    }
}

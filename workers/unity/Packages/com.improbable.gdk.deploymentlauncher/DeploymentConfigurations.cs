using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Improbable.Gdk.DeploymentLauncher
{
    [Serializable]
    internal class DeploymentConfig
    {
        public class Errors
        {
            public readonly Dictionary<string, List<string>> DeplErrors = new Dictionary<string, List<string>>();

            public bool Any()
            {
                return DeplErrors.Any(pair => pair.Value.Count != 0);
            }

            public string FormatErrors()
            {
                var sb = new StringBuilder();

                foreach (var pair in DeplErrors)
                {
                    var deplName = pair.Key;

                    foreach (var error in pair.Value)
                    {
                        sb.AppendLine($" - {deplName}: {error}");
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        ///     The SpatialOS project to launch this deployment into.
        /// </summary>
        public string ProjectName;

        /// <summary>
        ///     The name of the assembly to use in the deployment.
        /// </summary>
        public string AssemblyName;

        /// <summary>
        ///     The main deployment configuration.
        /// </summary>
        public BaseDeploymentConfig Deployment;

        /// <summary>
        ///     List of simulated player deployments that will target this deployment.
        /// </summary>
        public List<SimulatedPlayerDeploymentConfig> SimulatedPlayerDeploymentConfigs;

        public DeploymentConfig()
        {
            AssemblyName = string.Empty;
            Deployment = new BaseDeploymentConfig();
            SimulatedPlayerDeploymentConfigs = new List<SimulatedPlayerDeploymentConfig>();
        }

        public Errors GetErrors()
        {
            var errors = new Errors();

            errors.DeplErrors.Add(Deployment.Name, Deployment.GetErrors().ToList());

            if (!ValidateAssembly(out var message))
            {
                errors.DeplErrors[Deployment.Name].Add(message);
            }

            foreach (var simPlayerDepl in SimulatedPlayerDeploymentConfigs)
            {
                errors.DeplErrors.Add(simPlayerDepl.Name, simPlayerDepl.GetErrors().ToList());
            }

            return errors;
        }

        /// <summary>
        ///     Deep copy this configuration object.
        /// </summary>
        /// <returns>A copy of this <see cref="DeploymentConfig" /> object.</returns>
        internal DeploymentConfig DeepCopy()
        {
            return new DeploymentConfig
            {
                ProjectName = ProjectName,
                AssemblyName = AssemblyName,
                Deployment = Deployment.DeepCopy(),
                SimulatedPlayerDeploymentConfigs =
                    SimulatedPlayerDeploymentConfigs.Select(config => config.DeepCopy()).ToList(),
            };
        }

        private bool ValidateAssembly(out string message)
        {
            if (string.IsNullOrEmpty(AssemblyName))
            {
                message = "Assembly Name cannot be empty.";
                return false;
            }

            if (!Regex.Match(AssemblyName, "^[a-zA-Z0-9_.-]{5,64}$").Success)
            {
                message =
                    $"Assembly Name \"{AssemblyName}\" is invalid. Must conform to the regex: ^[a-zA-Z0-9_.-]{{5,64}}";
                return false;
            }

            message = null;
            return true;
        }
    }

    /// <summary>
    ///     Configuration that is specific to simulated player deployments.
    /// </summary>
    [Serializable]
    internal class SimulatedPlayerDeploymentConfig : BaseDeploymentConfig
    {
        /// <summary>
        ///     The name of the deployment that the simulated players should connect into.
        /// </summary>
        public string TargetDeploymentName;

        /// <summary>
        ///     The flag prefix for the simulated player coordinator worker flags.
        /// </summary>
        public string FlagPrefix;

        /// <summary>
        ///     The simulated player coordinator worker type.
        /// </summary>
        public string WorkerType;

        /// <summary>
        ///    The index of the simulated player coordinator worker type in the list of all workers.
        /// </summary>
        public int WorkerTypeId;

        public SimulatedPlayerDeploymentConfig()
        {
            TargetDeploymentName = string.Empty;
            FlagPrefix = string.Empty;
            WorkerType = string.Empty;
            WorkerTypeId = 0;
        }

        internal new SimulatedPlayerDeploymentConfig DeepCopy()
        {
            return new SimulatedPlayerDeploymentConfig
            {
                Name = Name,
                SnapshotPath = SnapshotPath,
                LaunchJson = LaunchJson,
                Region = Region,
                Tags = Tags.ToList(),

                TargetDeploymentName = TargetDeploymentName,
                FlagPrefix = FlagPrefix,
                WorkerType = WorkerType,
                WorkerTypeId = WorkerTypeId
            };
        }

        internal override List<string> GetCreateArguments()
        {
            var args = base.GetCreateArguments();
            args[0] = "create-sim";

            args.Add($"--target_deployment={TargetDeploymentName}");
            args.Add($"--flag_prefix={FlagPrefix}");
            args.Add($"--simulated_coordinator_worker_type={WorkerType}");

            return args;
        }

        internal new IEnumerable<string> GetErrors()
        {
            {
                foreach (var error in base.GetErrors())
                {
                    yield return error;
                }
            }

            {
                if (!ValidateFlagPrefix(out var message))
                {
                    yield return message;
                }
            }

            {
                if (!ValidateWorkerType(out var message))
                {
                    yield return message;
                }
            }
        }

        private bool ValidateFlagPrefix(out string message)
        {
            if (string.IsNullOrEmpty(FlagPrefix))
            {
                message = "Flag Prefix cannot be empty.";
                return false;
            }

            if (FlagPrefix.Contains("."))
            {
                message = "Flag Prefix cannot contain full stops.";
                return false;
            }

            if (FlagPrefix.Contains(" "))
            {
                message = "Flag Prefix cannot contain spaces.";
                return false;
            }

            message = null;
            return true;
        }

        private bool ValidateWorkerType(out string message)
        {
            if (string.IsNullOrEmpty(WorkerType))
            {
                message = "Worker Type cannot be empty.";
                return false;
            }

            message = null;
            return true;
        }
    }

    [Serializable]
    internal class BaseDeploymentConfig
    {
        /// <summary>
        ///     The name of the deployment to launch.
        /// </summary>
        public string Name;

        /// <summary>
        ///     The relative path from the root of the SpatialOS project to the snapshot.
        /// </summary>
        public string SnapshotPath;

        /// <summary>
        ///     The relative path from the root of the SpatialOS project to the launch json.
        /// </summary>
        public string LaunchJson;

        /// <summary>
        ///     The region to launch the deployment in.
        /// </summary>
        public DeploymentRegionCode Region;

        /// <summary>
        ///     Tags to add to the deployment.
        /// </summary>
        public List<string> Tags;

        public BaseDeploymentConfig()
        {
            Name = string.Empty;
            SnapshotPath = string.Empty;
            LaunchJson = string.Empty;
            Region = DeploymentRegionCode.EU;
            Tags = new List<string>();
        }

        internal virtual List<string> GetCreateArguments()
        {
            var args = new List<string>
            {
                "create",
                $"--deployment_name={Name}",
                $"--launch_json_path=\"{Path.Combine(Tools.Common.SpatialProjectRootDir, LaunchJson)}\"",
                $"--region={Region.ToString()}"
            };

            if (!string.IsNullOrEmpty(SnapshotPath))
            {
                args.Add($"--snapshot_path=\"{Path.Combine(Tools.Common.SpatialProjectRootDir, SnapshotPath)}\"");
            }

            if (Tags.Count > 0)
            {
                args.Add($"--tags={string.Join(",", Tags)}");
            }

            return args;
        }

        internal BaseDeploymentConfig DeepCopy()
        {
            return new BaseDeploymentConfig
            {
                Name = Name,
                SnapshotPath = SnapshotPath,
                LaunchJson = LaunchJson,
                Region = Region,
                Tags = Tags.ToList()
            };
        }

        internal IEnumerable<string> GetErrors()
        {
            {
                if (!ValidateName(out var message))
                {
                    yield return message;
                }
            }

            {
                if (!ValidateLaunchJson(out var message))
                {
                    yield return message;
                }
            }

            {
                if (!ValidateSnapshotPath(out var message))
                {
                    yield return message;
                }
            }

            {
                foreach (var tag in Tags)
                {
                    if (!ValidateTag(tag, out var message))
                    {
                        yield return message;
                    }
                }
            }
        }

        private bool ValidateName(out string message)
        {
            if (string.IsNullOrEmpty(Name))
            {
                message = "Deployment Name cannot be empty.";
                return false;
            }

            if (!Regex.Match(Name, "^[a-z0-9_]{2,32}$").Success)
            {
                message = $"Deployment Name \"{Name}\" invalid. Must conform to the regex: ^[a-z0-9_]{{2,32}}$";
                return false;
            }

            message = null;
            return true;
        }

        private bool ValidateLaunchJson(out string message)
        {
            if (string.IsNullOrEmpty(LaunchJson))
            {
                message = "Launch Config cannot be empty";
                return false;
            }

            var filePath = Path.Combine(Tools.Common.SpatialProjectRootDir, LaunchJson);

            if (!File.Exists(filePath))
            {
                message = $"Launch Config file at {filePath} cannot be found.";
                return false;
            }

            message = null;
            return true;
        }

        private bool ValidateSnapshotPath(out string message)
        {
            if (string.IsNullOrEmpty(SnapshotPath))
            {
                message = null;
                return true;
            }

            var filePath = Path.Combine(Tools.Common.SpatialProjectRootDir, SnapshotPath);

            if (!File.Exists(filePath))
            {
                message = $"Snapshot file at {filePath} cannot be found.";
                return false;
            }

            message = null;
            return true;
        }

        private bool ValidateTag(string tag, out string message)
        {
            if (!Regex.IsMatch(tag, "^[A-Za-z0-9][A-Za-z0-9_]{2,32}$"))
            {
                message = $"Tag \"{tag}\" invalid. Must conform to the regex: ^[A-Za-z0-9][A-Za-z0-9_]{{2,32}}$";
                return false;
            }

            message = null;
            return true;
        }
    }

    internal enum DeploymentRegionCode
    {
        US,
        EU
    }

    internal class DeploymentInfo
    {
        /// <summary>
        ///     The SpatialOS project that the deployment is running in.
        /// </summary>
        public string ProjectName { get; private set; }

        /// <summary>
        ///     The name of the deployment.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     The id of the deployment.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        ///     The start time of the deployment.
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        ///     The region that the deployment is in.
        /// </summary>
        public string Region { get; private set; }

        /// <summary>
        ///     The tags on the deployment.
        /// </summary>
        public HashSet<string> Tags { get; private set; }

        /// <summary>
        ///     Describes the types and counts of workers that are currently connected to this deployment.
        /// </summary>
        public IReadOnlyDictionary<string, long> Workers { get; private set; }

        public static DeploymentInfo FromJson(string projectName, Dictionary<string, object> json)
        {
            var workers = (Dictionary<string, object>) json["Workers"];

            return new DeploymentInfo
            {
                ProjectName = projectName,
                Name = (string) json["Name"],
                Id = (string) json["Id"],
                StartTime = DateTimeOffset.FromUnixTimeSeconds((long) json["StartTime"]).ToLocalTime().DateTime,
                Region = (string) json["Region"],
                Tags = new HashSet<string>(((List<object>) json["Tags"]).Select(str => (string) str)),
                Workers = workers
                    .Select(pair => (pair.Key, (long) pair.Value))
                    .Where(pair => pair.Item2 > 0)
                    .ToDictionary(pair => pair.Item1, pair => pair.Item2)
            };
        }
    }

    [Serializable]
    internal class AssemblyConfig
    {
        /// <summary>
        ///     The name of the SpatialOS project for this assembly.
        /// </summary>
        public string ProjectName;

        /// <summary>
        ///     The name of this assembly.
        /// </summary>
        public string AssemblyName;

        /// <summary>
        ///     Denotes whether to overwrite an existing assembly when uploading.
        /// </summary>
        public bool ShouldForceUpload;

        internal AssemblyConfig DeepCopy()
        {
            return new AssemblyConfig
            {
                ProjectName = ProjectName,
                AssemblyName = AssemblyName,
                ShouldForceUpload = ShouldForceUpload
            };
        }

        public string GetError()
        {
            if (string.IsNullOrEmpty(AssemblyName))
            {
                return "Assembly Name cannot be empty.";
            }

            if (!Regex.Match(AssemblyName, "^[a-zA-Z0-9_.-]{5,64}$").Success)
            {
                return $"Assembly Name \"{AssemblyName}\" is invalid. Must conform to the regex: ^[a-zA-Z0-9_.-]{{5,64}}";
            }

            return null;
        }
    }
}

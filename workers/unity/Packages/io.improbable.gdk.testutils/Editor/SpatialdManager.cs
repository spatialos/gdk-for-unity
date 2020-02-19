using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Improbable.Gdk.Tools;
using Improbable.Gdk.Tools.MiniJSON;
using UnityEngine;

namespace Improbable.Gdk.TestUtils.Editor
{
    /// <summary>
    ///     Manages the lifecycle of SpatialD and provides methods to interact with it.
    /// </summary>
    public class SpatialdManager : IDisposable
    {
        private static readonly string SpotBinary;
        private static readonly string ProjectName;

        private static readonly string SpotShimPath;

        static SpatialdManager()
        {
            var spotPath = Path.Combine(Common.GetPackagePath("io.improbable.worker.sdk"), ".spot/spot");

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                spotPath = Path.ChangeExtension(spotPath, ".exe");
            }

            SpotBinary = spotPath;

            var spatialJsonFile = Path.Combine(Common.SpatialProjectRootDir, "spatialos.json");

            if (!File.Exists(spatialJsonFile))
            {
                throw new FileNotFoundException($"Could not find a spatialos.json file located at: {spatialJsonFile}");
            }

            var data = Json.Deserialize(File.ReadAllText(spatialJsonFile));

            if (data == null)
            {
                throw new FormatException($"Could not parse spatialos.json file located at: {spatialJsonFile}");
            }

            try
            {
                ProjectName = (string) data["name"];
            }
            catch (KeyNotFoundException e)
            {
                throw new FormatException($"Could not find a \"name\" field in {spatialJsonFile}.\n Raw exception: {e.Message}");
            }

            var packagePath = Common.GetPackagePath("io.improbable.gdk.testutils");

            SpotShimPath = Path.Combine(packagePath, ".SpotShim", "SpotShim", "SpotShim");
        }

        /// <summary>
        ///     Starts SpatialD.
        /// </summary>
        /// <remarks>
        ///     If SpatialD is already running, it will stop that instance and start a new one.
        /// </remarks>
        /// <exception cref="Exception">Thrown if this fails to start SpatialD.</exception>
        public static async Task<SpatialdManager> Start()
        {
            await RedirectedProcess
                .Spatial("service", "stop")
                .InDirectory(Common.SpatialProjectRootDir)
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .RunAsync()
                .ConfigureAwait(false);

            var result = await RedirectedProcess
                .Spatial("service", "start")
                .InDirectory(Common.SpatialProjectRootDir)
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .RunAsync()
                .ConfigureAwait(false);

            if (result.ExitCode != 0)
            {
                throw new Exception($"Could not start spatiald with error:\n {string.Join("\n", result.Stderr)}");
            }

            return new SpatialdManager();
        }

        // Hide default constructor.
        private SpatialdManager()
        {
        }

        /// <summary>
        ///     Starts a local deployment asynchronously.
        /// </summary>
        /// <param name="name">The name for the local deployment.</param>
        /// <param name="deploymentJsonPath">
        ///     The path to the launch configuration JSON relative to the root of the SpatialOS project.
        /// </param>
        /// <param name="snapshotFileName">
        ///     The name of the snapshot to use for this deployment. Must be in the snapshots directory of your
        ///     SpatialOS project.
        /// </param>
        /// <returns>A task which represents the deployment that was started.</returns>
        /// <exception cref="ArgumentException">Thrown if <see cref="deploymentJsonPath"/> does not exist.</exception>
        /// <exception cref="Exception">Thrown if the deployment fails to start.</exception>
        public async Task<LocalDeployment> StartLocalDeployment(string name, string deploymentJsonPath, string snapshotFileName = "default.snapshot")
        {
            var fullJsonPath = Path.Combine(Common.SpatialProjectRootDir, deploymentJsonPath);

            if (!File.Exists(fullJsonPath))
            {
                throw new ArgumentException($"Could not find launch config file at {fullJsonPath}");
            }

            var fullSnapshotPath = Path.Combine(Common.SpatialProjectRootDir, "snapshots", snapshotFileName);

            if (!File.Exists(fullSnapshotPath))
            {
                throw new ArgumentException($"Could not find launch config file at {fullSnapshotPath}");
            }

            var buildConfigResult = await RedirectedProcess
                .Spatial("build", "build-config")
                .InDirectory(Common.SpatialProjectRootDir)
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .RunAsync()
                .ConfigureAwait(false);

            if (buildConfigResult.ExitCode != 0)
            {
                throw new Exception($"Failed to build worker configs with error:\n {string.Join("\n", buildConfigResult.Stderr)}");
            }

            var snapshotFile = Path.GetFileNameWithoutExtension(snapshotFileName);

            var result = await RedirectedProcess.Command(SpotBinary)
                .WithArgs("alpha", "deployment", "create", "-p", ProjectName, "-n", name, "-c", $"\"{fullJsonPath}\"", "-s", snapshotFile, "--json")
                .InDirectory(Common.SpatialProjectRootDir)
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .RunAsync()
                .ConfigureAwait(false);

            if (result.ExitCode != 0)
            {
                throw new Exception($"Failed to start deployment with error:\n {string.Join("\n", result.Stderr)}");
            }

            var deploymentData = Json.Deserialize(string.Join("", result.Stdout));
            var content = (Dictionary<string, object>) deploymentData["content"];
            var id = (string) content["id"];

            return new LocalDeployment(this, id, name, ProjectName);
        }

        /// <summary>
        ///     Stops a local deployment asynchronously.
        /// </summary>
        /// <param name="deployment">The deployment to stop.</param>
        /// <returns>A task which represents the operation to stop the deployment.</returns>
        /// <exception cref="Exception">Thrown if the deployment fails to be stopped.</exception>
        public async Task StopLocalDeployment(LocalDeployment deployment)
        {
            var result = await RedirectedProcess.Command(SpotBinary)
                .WithArgs("alpha", "deployment", "delete", "-i", deployment.Id)
                .InDirectory(Common.SpatialProjectRootDir)
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .RunAsync()
                .ConfigureAwait(false);

            if (result.ExitCode != 0)
            {
                throw new Exception($"Failed to stop deployment with error:\n {string.Join("\n", result.Stderr)}");
            }
        }

        /// <summary>
        ///     Gets the details of currently running deployments asynchronously.
        /// </summary>
        /// <returns>A task which represents list of</returns>
        public async Task<List<LocalDeployment>> GetRunningDeployments()
        {
            var result = await RedirectedProcess.Command(SpotBinary)
                .WithArgs("alpha", "deployment", "list", "-p", ProjectName, "-f", "NOT_STOPPED_DEPLOYMENTS", "--json")
                .InDirectory(Common.SpatialProjectRootDir)
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .RunAsync()
                .ConfigureAwait(false);

            if (result.ExitCode != 0)
            {
                throw new Exception($"Failed to list deployments with error:\n {string.Join("\n", result.Stderr)}");
            }

            var json = Json.Deserialize(string.Join("", result.Stdout));
            if (json == null)
            {
                throw new Exception($"Failed to list deployments because there was no output. Error: \n {string.Join("\n", result.Stderr)}");
            }

            var content = (Dictionary<string, object>) json["content"];

            if (!content.TryGetValue("deployments", out var deploymentsObj))
            {
                return new List<LocalDeployment>();
            }

            var deploymentData = (List<object>) deploymentsObj;

            return deploymentData
                .OfType<Dictionary<string, object>>()
                .Select(data =>
                {
                    var id = (string) data["id"];
                    var name = (string) data["name"];

                    if (!data.TryGetValue("tag", out var tagsObj))
                    {
                        tagsObj = new List<object>();
                    }

                    var tags = (List<object>) tagsObj;
                    return new LocalDeployment(this, id, name, ProjectName, tags.Cast<string>().ToArray());
                })
                .ToList();
        }

        public void Dispose()
        {
            var result = RedirectedProcess
                .Spatial("service", "stop")
                .InDirectory(Common.SpatialProjectRootDir)
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .RunAsync()
                .Result;

            if (result.ExitCode != 0)
            {
                Debug.LogWarning($"Failed to stop spatiald with error:\n {string.Join("\n", result.Stderr)}");
            }
        }

        /// <summary>
        ///     Represents a local deployment.
        /// </summary>
        public struct LocalDeployment : IDisposable
        {
            /// <summary>
            ///     The ID of this deployment.
            /// </summary>
            public readonly string Id;

            /// <summary>
            ///     The name of this deployment.
            /// </summary>
            public readonly string Name;

            /// <summary>
            ///     The project that this deployment belongs to.
            /// </summary>
            public readonly string ProjectName;

            /// <summary>
            ///     The tags that are present on this deployment.
            /// </summary>
            public readonly List<string> Tags;

            private readonly SpatialdManager spatiald;

            internal LocalDeployment(SpatialdManager spatiald, string id, string name, string projectName, params string[] tags)
            {
                this.spatiald = spatiald;
                Id = id;
                Name = name;
                ProjectName = projectName;
                Tags = tags.ToList();
            }

            /// <summary>
            ///     Adds the "dev_login" tag to this deployment asynchronously.
            /// </summary>
            /// <returns>A task which represents the underlying operation to add the tag.</returns>
            /// <exception cref="InvalidOperationException">Thrown if the operation to set the tag fails.</exception>
            public async Task AddDevLoginTag()
            {
                // TODO: Remove shim once tag functionality is added to `spot`: UTY-2212 to track.
                var result = await RedirectedProcess.Command(Common.DotNetBinary)
                    .WithArgs("run", Id, Name, ProjectName)
                    .InDirectory(SpotShimPath)
                    .RedirectOutputOptions(OutputRedirectBehaviour.None)
                    .RunAsync()
                    .ConfigureAwait(false);

                if (result.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Failed to set deployment tag with error:\n {string.Join("\n", result.Stderr)}");
                }

                Tags.Add("dev_login");
            }

            public void Dispose()
            {
                try
                {
                    spatiald.StopLocalDeployment(this).Wait();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}

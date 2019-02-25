using System;
using System.Collections.Generic;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    /// <summary>
    /// A meta-build that describes which platforms, what build options, and what scenes to build for a particular worker type.
    /// </summary>
    [Serializable]
    internal class WorkerBuildConfiguration
    {
        /// <summary>
        /// The type of the worker. This must match with the worker definition consumed by SpatialOS.
        /// </summary>
        public string WorkerType;

        /// <summary>
        /// A list of scenes to include in the worker.
        /// </summary>
        public List<SceneAsset> ScenesForWorker = new List<SceneAsset>();

        /// <summary>
        /// Build targets to use for local iteration.
        /// </summary>
        public BuildEnvironmentConfig LocalBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.LocalBuildTargets, WorkerBuildData.GetCurrentBuildTargetConfig());

        /// <summary>
        /// Build targets to use for cloud deployment and distribution to players.
        /// </summary>
        public BuildEnvironmentConfig CloudBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.AllBuildTargets, WorkerBuildData.GetCurrentBuildTargetConfig());

        /// <summary>
        /// Returns a build environment and its options.
        /// </summary>
        public BuildEnvironmentConfig GetEnvironmentConfig(BuildEnvironment targetEnvironment)
        {
            switch (targetEnvironment)
            {
                case BuildEnvironment.Local:
                    return LocalBuildConfig;
                case BuildEnvironment.Cloud:
                    return CloudBuildConfig;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetEnvironment), targetEnvironment, null);
            }
        }
    }
}

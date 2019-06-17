using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    /// <summary>
    ///     A set of BuildTargets that can be built as a group.
    /// </summary>
    [Serializable]
    internal class BuildEnvironmentConfig
    {
        /// <summary>
        ///     Targets to build in this environment.
        /// </summary>
        public List<BuildTargetConfig> BuildTargets;

        /// <summary>
        ///     Create a new instance of BuildEnvironmentConfig.
        /// </summary>
        /// <param name="availableTargets">All of the targets that can be built in this environment.</param>
        /// <param name="targets">
        ///     A list of configured targets. If a target is present in <paramref name="availableTargets" /> but
        ///     is not in <paramref name="targets" />, then it will have the default settings the final list of build targets.
        /// </param>
        public BuildEnvironmentConfig(IEnumerable<BuildTarget> availableTargets, params BuildTargetConfig[] targets)
        {
            BuildTargets = new List<BuildTargetConfig>();

            foreach (var available in availableTargets)
            {
                var overridden = targets.FirstOrDefault(t => t.Target == available);
                if (overridden.Target == available)
                {
                    BuildTargets.Add(overridden);
                }
                else
                {
                    BuildTargets.Add(new BuildTargetConfig(available,
                        WorkerBuildData.BuildTargetDefaultOptions[available], enabled: false, required: false));
                }
            }
        }
    }
}

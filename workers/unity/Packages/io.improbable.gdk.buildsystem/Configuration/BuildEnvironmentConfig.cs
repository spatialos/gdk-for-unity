using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

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
                        WorkerBuildData.BuildTargetDefaultOptions[available], enabled: false,
                        required: false, deprecated: BuildSupportChecker.IsDeprecatedTarget(available)));
                }
            }
        }

        public IEnumerable<BuildTargetConfig> GetSupportedTargets(BuildContextFilter contextFilter)
        {
            // Filter targets for CI
            var targetConfigs = BuildTargets
                .Where(t => t.Enabled && (contextFilter.BuildTargetFilter?.Contains(t.Target) ?? true))
                .ToList();

            // Filter out any deprecated targets
            var supportedTargets = targetConfigs
                .Where(c => !c.Deprecated)
                .ToList();

            // Which build targets are not supported by current install?
            var missingTargets = supportedTargets
                .Where(c => !BuildSupportChecker.CanBuildTarget(c.Target))
                .ToList();

            // Error on missing required build support
            if (missingTargets.Any(c => c.Required))
            {
                var targetNames = string.Join(", ", missingTargets
                    .Where(c => c.Required)
                    .Select(c => c.Target.ToString()));
                throw new BuildFailedException($"Cannot build for required ({targetNames}) because build support is not installed in the Unity Editor.");
            }

            // Log builds we're skipping
            if (missingTargets.Count > 0)
            {
                var targetNames = string.Join(", ", missingTargets.Select(c => c.Target.ToString()));
                Debug.LogWarning(
                    $"Skipping ({targetNames}) because build support is not installed in the Unity Editor and the build target is not marked as 'Required'.");

                supportedTargets.RemoveAll(t => missingTargets.Contains(t));
            }

            return supportedTargets;
        }
    }
}

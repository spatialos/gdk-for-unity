using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    /// <summary>
    /// A set of BuildTargets that can be built as a group.
    /// </summary>
    [Serializable]
    public class BuildEnvironmentConfig
    {
        public List<BuildTargetConfig> BuildTargets;

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
                    BuildTargets.Add(new BuildTargetConfig(available, WorkerBuildData.BuildTargetDefaultOptions[available], false));
                }
            }
        }
    }
}

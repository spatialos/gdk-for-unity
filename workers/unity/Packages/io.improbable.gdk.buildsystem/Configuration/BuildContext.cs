using Improbable.Gdk.Core;
using UnityEditor;
using UnityEditor.Build;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    public struct BuildContext
    {
        public string WorkerType;
        public BuildTargetConfig BuildTargetConfig;
        public BuildEnvironment BuildEnvironment;
        public ScriptingImplementation ScriptingImplementation;
    }
}

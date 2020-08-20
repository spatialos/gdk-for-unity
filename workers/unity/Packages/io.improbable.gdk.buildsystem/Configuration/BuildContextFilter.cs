using System.Collections.Generic;
using Improbable.Gdk.BuildSystem.Configuration;
using UnityEditor;

namespace Improbable.Gdk.BuildSystem
{
    public struct BuildContextFilter
    {
        public IEnumerable<string> WantedWorkerTypes;
        public BuildEnvironment BuildEnvironment;
        public ScriptingImplementation? ScriptImplementation;
        public ICollection<BuildTarget> BuildTargetFilter;
        public iOSSdkVersion? IOSSdkVersion;

        public BuildContextFilter(IEnumerable<string> wantedWorkerTypes, BuildEnvironment buildEnvironment,
            ScriptingImplementation? scriptImplementation = null, ICollection<BuildTarget> buildTargetFilter = null,
            iOSSdkVersion? iosSdkVersion = null)
        {
            WantedWorkerTypes = wantedWorkerTypes;
            BuildEnvironment = buildEnvironment;
            ScriptImplementation = scriptImplementation;
            BuildTargetFilter = buildTargetFilter;
            IOSSdkVersion = iosSdkVersion;
        }

        public static BuildContextFilter Local(IEnumerable<string> wantedWorkerTypes)
        {
            return new BuildContextFilter(wantedWorkerTypes, BuildEnvironment.Local);
        }

        public static BuildContextFilter Cloud(IEnumerable<string> wantedWorkerTypes)
        {
            return new BuildContextFilter(wantedWorkerTypes, BuildEnvironment.Cloud);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Improbable.Gdk.Core;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Legacy.BuildSystem.Util
{
    /// <summary>
    ///     Allow each user to specify the location of the spatial command.
    /// </summary>
    [InitializeOnLoad]
    public sealed class SpatialCommand
    {
        private static readonly SpatialCommand instance;

        internal const string usrLocalBin = "/usr/local/bin";

        private string spatialLocation;
        private string oldSpatialLocation;
        private string discoveredLocation;
        internal Func<IList<string>> GetCommandLine { get; set; }
        internal Func<string, string, string> GetUserString { get; set; }
        internal Func<string, bool> FileExists { get; set; }
        internal Func<string, string> GetEnvironmentVariable { get; set; }

        public const string SpatialPathArgument = "spatialCommandPath";


        /// <inheritdoc cref="ISpatialOsEditorAddon" />
        public string Name => "Spatial CLI [Built-in]";

        /// <inheritdoc cref="ISpatialOsEditorAddon" />
        public string Vendor => "Improbable Worlds, Ltd.";

        /// <summary>
        ///     Returns the user-configured location of spatial[.exe], or simply "spatial" if it's not set.
        /// </summary>
        /// <remarks>
        ///     If +spatialCommandPath "/path/to/spatial" is specified on the command line, it is used in preference to any user
        ///     settings.
        ///     By default, it is assumed that "spatial" will be on the system PATH.
        /// </remarks>
        public static string SpatialPath => instance.GetSpatialPath();

        /// <summary>
        ///     Starts a process and ensures that fullPathToSpatial is available in the PATH environment variable.
        /// </summary>
        public static Process RunCommandWithSpatialInThePath(string fullPathToSpatial, ProcessStartInfo startInfo)
        {
            return SpatialRunner.RunCommandWithSpatialInThePath(fullPathToSpatial, startInfo);
        }

        private string DiscoverSpatialLocation(string location)
        {
            bool viaPath;
            return DiscoverSpatialLocation(location, out viaPath);
        }

        private string DiscoverSpatialLocation(string location, out bool viaPath)
        {
            viaPath = false;
            if (string.IsNullOrEmpty(location))
            {
                var pathValue = GetEnvironmentVariable("PATH");
                if (pathValue == null)
                {
                    return string.Empty;
                }

                var fileName = SpatialRunner.DefaultSpatialCommand;
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    fileName = Path.ChangeExtension(fileName, ".exe");
                }

                var splitPath = pathValue.Split(Path.PathSeparator);

                if (Application.platform == RuntimePlatform.OSXEditor && !splitPath.Contains(usrLocalBin))
                {
                    splitPath = splitPath.Union(new[] { usrLocalBin }).ToArray();
                }

                foreach (var path in splitPath)
                {
                    var testPath = Path.Combine(path, fileName);
                    if (FileExists(testPath))
                    {
                        viaPath = true;
                        return testPath;
                    }
                }
            }
            else
            {
                var fullLocation = location;
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    fullLocation = Path.ChangeExtension(fullLocation, ".exe");
                }

                if (FileExists(fullLocation))
                {
                    return fullLocation;
                }
            }

            return string.Empty;
        }

        static SpatialCommand()
        {
            instance = new SpatialCommand();
        }

        internal SpatialCommand()
        {
            GetCommandLine = Environment.GetCommandLineArgs;
            GetUserString = EditorPrefs.GetString;
            FileExists = File.Exists;
            GetEnvironmentVariable = Environment.GetEnvironmentVariable;
        }

        internal string GetSpatialPath()
        {
            string path;
            // The command line overrides everything.
            if (!CommandLineUtility.TryGetCommandLineValue(GetCommandLine(), SpatialPathArgument, out path))
            {
                // Then try the user-specific preferences
                path = GetUserString(SpatialRunner.CommandLocationKey, string.Empty);
            }

            // If nothing has been configured, assume it's on the system PATH, and use a sensible default of "spatial"
            if (string.IsNullOrEmpty(path))
            {
                path = DiscoverSpatialLocation(null);
            }

            return path;
        }
    }
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Legacy.BuildSystem.Util
{
    /// <summary>
    ///     Helper to allow for running the spatial CLI tool.
    /// </summary>
    internal static class SpatialRunner
    {
        // Ensure that a new script file is written during a particular session.
        private static int processRunCounter;

        private const string SpatialCommandLaunchFailure =
            "Could not launch spatial. Make sure it was on the PATH when Unity launched, " +
            "the Unity process has permissions to execute it, " +
            "and the binary is not corrupted (e.g. by running spatial update).\n" +
            "Error Code 0x{0:X8}\n{1}\n";

        public const string DefaultSpatialCommand = "spatial";

        public const string CommandLocationKey = "Improbable.Unity.Editor.Addons.SpatialCommand.Location";

        /// <summary>
        ///     Temporary solution for running external processes.
        /// </summary>
        public static void RunPausedProcess(string command, string arguments, string workingDir)
        {
            var scriptPath = Path.Combine(Path.GetTempPath(),
                string.Format("unitysdk_spatialos_command_{0}", processRunCounter++));
            string scriptCommand;
            string scriptArguments;

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                scriptCommand = "cmd";
                scriptPath += ".cmd";
                scriptArguments = string.Format("/c \"{0}\"", scriptPath);

                File.WriteAllText(scriptPath,
                    string.Format("@{0} {1}\r\n@pause\r\n@del \"{2}\"", command, arguments, scriptPath));
            }
            else
            {
                scriptCommand = "open";
                scriptPath += ".command";
                scriptArguments = string.Format("\"{0}\"", scriptPath);
                File.WriteAllText(scriptPath,
                    string
                        .Format("cd \"{0}\"\n{1} {2}\nread -n 1 -s -p \"Press any key to continue\"\nrm \"{3}\"",
                            workingDir, command, arguments, scriptPath));

                MakeScriptAsExecutable(scriptPath);
            }

            var startInfo = new ProcessStartInfo(scriptCommand, scriptArguments)
            {
                WorkingDirectory = workingDir,
                UseShellExecute = false,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal
            };

            // Try to use what the user has setup before.
            var fullPathToSpatial = EditorPrefs.GetString(CommandLocationKey, DefaultSpatialCommand);

            var process = RunCommandWithSpatialInThePath(fullPathToSpatial, startInfo);
            if (process != null)
            {
                process.Close();
            }
        }

        private static void MakeScriptAsExecutable(string scriptPath)
        {
            var chmodStartInfo = new ProcessStartInfo("chmod", string.Format("+x \"{0}\"", scriptPath))
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var chmodProcess = Process.Start(chmodStartInfo))
            {
                if (chmodProcess != null)
                {
                    chmodProcess.WaitForExit();
                }
            }
        }

        public static Process RunCommandWithSpatialInThePath(string fullPathToSpatial, ProcessStartInfo startInfo)
        {
            var spatialLocation = Path.GetDirectoryName(fullPathToSpatial);

            if (!string.IsNullOrEmpty(spatialLocation))
            {
                var newPathEnv = Environment.GetEnvironmentVariable("PATH");
                newPathEnv = string.Format("{0}{1}{2}", newPathEnv, Path.PathSeparator, spatialLocation);
                startInfo.EnvironmentVariables["PATH"] = newPathEnv;
            }

            // Ensure UNITY_HOME reflects the running instance of Unity.
            // EditorApplication.applicationContentsPath returns "<path>/Editor/Data", so strip the upper two directories.
            var combined = Path.Combine(EditorApplication.applicationContentsPath, "..");
            var applicationPath = Path.GetFullPath(Path.Combine(combined, ".."));
            startInfo.EnvironmentVariables["UNITY_HOME"] = applicationPath;

            try
            {
                return Process.Start(startInfo);
            }
            catch (Win32Exception e)
            {
                var errorMessage = string.Format(SpatialCommandLaunchFailure, e.ErrorCode, e.Message);
                throw new Exception(errorMessage);
            }
        }
    }
}

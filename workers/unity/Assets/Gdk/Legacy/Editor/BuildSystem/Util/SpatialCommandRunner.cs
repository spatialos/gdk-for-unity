using System;
using System.Diagnostics;
using System.IO;

namespace Improbable.Gdk.Legacy.BuildSystem.Util
{
    internal static class SpatialCommandRunner
    {
        internal static void RunSpatialCommand(string args, string description)
        {
            var process = SpatialCommand.RunCommandWithSpatialInThePath(SpatialCommand.SpatialPath, new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = SpatialCommand.SpatialPath,
                Arguments = args,
                CreateNoWindow = true
            });

            var output = "";
            var errOut = "";
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    output += e.Data;
                }
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    errOut += e.Data;
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception(string.Format(
                    "Could not {2}. The following error occurred: {0}, {1}\n", output,
                    errOut, description));
            }
        }
    }
}

using System;
using System.Diagnostics;

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

            var line = "";
            var output = "";
            while (line != null)
            {
                try
                {
                    line= process.StandardOutput.ReadToEnd();
                    output += line;
                }
                catch
                {
                    line = null;
                }
            }

            line = "";
            var errOut = "";
            while (line != null)
            {
                try
                {
                    line = process.StandardError.ReadToEnd();
                    errOut += line;
                }
                catch
                {
                    line = null;
                }
            }

            //var output = process.StandardOutput.ReadToEnd();
            //var errOut = process.StandardError.ReadToEnd();
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

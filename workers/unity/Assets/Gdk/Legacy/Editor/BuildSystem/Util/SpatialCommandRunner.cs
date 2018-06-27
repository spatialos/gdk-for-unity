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

            var output = GetOutput(process.StandardOutput);
            var errOut = GetOutput(process.StandardError);
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception(string.Format(
                    "Could not {2}. The following error occurred: {0}, {1}\n", output,
                    errOut, description));
            }
        }

        private static string GetOutput(TextReader reader)
        {
            var line = "";
            var output = "";
            while (line != null)
            {
                try
                {
                    line = reader.ReadLine();
                    output += line;
                }
                catch
                {
                    line = null;
                }
            }
            return output;
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

static class Common
{
    public const string CoreSdkVersion = "14.0-b5566-c80dc-WORKER-SNAPSHOT";

    public static void RunProcess(string command, params string[] arguments)
    {
        try
        {
            var info = new ProcessStartInfo(command, string.Join(" ", arguments))
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, ".."))
            };

            using (var process = Process.Start(info))
            {
                if (process == null)
                {
                    throw new Exception("Failed to start process. Is the .NET Core SDK installed?");
                }

                process.EnableRaisingEvents = true;

                process.OutputDataReceived += OnReceived;
                process.ErrorDataReceived += OnErrorReceived;

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"Exit code {process.ExitCode}");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private static void OnReceived(object sender, DataReceivedEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Data))
        {
            Debug.Log(args.Data);
        }
    }

    private static void OnErrorReceived(object sender, DataReceivedEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Data))
        {
            Debug.LogError(args.Data);
        }
    }
}

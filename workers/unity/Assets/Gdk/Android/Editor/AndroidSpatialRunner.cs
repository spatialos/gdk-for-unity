using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Android.Editor
{
    internal class AndroidSpatialRunner
    {
        private static readonly string ProjectDirectory = Path.Combine(Application.dataPath, "..", "..", "..");
        private const string ProcessName = "spatial";
        private const string ProcessParams = "local launch";
        private const string Config = "android_launch.json";

        [MenuItem("Improbable/Run Spatial for Android")]
        public static void RunAndroidSpatial()
        {
            Debug.Log($"Connect phone to IP: {GetLocalIpAddress()}");
            Process.Start(GetProcessStartInfo());
        }

        private static IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            if (ipAddress == null)
            {
                throw new NullReferenceException(
                    "Could not find local IP Address. Make sure you are connected to the Internet.");
            }

            return ipAddress;
        }

        private static ProcessStartInfo GetProcessStartInfo()
        {
            return new ProcessStartInfo
            {
                FileName = ProcessName,
                Arguments = $"{ProcessParams} {Config} --runtime_ip={GetLocalIpAddress()}",
                WorkingDirectory = ProjectDirectory,
                UseShellExecute = true
            };
        }
    }
}

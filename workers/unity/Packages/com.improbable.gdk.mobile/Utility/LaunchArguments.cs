using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public static class LaunchArguments
    {
        private const string LaunchArgumentsEnvKey = "SPATIALOS_ARGUMENTS";

        public static Dictionary<string, string> GetAndroidArguments()
        {
            if (Application.isEditor)
            {
                return new Dictionary<string, string>();
            }

            try
            {
                using (var unityPlayer = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var currentActivity = unityPlayer.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity"))
                using (var intent = currentActivity.Call<UnityEngine.AndroidJavaObject>("getIntent"))
                {
                    var hasExtra = intent.Call<bool>("hasExtra", "arguments");
                    if (hasExtra)
                    {
                        using (var extras = intent.Call<UnityEngine.AndroidJavaObject>("getExtras"))
                        {
                            var arguments = extras.Call<string>("getString", "arguments").Split(' ');
                            return CommandLineUtility.ParseCommandLineArgs(arguments);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return null;
        }

        public static Dictionary<string, string> GetiOSArguments()
        {
            if (Application.isEditor)
            {
                return new Dictionary<string, string>();
            }

            try
            {
                var argumentsEnvVar = System.Environment.GetEnvironmentVariable(LaunchArgumentsEnvKey);
                if (argumentsEnvVar != null)
                {
                    return CommandLineUtility.ParseCommandLineArgs(argumentsEnvVar.Split(' '));
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }

            return null;
        }
    }
}

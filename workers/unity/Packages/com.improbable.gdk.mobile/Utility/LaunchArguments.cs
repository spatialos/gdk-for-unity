using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public static class LaunchArguments
    {
        public const string iOSEnvironmentKey = "SPATIALOS_ARGUMENTS";

        public static Dictionary<string, string> GetArguments()
        {
            if (Application.isEditor)
            {
                return new Dictionary<string, string>();
            }

#if UNITY_ANDROID
            try
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var currentActivity = unityPlayer.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity"))
                using (var intent = currentActivity.Call<AndroidJavaObject>("getIntent"))
                {
                    var hasExtra = intent.Call<bool>("hasExtra", "arguments");
                    if (hasExtra)
                    {
                        using (var extras = intent.Call<AndroidJavaObject>("getExtras"))
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
#elif UNITY_IOS
            try
            {
                var argumentsEnvVar = System.Environment.GetEnvironmentVariable(iOSEnvironmentKey);
                if (argumentsEnvVar != null)
                {
                    return CommandLineUtility.ParseCommandLineArgs(argumentsEnvVar.Split(' '));
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
#endif

            return new Dictionary<string, string>();
        }
    }
}

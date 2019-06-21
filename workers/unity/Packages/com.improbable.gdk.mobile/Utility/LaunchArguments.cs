using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEngine;

#if UNITY_ANDROID || UNITY_IOS
using System;
#endif

namespace Improbable.Gdk.Mobile
{
    public static class LaunchArguments
    {
        public const string iOSEnvironmentKey = "SPATIALOS_ARGUMENTS";

        public static CommandLineArgs GetArguments()
        {
            if (Application.isEditor)
            {
                return CommandLineArgs.From(new Dictionary<string, string>());
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
                            return CommandLineArgs.From(arguments);
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
                    return CommandLineArgs.From(argumentsEnvVar.Split(' '));
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
#endif

            return CommandLineArgs.From(new Dictionary<string, string>());
        }
    }
}

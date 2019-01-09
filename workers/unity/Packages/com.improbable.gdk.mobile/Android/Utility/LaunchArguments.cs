using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Mobile.Android
{
    public static class LaunchArguments
    {
        public static Dictionary<string, string> GetArguments()
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

            return new Dictionary<string, string>();
        }
    }
}

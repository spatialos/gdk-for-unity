using System;
using System.Collections.Generic;
using System.IO;
using Improbable.Gdk.Tools.MiniJSON;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public static class DevAuthTokenUtils
    {
        [MenuItem("SpatialOS/Generate Dev Authentication Token", false, MenuPriorities.GenerateDevAuthToken)]
        private static void Generate()
        {
            var devAuthToken = string.Empty;
            var gdkToolsConfiguration = GdkToolsConfiguration.GetOrCreateInstance();
            var devAuthTokenDir = gdkToolsConfiguration.DevAuthTokenFullDir;
            var devAuthTokenFilePath = gdkToolsConfiguration.DevAuthTokenFilepath;
            var devAuthTokenLifetimeHours = $"{gdkToolsConfiguration.DevAuthTokenLifetimeHours}h";

            var receivedMessage = string.Empty;
            RedirectedProcess
                .Command(Common.SpatialBinary)
                .WithArgs("project", "auth", "dev-auth-token", "create", "--description", "\"Dev Auth Token\"",
                    "--lifetime", devAuthTokenLifetimeHours, "--json_output")
                .InDirectory(Common.SpatialProjectRootDir)
                .AddOutputProcessing(message => receivedMessage = message)
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .Run();

            try
            {
                if (Json.Deserialize(receivedMessage).TryGetValue("json_data", out var jsonData) &&
                    ((Dictionary<string, object>) jsonData).TryGetValue("token_secret", out var tokenSecret))
                {
                    devAuthToken = (string) tokenSecret;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to generate Dev Auth Token. {e.Message}");
                return;
            }

            if (!Directory.Exists(devAuthTokenDir))
            {
                Directory.CreateDirectory(devAuthTokenDir);
            }

            using (var writer = File.CreateText(devAuthTokenFilePath))
            {
                writer.WriteLine(devAuthToken);
            }

            Debug.Log($"Saving token {devAuthToken} to {devAuthTokenFilePath}.");
        }
    }
}

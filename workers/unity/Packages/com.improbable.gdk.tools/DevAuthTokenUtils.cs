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
        private static readonly string JsonDataKey = "json_data";
        private static readonly string TokenSecretKey = "token_secret";

        [MenuItem("SpatialOS/Generate Dev Authentication Token", false, MenuPriorities.GenerateDevAuthToken)]
        private static void Generate()
        {
            var devAuthToken = string.Empty;
            var gdkToolsConfiguration = GdkToolsConfiguration.GetOrCreateInstance();
            var devAuthTokenFullDir = gdkToolsConfiguration.DevAuthTokenFullDir;
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
                if (Json.Deserialize(receivedMessage).TryGetValue(JsonDataKey, out var jsonData) &&
                    ((Dictionary<string, object>) jsonData).TryGetValue(TokenSecretKey, out var tokenSecret))
                {
                    devAuthToken = (string) tokenSecret;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to generate Dev Auth Token. {e.Message}");
                return;
            }

            if (!Directory.Exists(devAuthTokenFullDir))
            {
                Directory.CreateDirectory(devAuthTokenFullDir);
            }

            try
            {
                File.WriteAllText(devAuthTokenFilePath, devAuthToken);
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save Dev Auth Token asset. {e.Message}");
                return;
            }

            Debug.Log($"Saving token {devAuthToken} to {devAuthTokenFilePath}.");
            AssetDatabase.ImportAsset(
                Path.Combine("Assets", gdkToolsConfiguration.DevAuthTokenDir, "DevAuthToken.txt"),
                ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();
        }
    }
}

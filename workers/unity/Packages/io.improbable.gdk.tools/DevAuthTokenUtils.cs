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
        public static string DevAuthToken => PlayerPrefs.GetString(PlayerPrefDevAuthTokenKey);

        private static string DevAuthTokenAssetPath =>
            Path.Combine("Assets", GdkToolsConfiguration.GetOrCreateInstance().DevAuthTokenDir, "DevAuthToken.txt");

        private static readonly string JsonDataKey = "json_data";
        private static readonly string JsonErrorKey = "error";
        private static readonly string JsonTokenSecretKey = "token_secret";
        private static readonly string PlayerPrefDevAuthTokenKey = "devAuthTokenSecret";

        private const string DevAuthMenuPrefix = "SpatialOS/Dev Authentication Token";
        private const string DevAuthMenuGenerateToken = "/Generate Token";
        private const string DevAuthMenuClearToken = "/Clear Token";

        [MenuItem(DevAuthMenuPrefix + DevAuthMenuGenerateToken, false, MenuPriorities.GenerateDevAuthToken)]
        public static bool TryGenerate()
        {
            var devAuthToken = string.Empty;
            var gdkToolsConfiguration = GdkToolsConfiguration.GetOrCreateInstance();

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
                var deserializedMessage = Json.Deserialize(receivedMessage);
                if (deserializedMessage.TryGetValue(JsonDataKey, out var jsonData) &&
                    ((Dictionary<string, object>) jsonData).TryGetValue(JsonTokenSecretKey, out var tokenSecret))
                {
                    devAuthToken = (string) tokenSecret;
                }
                else
                {
                    if (deserializedMessage.TryGetValue(JsonErrorKey, out var errorMessage))
                    {
                        throw new Exception(errorMessage.ToString());
                    }

                    throw new Exception(string.Empty);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to generate Dev Auth Token. {e.Message}");
                return false;
            }

            Debug.Log($"Saving token to Player Preferences.");
            PlayerPrefs.SetString(PlayerPrefDevAuthTokenKey, devAuthToken);

            if (gdkToolsConfiguration.SaveDevAuthTokenToFile)
            {
                return SaveTokenToFile();
            }

            return true;
        }

        private static bool SaveTokenToFile()
        {
            var gdkToolsConfiguration = GdkToolsConfiguration.GetOrCreateInstance();
            var devAuthTokenFullDir = gdkToolsConfiguration.DevAuthTokenFullDir;
            var devAuthTokenFilePath = gdkToolsConfiguration.DevAuthTokenFilepath;

            if (!PlayerPrefs.HasKey(PlayerPrefDevAuthTokenKey))
            {
                // Given we call SaveTokenToFile after successfully generating a Dev Auth Token,
                // we should never see the following error.
                Debug.LogError("Cannot save Development Authentication Token, as it has not been generated.");
                return false;
            }

            var devAuthToken = PlayerPrefs.GetString(PlayerPrefDevAuthTokenKey);

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
                return false;
            }

            Debug.Log($"Saving token to {devAuthTokenFilePath}.");
            AssetDatabase.ImportAsset(DevAuthTokenAssetPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();

            return true;
        }

        [MenuItem(DevAuthMenuPrefix + DevAuthMenuClearToken, false, MenuPriorities.ClearDevAuthToken)]
        private static void ClearToken()
        {
            PlayerPrefs.DeleteKey(PlayerPrefDevAuthTokenKey);
            AssetDatabase.DeleteAsset(DevAuthTokenAssetPath);
            AssetDatabase.Refresh();
        }
    }
}

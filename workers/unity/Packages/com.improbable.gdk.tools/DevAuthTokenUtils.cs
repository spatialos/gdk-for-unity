using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public static class DevAuthTokenUtils
    {
        [MenuItem("SpatialOS/Generate Dev Authentication Token", false, MenuPriorities.GenerateDevAuthToken)]
        private static void Generate()
        {
            var devAuthTokenDir = GdkToolsConfiguration.GetOrCreateInstance().DevAuthTokenDir;
            var devAuthTokenFilePath = Path.Combine(Application.dataPath, devAuthTokenDir, "DevAuthToken.txt");

            var receivedMessage = string.Empty;
            RedirectedProcess
                .Command(Common.SpatialBinary)
                .WithArgs("project", "auth", "dev-auth-token", "create", "--description", "\"Dev Auth Token\"")
                .InDirectory(Common.SpatialProjectRootDir)
                .AddOutputProcessing((message) => receivedMessage += message)
                .Run();
            var regex = new Regex(@"token_secret:\\""(.*?)\\");
            var match = regex.Match(receivedMessage);

            if (!match.Success || match.Groups.Count < 2)
            {
                Debug.LogError("Unable to generate Dev Auth Token.");
                return;
            }

            var devAuthToken = match.Groups[1].Value;

            if (!Directory.Exists(devAuthTokenDir))
            {
                Directory.CreateDirectory(devAuthTokenDir);
            }

            using (var writer = File.CreateText(devAuthTokenFilePath))
            {
                writer.WriteLine(devAuthToken);
            }

            Debug.Log($"Saving Token {devAuthToken} to {devAuthTokenFilePath}");
        }
    }
}

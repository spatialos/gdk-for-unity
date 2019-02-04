using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public class GenerateFiles
    {
        private static readonly string
            ResourcesDir = Path.GetFullPath(Path.Combine(Application.dataPath, "Resources"));

        [MenuItem("SpatialOS/Generate Dev Authentication Token", false, MenuPriorities.GenerateDevAuthToken)]
        private static void GenerateDevAuthToken()
        {
            var receivedMessage = string.Empty;
            RedirectedProcess
                .Command(Common.SpatialBinary)
                .WithArgs("project", "auth", "dev-auth-token", "create", "--description", "Dev Auth Token")
                .InDirectory(Common.SpatialProjectRootDir)
                .AddOutputProcessing((message) => receivedMessage += message)
                .Run();
            var regexString = @"""id\\"": \\""(.*?)\\""";
            var regex = new Regex(regexString);
            var devAuthToken = regex.Match(receivedMessage).Groups[1].Value;

            if (!Directory.Exists(ResourcesDir))
            {
                Directory.CreateDirectory(ResourcesDir);
            }

            using (var writer = File.CreateText(Path.Combine(ResourcesDir, "DevAuthToken.txt")))
            {
                writer.WriteLine(devAuthToken);
            }
        }
    }
}

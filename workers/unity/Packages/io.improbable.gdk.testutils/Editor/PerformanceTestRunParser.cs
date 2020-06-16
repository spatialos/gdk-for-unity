using System;
using System.IO;
using Improbable.Gdk.Core;
using Newtonsoft.Json;
using Unity.PerformanceTesting.Editor;
using UnityEngine;

namespace Improbable.Gdk.TestUtils.Editor
{
    public static class PerformanceTestRunParser
    {
        private static string xmlResultsDirKey = "xmlResultsDirectory";
        private static string jsonOutputDirKey = "jsonOutputDirectory";

        public static void Parse()
        {
            var args = CommandLineArgs.FromCommandLine();
            var xmlResultsDirectory = string.Empty;
            var jsonOutputDirectory = string.Empty;

            if (!args.TryGetCommandLineValue(xmlResultsDirKey, ref xmlResultsDirectory) ||
                !args.TryGetCommandLineValue(jsonOutputDirKey, ref jsonOutputDirectory))
            {
                Debug.LogWarning($"You must provide valid {xmlResultsDirKey} and {jsonOutputDirKey} arguments.");
                return;
            }

            var jsonOutputPath = Path.GetFullPath(jsonOutputDirectory);

            var testResults = Directory.EnumerateFiles(Path.GetFullPath(xmlResultsDirectory), "*.xml");
            foreach (var testResult in testResults)
            {
                try
                {
                    Directory.CreateDirectory(jsonOutputPath);

                    var jsonFileName = Path.ChangeExtension(Path.GetFileName(testResult), "json");
                    var jsonFilePath = Path.Combine(jsonOutputPath, jsonFileName);

                    var xmlParser = new TestResultXmlParser();
                    var run = xmlParser.GetPerformanceTestRunFromXml(testResult);
                    if (run == null)
                    {
                        Debug.LogWarning($"No result at given path: {testResult}");
                        return;
                    }

                    File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(run, Formatting.Indented));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}

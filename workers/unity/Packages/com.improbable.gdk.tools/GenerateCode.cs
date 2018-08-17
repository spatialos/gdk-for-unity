using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
//    [InitializeOnLoad]
    internal static class GenerateCode
    {
        [MenuItem("Improbable/Generate code")]
        static void Generate()
        {
            try
            {
                EditorApplication.LockReloadAssemblies();
                var projectPath = Path.GetFullPath(Path.Combine(Application.dataPath,
                    "../Packages/com.improbable.gdk.tools/.CodeGenerator/GdkCodeGenerator/GdkCodeGenerator.csproj"));
                var schemaCompilerPath = Path.GetFullPath(Path.Combine(Application.dataPath,
                    $"../CoreSdk/{Common.CoreSdkVersion}/schema_compiler/schema_compiler"));

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    schemaCompilerPath = Path.ChangeExtension(schemaCompilerPath, ".exe");
                }

                const string assetsGeneratedSource = "Assets/Generated/Source";
                Common.RunProcess("dotnet", "run", "-p", $"\"{projectPath}\"", "--", "--schema-path=\"../../schema\"",
                    "--schema-path=../../build/dependencies/schema/standard_library",
                    "--json-dir=.spatialos/ImprobableJson",
                    $"--native-output-dir={assetsGeneratedSource}",
                    $"--schema-compiler-path=\"{schemaCompilerPath}\"");                
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();
            }
            // AssetDatabase.ImportAsset(assetsGeneratedSource, ImportAssetOptions.ImportRecursive);
        }        
    }
}

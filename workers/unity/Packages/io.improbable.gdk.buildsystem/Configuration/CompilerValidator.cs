using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [InitializeOnLoad]
    public class CompilerValidator
    {
        private static bool hasCompileErrors = false;

        public static bool HasCompileErrors => hasCompileErrors;

        static CompilerValidator()
        {
            CompilationPipeline.assemblyCompilationFinished += ProcessCompileFinished;
        }

        private static void ProcessCompileFinished(string s, CompilerMessage[] compilerMessages)
        {
            hasCompileErrors = compilerMessages
                .Count(m => m.type == CompilerMessageType.Error) > 0;

            // var errCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
            // Debug.Log($"Finished with compiler errors {hasCompileErrors.ToString()} with total {errCount.ToString()}");
        }
    }
}

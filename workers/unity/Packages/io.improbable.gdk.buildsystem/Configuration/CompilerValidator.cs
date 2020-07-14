using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    [InitializeOnLoad]
    public static class CompilerValidator
    {
        private static bool hasCompileErrors;

        public static bool HasCompileErrors => hasCompileErrors;

        static CompilerValidator()
        {
            CompilationPipeline.assemblyCompilationFinished += ProcessCompileFinished;
        }

        private static void ProcessCompileFinished(string s, CompilerMessage[] compilerMessages)
        {
            hasCompileErrors = compilerMessages
                .Any(m => m.type == CompilerMessageType.Error);
        }
    }
}

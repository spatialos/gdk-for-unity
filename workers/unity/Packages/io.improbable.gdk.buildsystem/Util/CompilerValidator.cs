using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;

namespace Packages.io.improbable.gdk.buildsystem.Util
{
    [InitializeOnLoad]
    public static class CompilerValidator
    {
        private static bool hasCompileErrors;
        private static int compileErrorCount;

        public static bool HasCompileErrors => hasCompileErrors;

        public static int CompileErrorCount => compileErrorCount;

        static CompilerValidator()
        {
            CompilationPipeline.assemblyCompilationFinished += ProcessCompileFinished;
        }

        private static void ProcessCompileFinished(string s, CompilerMessage[] compilerMessages)
        {
            compileErrorCount = compilerMessages
                .Count(m => m.type == CompilerMessageType.Error);
            hasCompileErrors = compileErrorCount > 0;
        }
    }
}

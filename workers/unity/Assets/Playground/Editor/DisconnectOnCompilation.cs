using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Playground.Editor
{
    public class DisconnectOnCompilation
    {
        private static bool stopped;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            CompilationPipeline.assemblyCompilationStarted += CompilationPipeline_assemblyCompilationStarted;
            CompilationPipeline.assemblyCompilationFinished += CompilationPipeline_assemblyCompilationFinished;
        }

        private static void CompilationPipeline_assemblyCompilationStarted(string obj)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                stopped = true;
            }
        }

        private static void CompilationPipeline_assemblyCompilationFinished(string arg1, CompilerMessage[] arg2)
        {
            if (stopped)
            {
                EditorApplication.isPlaying = true;
                stopped = false;
            }
        }
    }
}

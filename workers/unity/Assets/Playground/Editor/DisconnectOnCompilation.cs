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
            CompilationPipeline.assemblyCompilationStarted += OnCompilationStarted;
            CompilationPipeline.assemblyCompilationFinished += OnCompilationFinished;
        }

        private static void OnCompilationStarted(string obj)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                stopped = true;
            }
        }

        private static void OnCompilationFinished(string arg1, CompilerMessage[] arg2)
        {
            if (stopped)
            {
                EditorApplication.isPlaying = true;
                stopped = false;
            }
        }
    }
}

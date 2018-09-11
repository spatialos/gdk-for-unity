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

        private static void OnCompilationStarted(string assemblyPath)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                stopped = true;
            }
        }

        private static void OnCompilationFinished(string assemblyPath, CompilerMessage[] compilerMessages)
        {
            if (stopped)
            {
                stopped = false;

                foreach (var message in compilerMessages)
                {
                    if (message.type == CompilerMessageType.Error)
                    {
                        return;
                    }
                }

                EditorApplication.isPlaying = true;
            }
        }
    }
}

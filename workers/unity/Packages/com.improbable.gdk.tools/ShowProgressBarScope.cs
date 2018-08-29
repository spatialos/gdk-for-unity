using System;
using UnityEditor;

namespace Improbable.Gdk.Tools
{
    internal struct ShowProgressBarScope : IDisposable
    {
        public ShowProgressBarScope(string name)
        {
            EditorUtility.DisplayProgressBar("SpatialOS for Unity", name, 0.5f);
        }

        public void Dispose()
        {
            EditorUtility.ClearProgressBar();
        }
    }
}

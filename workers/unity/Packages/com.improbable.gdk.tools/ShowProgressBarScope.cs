using System;
using UnityEditor;

namespace Improbable.Gdk.Tools
{
    /// <summary>
    ///     Use to avoid boilerplate try/finally just to ensure the progress bar is properly cleared.
    /// </summary>
    public struct ShowProgressBarScope : IDisposable
    {
        public ShowProgressBarScope(string name)
        {
            EditorUtility.DisplayProgressBar(Common.ProductName, name, 0.5f);
        }

        public void Dispose()
        {
            EditorUtility.ClearProgressBar();
        }
    }
}

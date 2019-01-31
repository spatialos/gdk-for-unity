using System;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public class GUIColorScope : IDisposable
    {
        private readonly Color oldColor;

        public GUIColorScope(Color color)
        {
            oldColor = GUI.color;
            GUI.color = color;
        }

        public void Dispose()
        {
            GUI.color = oldColor;
        }
    }
}

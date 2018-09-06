using System;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    internal class GUIColorScope : IDisposable
    {
        private readonly Color color;

        public GUIColorScope(Color newColor)
        {
            color = GUI.color;
            GUI.color = newColor;
        }

        public void Dispose()
        {
            GUI.color = color;
        }
    }
}

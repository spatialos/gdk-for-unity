using System;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public class ScopedGUIColor : IDisposable
    {
        private readonly Color oldColor;

        public ScopedGUIColor(Color color)
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

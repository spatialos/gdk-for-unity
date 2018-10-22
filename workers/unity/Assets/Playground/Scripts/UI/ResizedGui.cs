using System;
using UnityEngine;

namespace Playground
{
    internal struct ResizedGui : IDisposable
    {
        private readonly GUIStyle[] styles;
        private readonly Font[] fontCache;
        private readonly int[] fontSizeCache;

        public ResizedGui(Font font, int fontSize, params GUIStyle[] skins)
        {
            styles = skins;

            fontCache = new Font[skins.Length];
            fontSizeCache = new int[skins.Length];

            for (var i = 0; i < styles.Length; i++)
            {
                var guiStyle = styles[i];
                fontCache[i] = guiStyle.font;
                fontSizeCache[i] = guiStyle.fontSize;

                guiStyle.font = font;
                guiStyle.fontSize = fontSize;
            }
        }

        public void Dispose()
        {
            for (var i = 0; i < styles.Length; i++)
            {
                var guiStyle = styles[i];
                guiStyle.font = fontCache[i];
                guiStyle.fontSize = fontSizeCache[i];
            }
        }

        private static readonly GUIContent TempGuiContent = new GUIContent();

        public static void Label(string labelText)
        {
            TempGuiContent.text = labelText;
            GUILayout.Label(labelText, GUILayout.Height(GUI.skin.label.CalcSize(TempGuiContent).y));
        }

        public static bool Button(string text)
        {
            TempGuiContent.text = text;
            return GUILayout.Button(text, GUILayout.Height(GUI.skin.button.CalcSize(TempGuiContent).y));
        }

        public static string TextField(string text)
        {
            TempGuiContent.text = text;
            return GUILayout.TextField(text, GUILayout.Height(GUI.skin.textField.CalcSize(TempGuiContent).y));
        }

        public static string TextArea(string text)
        {
            TempGuiContent.text = text;
            return GUILayout.TextArea(text, 
                GUILayout.Height(GUI.skin.textArea.CalcHeight(TempGuiContent, Screen.width)));
        }
    }
}
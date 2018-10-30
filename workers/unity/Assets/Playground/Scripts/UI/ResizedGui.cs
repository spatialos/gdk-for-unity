using System;
using UnityEngine;

namespace Playground
{
    /// <summary>
    /// A custom GUI implementation to resize different UI elements depending on the font.
    /// This is necessary as using the default GUI input field in an iOS application will cause the
    /// app to crash. Can be removed as soon as that bug is fixed.
    /// </summary>
    internal struct ResizedGui : IDisposable
    {
        private readonly GUIStyle[] styles;
        private readonly Font[] fontCache;
        private readonly int[] fontSizeCache;

        public ResizedGui(Font font, float screenWidthFontRatio, params GUIStyle[] skins)
        {
            styles = skins;

            fontCache = new Font[skins.Length];
            fontSizeCache = new int[skins.Length];

            var actualFontSize = (int) (Screen.width * screenWidthFontRatio);

            for (var i = 0; i < styles.Length; i++)
            {
                var guiStyle = styles[i];
                fontCache[i] = guiStyle.font;
                fontSizeCache[i] = guiStyle.fontSize;

                guiStyle.font = font;
                guiStyle.fontSize = actualFontSize;
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

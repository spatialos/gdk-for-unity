using System.Linq;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    internal class WorkerChoicePopup : PopupWindowContent
    {
        public readonly string[] Choices;
        public int Choice = -1;

        private readonly Rect[] choiceRects;
        private readonly Vector2 windowSize;
        private int hover = -1;

        public WorkerChoicePopup(Rect parentRect, BuildConfig config, string[] allWorkers)
        {          
            windowSize = parentRect.size;

            Choices =
                allWorkers.Except(
                    config.WorkerBuildConfigurations.Select(w => w.WorkerType)).ToArray();

            choiceRects = new Rect[Choices.Length];

            windowSize.y = 22.0f * Choices.Length;
        }

        public override Vector2 GetWindowSize()
        {
            return windowSize;
        }

        public override void OnGUI(Rect rect)
        {
            var style = new GUIStyle(EditorStyles.miniLabel);
            var anyHover = false;

            for (var i = 0; i < Choices.Length; i++)
            {
                if (hover == i)
                {
                    style.normal.textColor = Color.white;

                    if (Event.current.type == EventType.Repaint)
                    {
                        var s = new GUIStyle();
                        s.normal.background = Texture2D.whiteTexture;

                        using (new GUIColorScope(EditorStyles.foldout.active.textColor))
                        {
                            GUI.Box(choiceRects[i], Texture2D.whiteTexture, s);
                        }
                    }
                }
                else
                {
                    style.normal.background = null;
                    style.normal.textColor = EditorStyles.miniLabel.normal.textColor;
                }

                if (GUILayout.Button(Choices[i], style))
                {
                    Choice = i;
                    editorWindow.Close();
                    break;
                }

                if (Event.current.type == EventType.Repaint)
                {
                    choiceRects[i] = GUILayoutUtility.GetLastRect();
                }

                if (Event.current.type == EventType.MouseMove &&
                    choiceRects[i].Contains(Event.current.mousePosition))
                {
                    if (hover != i)
                    {
                        hover = i;
                        editorWindow.Repaint();
                    }

                    anyHover = true;
                }
            }

            if (Event.current.type == EventType.MouseMove && !anyHover && hover != -1)
            {
                hover = -1;
                editorWindow.Repaint();
            }
        }
    }
}

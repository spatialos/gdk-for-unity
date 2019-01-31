using System.Linq;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem.Configuration
{
    internal class WorkerChoicePopup : PopupWindowContent
    {
        private readonly BuildConfig workerConfiguration;

        private readonly string[] choices;
        private readonly Rect[] choiceRects;
        private readonly Vector2 windowSize;
        private int hover = -1;

        public WorkerChoicePopup(Rect parentRect, BuildConfig config)
        {
            workerConfiguration = config;
            windowSize = parentRect.size;

            choices =
                BuildWorkerMenu.AllWorkers.Except(
                    workerConfiguration.WorkerBuildConfigurations.Select(w => w.WorkerType)).ToArray();

            choiceRects = new Rect[choices.Length];

            windowSize.y = 22.0f * choices.Length;
        }

        public override Vector2 GetWindowSize()
        {
            return windowSize;
        }

        public override void OnGUI(Rect rect)
        {
            var style = new GUIStyle(EditorStyles.miniLabel);
            var anyHover = false;

            for (var i = 0; i < choices.Length; i++)
            {
                if (hover == i)
                {
                    style.normal.textColor = Color.white;

                    if (Event.current.type == EventType.Repaint)
                    {
                        var s = new GUIStyle();
                        s.normal.background = Texture2D.whiteTexture;

                        using (new ScopedGUIColor(EditorStyles.foldout.active.textColor))
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

                if (GUILayout.Button(choices[i], style))
                {
                    EditorUtility.SetDirty(workerConfiguration);
                    Undo.RecordObject(workerConfiguration, $"Add '{choices[i]}'");

                    var config = new WorkerBuildConfiguration
                    {
                        WorkerType = choices[i],
                        LocalBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.LocalBuildTargets,
                            WorkerBuildData.GetCurrentBuildTargetConfig()),
                        CloudBuildConfig = new BuildEnvironmentConfig(WorkerBuildData.AllBuildTargets,
                            WorkerBuildData.GetLinuxBuildTargetConfig())
                    };
                    workerConfiguration.WorkerBuildConfigurations.Add(config);
                    editorWindow.Close();
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

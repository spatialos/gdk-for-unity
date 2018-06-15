using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Core.Editor
{
    public class WorkerConfigurationWindow : EditorWindow
    {
        private string[] workerTypes;

        private ScriptableWorkerConfiguration selectedWorkers;
        private Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Improbable/Configure editor workers")]
        public static void ShowWindow()
        {
            GetWindow<WorkerConfigurationWindow>(false, "Worker config", true);
        }

        private void PrepareWorkerTypes()
        {
            var assembliesPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var assemblies = Directory.GetFiles(assembliesPath, "*.dll");
            var workerTypeList = new List<string>();
            foreach (var assemblyPath in assemblies)
            {
                var assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
                var assembly = Assembly.Load(assemblyName);
                var types = assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(WorkerBase)) && !t.IsAbstract)
                    .Select(type => type.Name);
                workerTypeList.AddRange(types);
            }

            workerTypes = workerTypeList.ToArray();
        }

        private void OnEnable()
        {
            PrepareWorkerTypes();
        }

        private void InitOnce()
        {
            if (selectedWorkers == null)
            {
                selectedWorkers =
                    AssetDatabase.LoadAssetAtPath<ScriptableWorkerConfiguration>(
                        ScriptableWorkerConfiguration.AssetPath);
                if (selectedWorkers == null)
                {
                    selectedWorkers = CreateInstance<ScriptableWorkerConfiguration>();
                    AssetDatabase.CreateAsset(selectedWorkers, ScriptableWorkerConfiguration.AssetPath);
                    EditorUtility.SetDirty(selectedWorkers);
                }
            }
        }

        public void OnGUI()
        {
            InitOnce();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            GUILayout.Label("Receptionist Config");
            selectedWorkers.UseExternalIp =
                EditorGUILayout.Toggle("Use External IP", selectedWorkers.UseExternalIp);
            GUILayout.Space(15);

            GUILayout.Label("Workers");
            foreach (var workerConfig in selectedWorkers.WorkerConfigurations)
            {
                workerConfig.IsExpanded = EditorGUILayout.Foldout(workerConfig.IsExpanded, workerConfig.Name);

                if (workerConfig.IsExpanded)
                {
                    workerConfig.Name = EditorGUILayout.TextField("Name", workerConfig.Name);
                    workerConfig.IsEnabled = EditorGUILayout.Toggle("Enabled", workerConfig.IsEnabled);
                    workerConfig.TypeIndex =
                        EditorGUILayout.Popup("Type", workerConfig.TypeIndex, workerTypes);
                    workerConfig.Type = workerTypes[workerConfig.TypeIndex];
                    workerConfig.Origin = EditorGUILayout.Vector3Field("Origin", workerConfig.Origin);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Remove worker", GUILayout.Width(220)))
                        {
                            selectedWorkers.WorkerConfigurations.Remove(workerConfig);
                            EditorUtility.SetDirty(selectedWorkers);
                            return;
                        }
                    }
                }

                GUILayout.Space(5);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Create worker", GUILayout.Width(220)))
                {
                    var workerConfig = new WorkerConfiguration(workerTypes[0]);
                    selectedWorkers.WorkerConfigurations.Add(workerConfig);
                }
            }

            GUILayout.Space(15);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Save worker configuration", GUILayout.Width(220)))
                {
                    SaveWorkerConfig();
                }
            }

            GUILayout.Space(5);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Refresh worker types", GUILayout.Width(220)))
                {
                    PrepareWorkerTypes();
                }
            }

            GUILayout.EndScrollView();
        }

        private void OnDestroy()
        {
            SaveWorkerConfig();
        }

        private void SaveWorkerConfig()
        {
            EditorUtility.SetDirty(selectedWorkers);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

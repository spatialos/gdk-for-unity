using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEditor;
using UnityEngine;


namespace Improbable.Gdk.Core
{
    public class NetworkStatisticsSystem : ComponentSystem
    {
        private const int DefaultBufferSize = 60;

        // Very inefficient, but okay for now.
        private Queue<Dictionary<uint, MetricsFrame.ComponentMetrics>> updateMetrics;

        private HashSet<uint> knownComponents = new HashSet<uint>();

        protected override void OnCreate()
        {
            Enabled = false;
            updateMetrics = new Queue<Dictionary<uint, MetricsFrame.ComponentMetrics>>(60);
        }

        protected override void OnUpdate()
        {
        }

        internal void ApplyDiff(ViewDiff diff)
        {
            if (updateMetrics.Count == DefaultBufferSize)
            {
                updateMetrics.Dequeue();
            }

            var copy = diff.MetricsFrame.UpdateMetrics.ToDictionary(pair => pair.Key, pair => pair.Value);
            updateMetrics.Enqueue(copy);
        }

        internal Dictionary<uint, MetricsFrame.ComponentMetrics> GetSummaryStats()
        {
            var result = new Dictionary<uint, MetricsFrame.ComponentMetrics>();

            foreach (var element in updateMetrics)
            {
                foreach (var pair in element)
                {
                    knownComponents.Add(pair.Key);
                    result.TryGetValue(pair.Key, out var data);

                    data.Count += pair.Value.Count;
                    data.Size += pair.Value.Size;

                    result[pair.Key] = data;
                }
            }

            // TODO: Revisit.
            foreach (var id in knownComponents)
            {
                if (!result.TryGetValue(id, out var data))
                {
                    result[id] = data;
                }
            }

            return result;
        }
    }

    internal class NetworkStatsWindow : EditorWindow
    {
        private (World, string)[] potentialWorlds;
        private int currentlySelected;

        [MenuItem("SpatialOS/Network Statistics", isValidateFunction: false, priority: 1000)]
        public static void ShowWindow()
        {
            GetWindow<NetworkStatsWindow>().Show();
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Network Stats");

            potentialWorlds = World.AllWorlds
                .Where(world => world.GetExistingSystem<NetworkStatisticsSystem>() != null)
                .Select(world => (world, world.Name))
                .ToArray();
        }

        private void OnInspectorUpdate()
        {
            if (Application.isPlaying)
            {
                Repaint();
            }
        }

        private void OnGUI()
        {
            currentlySelected = EditorGUILayout.Popup("World", currentlySelected,
                potentialWorlds.Select(pair => pair.Item2).ToArray());

            var system = potentialWorlds[currentlySelected].Item1.GetExistingSystem<NetworkStatisticsSystem>();
            var stats = system.GetSummaryStats();

            var ids = stats.Keys.ToList();
            ids.Sort();

            EditorGUILayout.LabelField("Name", "Count (/s)  Average Size (B/update)  TotalSize (B)");

            foreach (var id in ids)
            {
                var metrics = stats[id];

                var name = ComponentDatabase.IdsToComponents[id].DeclaringType.Name;
                RenderLine(name, metrics.Count, metrics.Size / Mathf.Max(1, metrics.Count), metrics.Size);
            }
        }

        private void RenderLine(string name, uint count, float averageSize, uint totalSize)
        {
            EditorGUILayout.LabelField(name, $"{count}      {averageSize:F1}                {totalSize}");
        }
    }
}

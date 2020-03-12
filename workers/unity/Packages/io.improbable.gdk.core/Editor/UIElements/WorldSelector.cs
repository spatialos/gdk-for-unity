using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Core.Editor.UIElements
{
    public class WorldSelector : VisualElement
    {
        public delegate void WorldChanged(World world);

        private const string UxmlPath = "Packages/io.improbable.gdk.core/Editor/UIElements/WorldSelector.uxml";

        public World ActiveWorld { get; private set; }
        public WorldChanged OnWorldChanged;

        private readonly ToolbarMenu menuElement;

        public WorldSelector()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            template.CloneTree(this);
            menuElement = this.Q<ToolbarMenu>("worldSelector");
        }

        public void UpdateWorldSelection()
        {
            var worldMenu = menuElement.menu;

            var spatialWorlds = new List<World>();

            foreach (var world in World.All)
            {
                if (world.GetExistingSystem<WorkerSystem>() != null)
                {
                    spatialWorlds.Add(world);
                }
            }

            for (var i = 0; i < spatialWorlds.Count; i++)
            {
                var spatialWorld = spatialWorlds[i];
                worldMenu.InsertAction(i, spatialWorld.Name,
                    action => SelectWorld((World) action.userData),
                    action => ActiveWorld == (World) action.userData
                        ? DropdownMenuAction.Status.Checked
                        : DropdownMenuAction.Status.Normal,
                    spatialWorld);
            }

            // Trim excess items
            var menuSize = worldMenu.MenuItems().Count;
            for (var i = spatialWorlds.Count; i < menuSize; i++)
            {
                worldMenu.RemoveItemAt(spatialWorlds.Count);
            }

            // Update selected item if needed
            if (ActiveWorld == null || !ActiveWorld.IsCreated)
            {
                SelectWorld(spatialWorlds.FirstOrDefault());
            }
        }

        private void SelectWorld(World world)
        {
            ActiveWorld = world;
            menuElement.text = ActiveWorld?.Name ?? "No SpatialOS worlds active";
            OnWorldChanged?.Invoke(world);
        }

        public new class UxmlFactory : UxmlFactory<WorldSelector>
        {
        }
    }
}

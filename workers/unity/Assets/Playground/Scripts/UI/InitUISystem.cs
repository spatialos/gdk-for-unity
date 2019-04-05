using Improbable.Gdk.Core;
using Playground.Scripts.UI;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class InitUISystem : ComponentSystem
    {
        private ComponentUpdateSystem componentUpdateSystem;
        private ComponentGroup uiInitGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            componentUpdateSystem = World.GetExistingManager<ComponentUpdateSystem>();
            uiInitGroup = GetComponentGroup(
                ComponentType.ReadOnly<Launcher.Component>(),
                ComponentType.ReadOnly<Score.Component>(),
                ComponentType.ReadOnly<PlayerInput.ComponentAuthority>(),
                ComponentType.ReadOnly<SpatialEntityId>()
            );
            uiInitGroup.SetFilter(PlayerInput.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            var launcherData = uiInitGroup.GetComponentDataArray<Launcher.Component>();
            var scoreData = uiInitGroup.GetComponentDataArray<Score.Component>();
            var spatialIdData = uiInitGroup.GetComponentDataArray<SpatialEntityId>();

            for (var i = 0; i < spatialIdData.Length; i++)
            {
                var authUpdates =
                    componentUpdateSystem.GetAuthorityChangesReceived(spatialIdData[i].EntityId,
                        PlayerInput.ComponentId);
                if (authUpdates.Count > 0)
                {
                    var ui = Resources.Load("Prefabs/UIGameObject");
                    var inst = (GameObject) Object.Instantiate(ui, Vector3.zero, Quaternion.identity);
                    var uiComponent = inst.GetComponent<UIComponent>();
                    UIComponent.Main = uiComponent;
                    uiComponent.TestText.text = $"Energy: {launcherData[i].EnergyLeft}";
                    uiComponent.ScoreText.text = $"Score: {scoreData[i].Score}";

                    Enabled = false;
                }
            }
        }

        protected override void OnDestroyManager()
        {
            if (UIComponent.Main != null)
            {
                UnityObjectDestroyer.Destroy(UIComponent.Main.gameObject);
            }
        }
    }
}

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
        private EntityQuery uiInitGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            componentUpdateSystem = World.GetExistingSystem<ComponentUpdateSystem>();
            uiInitGroup = GetEntityQuery(
                ComponentType.ReadOnly<Launcher.Component>(),
                ComponentType.ReadOnly<Score.Component>(),
                ComponentType.ReadOnly<PlayerInput.ComponentAuthority>(),
                ComponentType.ReadOnly<SpatialEntityId>()
            );
            uiInitGroup.SetFilter(PlayerInput.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            Entities.With(uiInitGroup).ForEach(
                (ref SpatialEntityId spatialEntityId, ref Launcher.Component launcher, ref Score.Component score) =>
                {
                    var authUpdates =
                        componentUpdateSystem.GetAuthorityChangesReceived(spatialEntityId.EntityId,
                            PlayerInput.ComponentId);
                    if (authUpdates.Count > 0)
                    {
                        var ui = Resources.Load("Prefabs/UIGameObject");
                        var inst = (GameObject) Object.Instantiate(ui, Vector3.zero, Quaternion.identity);
                        var uiComponent = inst.GetComponent<UIComponent>();
                        UIComponent.Main = uiComponent;
                        uiComponent.TestText.text = $"Energy: {launcher.EnergyLeft}";
                        uiComponent.ScoreText.text = $"Score: {score.Score}";

                        Enabled = false;
                    }
                });
        }

        protected override void OnDestroy()
        {
            if (UIComponent.Main != null)
            {
                UnityObjectDestroyer.Destroy(UIComponent.Main.gameObject);
            }

            base.OnDestroy();
        }
    }
}

using Generated.Playground;
using Improbable.Gdk.Core;
using Playground.Scripts.UI;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class InitUISystem : ComponentSystem
    {
        public struct Data
        {
            public readonly int Length;
            public EntityArray Entites;
            [ReadOnly] public ComponentDataArray<SpatialOSLauncher> Launcher;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerInput;
            [ReadOnly] public ComponentArray<AuthoritiesChanged<SpatialOSPlayerInput>> PlayerInputAuthority;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var prefab = Resources.Load<GameObject>("Prefabs/UIGameObject");
                var instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                UIComponent.Main = instance.GetComponent<UIComponent>();
                UIComponent.Main.TestText.text = $"Energy: {data.Launcher[i].EnergyLeft}";
            }

            // Disable system after first run.
            Enabled = false;
        }
    }
}

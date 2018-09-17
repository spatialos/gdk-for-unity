using Improbable.Gdk.Core;
using Playground.Scripts.UI;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class InitUISystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public EntityArray Entites;
            [ReadOnly] public ComponentDataArray<Launcher.Component> Launcher;
            [ReadOnly] public ComponentDataArray<Score.Component> Score;
            [ReadOnly] public ComponentDataArray<Authoritative<PlayerInput.Component>> PlayerInput;
            [ReadOnly] public ComponentDataArray<AuthorityChanges<PlayerInput.Component>> PlayerInputAuthority;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var ui = Resources.Load("Prefabs/UIGameObject");
                var inst = (GameObject) Object.Instantiate(ui, Vector3.zero, Quaternion.identity);
                var uiComponent = inst.GetComponent<UIComponent>();
                UIComponent.Main = uiComponent;
                uiComponent.TestText.text = $"Energy: {data.Launcher[i].EnergyLeft}";
                uiComponent.ScoreText.text = $"Score: {data.Score[i].Score}";
            }

            // Disable system after first run.
            Enabled = false;
        }
    }
}

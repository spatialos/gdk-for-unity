using System;
using Improbable.Gdk.Core;
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
    public class TriggerColorChangeSystem : ComponentSystem
    {
        private struct CubeColorData
        {
            public readonly int Length;
            [ReadOnly] public SharedComponentDataArray<CubeColor.ComponentAuthority> DenotesAuthority;
            public ComponentDataArray<SpatialEntityId> Entities;
        }

        [Inject] private CubeColorData cubeColorData;
        [Inject] private ComponentUpdateSystem updateSystem;

        private Array colorValues;
        private int colorIndex;
        private float nextColorChange;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            colorValues = Enum.GetValues(typeof(Color));
        }

        protected override void OnUpdate()
        {
            if (Time.time < nextColorChange)
            {
                return;
            }

            nextColorChange = Time.time + 2;

            var colorEventData = new ColorData
            {
                Color = (Color) colorValues.GetValue(colorIndex),
            };

            for (var i = 0; i < cubeColorData.Length; i++)
            {
                if (cubeColorData.DenotesAuthority[i].HasAuthority)
                {
                    updateSystem.SendEvent(new CubeColor.ChangeColor.Event(colorEventData),
                        cubeColorData.Entities[i].EntityId);
                }
            }

            colorIndex = (colorIndex + 1) % colorValues.Length;
        }
    }
}

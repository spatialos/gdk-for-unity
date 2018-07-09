using System;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.Timing;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class TriggerColorChangeSystem : ComponentSystem
    {
        public struct CubeColorData
        {
            public int Length;
            [ReadOnly] public ComponentDataArray<EventSender<SpatialOSCubeColor>> EventSenders;
            [ReadOnly] public ComponentDataArray<ColorChangeComponent> ColorChangeData;
            [ReadOnly] public ComponentDataArray<ShouldSendColorChangeTemporary> DenotesShouldSendEvent;
            [ReadOnly] public EntityArray Entities;
        }

        [Inject] private CubeColorData cubeColorData;

        private Array colorValues;
        private int lastColorIndex;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            colorValues = Enum.GetValues(typeof(Color));
        }

        protected override void OnUpdate()
        {
            int colorIndex = (lastColorIndex + 1) % colorValues.Length;
            lastColorIndex = colorIndex;
            var nextColor = (Generated.Playground.Color) colorValues.GetValue(colorIndex);

            for (var i = 0; i < cubeColorData.Length; i++)
            {
                var colorData = new Generated.Playground.ColorData
                {
                    Color = nextColor
                };

                cubeColorData.EventSenders[i].SendChangeColorEvent(colorData);

                var timeTillNextUpdate = cubeColorData.ColorChangeData[i].TimeBetweenEvents;

                LocalTimer.AddComponentAtLocalTime<ShouldSendColorChangeTemporary>(Time.time + timeTillNextUpdate,
                    cubeColorData.Entities[i], World);
            }
        }
    }
}

using System;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;
using Unity.Collections;
using Unity.Entities;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global

#endregion

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class TriggerColorChangeSystem : ComponentSystem
    {
        public struct CubeColorData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<EventSender<SpatialOSCubeColor>> EventSenders;
        }

        [Inject] private CubeColorData cubeColorData;

        private Array colorValues;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            colorValues = Enum.GetValues(typeof(Color));
        }

        protected override void OnUpdate()
        {
            if (World.GetExistingManager<TickSystem>().GlobalTick % 100 != 0)
            {
                return;
            }

            var newColor = (Generated.Playground.Color) colorValues.GetValue(new Random().Next(colorValues.Length));

            for (var i = 0; i < cubeColorData.Length; i++)
            {
                var colorData = new Generated.Playground.ColorData
                {
                    Color = newColor
                };

                cubeColorData.EventSenders[i].SendChangeColorEvent(colorData);
            }
        }
    }
}

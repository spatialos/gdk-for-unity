using System;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class TriggerColorChangeSystem : ComponentSystem
    {
        private ComponentGroup group;
        private ComponentUpdateSystem updateSystem;

        private Array colorValues;
        private int colorIndex;
        private float nextColorChange;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            updateSystem = World.GetExistingManager<ComponentUpdateSystem>();

            group = GetComponentGroup(
                ComponentType.ReadOnly<CubeColor.ComponentAuthority>(),
                ComponentType.ReadOnly<SpatialEntityId>()
            );
            group.SetFilter(CubeColor.ComponentAuthority.Authoritative);

            colorValues = Enum.GetValues(typeof(Color));
        }

        protected override void OnUpdate()
        {
            if (Time.time < nextColorChange)
            {
                return;
            }

            nextColorChange = Time.time + 2;

            var spatialEntityIdData = group.GetComponentDataArray<SpatialEntityId>();

            var colorEventData = new ColorData
            {
                Color = (Color) colorValues.GetValue(colorIndex),
            };

            for (var i = 0; i < spatialEntityIdData.Length; i++)
            {
                updateSystem.SendEvent(new CubeColor.ChangeColor.Event(colorEventData),
                    spatialEntityIdData[i].EntityId);
            }

            colorIndex = (colorIndex + 1) % colorValues.Length;
        }
    }
}

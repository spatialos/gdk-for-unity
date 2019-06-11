using System;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class TriggerColorChangeSystem : ComponentSystem
    {
        private EntityQuery group;
        private ComponentUpdateSystem updateSystem;

        private Array colorValues;
        private int colorIndex;
        private float nextColorChange;

        protected override void OnCreate()
        {
            base.OnCreate();

            updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

            group = GetEntityQuery(
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

            var colorEventData = new ColorData
            {
                Color = (Color) colorValues.GetValue(colorIndex),
            };

            Entities.With(group).ForEach((ref SpatialEntityId spatialEntityId) =>
            {
                updateSystem.SendEvent(new CubeColor.ChangeColor.Event(colorEventData), spatialEntityId.EntityId);
            });

            colorIndex = (colorIndex + 1) % colorValues.Length;
        }
    }
}

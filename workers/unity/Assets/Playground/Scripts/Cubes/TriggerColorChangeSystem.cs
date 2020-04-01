using System;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [DisableAutoCreation]
    public class TriggerColorChangeSystem : ComponentSystem
    {
        private EntityQuery group;
        private ComponentUpdateSystem updateSystem;

        private Array colorValues;
        private int colorIndex;
        private double nextColorChange;

        protected override void OnCreate()
        {
            base.OnCreate();

            updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

            group = GetEntityQuery(
                ComponentType.ReadOnly<CubeColor.HasAuthority>(),
                ComponentType.ReadOnly<SpatialEntityId>()
            );

            colorValues = Enum.GetValues(typeof(Color));
        }

        protected override void OnUpdate()
        {
            if (Time.ElapsedTime < nextColorChange)
            {
                return;
            }

            nextColorChange = Time.ElapsedTime + 2.0;

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

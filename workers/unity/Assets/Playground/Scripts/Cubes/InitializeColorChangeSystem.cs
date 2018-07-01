using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(TriggerColorChangeSystem))]
    public class InitializeColorChangeSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;
            public ComponentDataArray<ColorChangeComponent> ColorChangeConfig;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> DenotesNewEntity;
            [ReadOnly] public EntityArray Entities;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            // Intialize behaviour on entity according to config or delay the start
            for (int i = 0; i < data.Length; ++i)
            {
                var config = data.ColorChangeConfig[i];
                config.TimeBetweenEvents = 2.0f;
                data.ColorChangeConfig[i] = config;

                PostUpdateCommands.AddComponent(data.Entities[i], new ShouldSendColorChangeTemporary());
            }
        }
    }
}

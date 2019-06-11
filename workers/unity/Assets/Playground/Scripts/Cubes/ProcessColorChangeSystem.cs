using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class ProcessColorChangeSystem : ComponentSystem
    {
        private ComponentUpdateSystem updateSystem;
        private WorkerSystem workerSystem;

        private Dictionary<Color, MaterialPropertyBlock> materialPropertyBlocks;

        protected override void OnCreate()
        {
            base.OnCreate();

            updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();
            workerSystem = World.GetExistingSystem<WorkerSystem>();

            ColorTranslationUtil.PopulateMaterialPropertyBlockMap(out materialPropertyBlocks);
        }

        protected override void OnUpdate()
        {
            var changeColorEvents = updateSystem.GetEventsReceived<CubeColor.ChangeColor.Event>();

            for (var i = 0; i < changeColorEvents.Count; i++)
            {
                var colorEvent = changeColorEvents[i];
                if (!workerSystem.TryGetEntity(colorEvent.EntityId, out var entity))
                {
                    continue;
                }

                if (EntityManager.HasComponent<MeshRenderer>(entity))
                {
                    var renderer = EntityManager.GetComponentObject<MeshRenderer>(entity);
                    var eventColor = colorEvent.Event.Payload.Color;
                    renderer.SetPropertyBlock(materialPropertyBlocks[eventColor]);
                }
            }
        }
    }
}

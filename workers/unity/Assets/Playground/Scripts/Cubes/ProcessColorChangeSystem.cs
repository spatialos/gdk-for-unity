using System.Collections.Generic;
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
    public class ProcessColorChangeSystem : ComponentSystem
    {
        [Inject] private ComponentUpdateSystem updateSystem;
        [Inject] private WorkerSystem workerSystem;

        private Dictionary<Color, MaterialPropertyBlock> materialPropertyBlocks;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
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

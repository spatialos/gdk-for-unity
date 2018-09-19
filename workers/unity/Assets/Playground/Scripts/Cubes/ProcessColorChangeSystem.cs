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
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<CubeColor.ReceivedEvents.ChangeColor> EventUpdate;
            public ComponentArray<MeshRenderer> Renderers;
        }

        [Inject] private Data data;

        private Dictionary<Color, MaterialPropertyBlock> materialPropertyBlocks;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            ColorTranslationUtil.PopulateMaterialPropertyBlockMap(out materialPropertyBlocks);
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var colorChangeEvents = data.EventUpdate[i];
                var renderer = data.Renderers[i];

                var lastEvent = colorChangeEvents.Events[colorChangeEvents.Events.Count - 1];
                renderer.SetPropertyBlock(materialPropertyBlocks[lastEvent.Color]);
            }
        }
    }
}

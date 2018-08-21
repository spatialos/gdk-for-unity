using System.Collections.Generic;
using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Color = Generated.Playground.Color;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global

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

        private static readonly Dictionary<Color, UnityEngine.Color> ColorMapping =
            new Dictionary<Color, UnityEngine.Color>
            {
                { Color.BLUE, UnityEngine.Color.blue },
                { Color.GREEN, UnityEngine.Color.green },
                { Color.YELLOW, UnityEngine.Color.yellow },
                { Color.RED, UnityEngine.Color.red }
            };

        [Inject] private Data data;

        private Dictionary<Color, MaterialPropertyBlock> materialPropertyBlocks;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            PopulateMaterialPropertyBlockMap(out materialPropertyBlocks);
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

        private static void PopulateMaterialPropertyBlockMap(
            out Dictionary<Color, MaterialPropertyBlock> materialpropertyBlocks)
        {
            materialpropertyBlocks = new Dictionary<Color, MaterialPropertyBlock>(ColorMapping.Count);

            foreach (var colorPair in ColorMapping)
            {
                var materialPropertyBlock = new MaterialPropertyBlock();
                materialPropertyBlock.SetColor("_Color", colorPair.Value);
                materialpropertyBlocks.Add(colorPair.Key, materialPropertyBlock);
            }
        }
    }
}

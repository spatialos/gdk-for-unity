using System.Collections.Generic;
using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

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

        private static readonly Dictionary<Generated.Playground.Color, UnityEngine.Color> ColorMapping =
            new Dictionary<Generated.Playground.Color, UnityEngine.Color>
            {
                { Generated.Playground.Color.BLUE, UnityEngine.Color.blue },
                { Generated.Playground.Color.GREEN, UnityEngine.Color.green },
                { Generated.Playground.Color.YELLOW, UnityEngine.Color.yellow },
                { Generated.Playground.Color.RED, UnityEngine.Color.red }
            };

        [Inject] private Data data;

        private Dictionary<Generated.Playground.Color, MaterialPropertyBlock> materialPropertyBlocks;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            PopulateMaterialPropertyBlockMap(out materialPropertyBlocks);
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var component = data.EventUpdate[i];
                var renderer = data.Renderers[i];
                foreach (var colorEvent in component.Events)
                {
                    renderer.SetPropertyBlock(materialPropertyBlocks[colorEvent.Color]);
                }
            }
        }

        private static void PopulateMaterialPropertyBlockMap(
            out Dictionary<Generated.Playground.Color, MaterialPropertyBlock> materialpropertyBlocks)
        {
            materialpropertyBlocks = new Dictionary<Generated.Playground.Color, MaterialPropertyBlock>();
            foreach (var keyValuePair in ColorMapping)
            {
                var materialPropertyBlock = new MaterialPropertyBlock();
                materialPropertyBlock.SetColor("_Color", keyValuePair.Value);
                materialpropertyBlocks.Add(keyValuePair.Key, materialPropertyBlock);
            }
        }
    }
}

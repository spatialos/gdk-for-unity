using System.Collections.Generic;
using UnityEngine;

namespace Playground
{
    public static class ColorTranslationUtil
    {
        private static readonly Dictionary<Color, UnityEngine.Color> ColorMapping =
            new Dictionary<Color, UnityEngine.Color>
            {
                { Color.BLUE, UnityEngine.Color.blue },
                { Color.GREEN, UnityEngine.Color.green },
                { Color.YELLOW, UnityEngine.Color.yellow },
                { Color.RED, UnityEngine.Color.red }
            };

        public static void PopulateMaterialPropertyBlockMap(
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

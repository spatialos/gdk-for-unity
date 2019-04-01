using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class ProcessSpinnerColorChange : MonoBehaviour
    {
#pragma warning disable 649
        [Require] private CollisionsReader collisionsReader;
        [Require] private SpinnerColorReader colorReader;
#pragma warning restore 649

        private float collideTime;
        private bool flashing;

        [SerializeField] private float flashTime = 0.2f;

        private MeshRenderer meshRenderer;

        private static Dictionary<Color, MaterialPropertyBlock> materialPropertyBlocks;
        private static MaterialPropertyBlock flashingMaterial;

        [RuntimeInitializeOnLoadMethod]
        public static void SetupColors()
        {
            flashingMaterial = new MaterialPropertyBlock();
            flashingMaterial.SetColor("_Color", UnityEngine.Color.magenta);
            ColorTranslationUtil.PopulateMaterialPropertyBlockMap(out materialPropertyBlocks);
        }

        private void OnEnable()
        {
            collisionsReader.OnPlayerCollidedEvent += HandleCollisionEvent;
            colorReader.OnColorUpdate += HandleColorChange;
        }

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogError($"No MeshRenderer on GameObject with MonoBehaviour {nameof(ProcessSpinnerColorChange)}!");
            }
        }

        private void HandleCollisionEvent(Empty empty)
        {
            collideTime = Time.time;
            flashing = true;
            meshRenderer.SetPropertyBlock(flashingMaterial);
        }

        private void HandleColorChange(Color color)
        {
            if (!flashing)
            {
                meshRenderer.SetPropertyBlock(materialPropertyBlocks[color]);
            }
        }

        private void Update()
        {
            if (flashing && Time.time - collideTime > flashTime)
            {
                meshRenderer.SetPropertyBlock(materialPropertyBlocks[colorReader.Data.Color]);
                flashing = false;
            }
        }
    }
}

using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Playground.MonoBehaviours
{
    public class PlayerColor : MonoBehaviour
    {
        private static MaterialPropertyBlock basicMaterial;
        private static MaterialPropertyBlock runningMaterial;
        private static MaterialPropertyBlock flashingMaterial;

        [RuntimeInitializeOnLoadMethod]
        public static void SetupColors()
        {
            basicMaterial = new MaterialPropertyBlock();
            basicMaterial.SetColor("_Color", Color.white);

            flashingMaterial = new MaterialPropertyBlock();
            flashingMaterial.SetColor("_Color", Color.red);

            runningMaterial = new MaterialPropertyBlock();
            runningMaterial.SetColor("_Color", Color.yellow);
        }

        [SerializeField] private float flashTime = 0.05f;

        [Require] PlayerInput.Requirables.Reader reader;

        private MeshRenderer renderer;
        private bool isRunning = false;
        private bool isFlashing = false;
        private float flashStartTime;

        void OnEnable()
        {
            if (reader != null) // TODO remove in UTY-791
            {
                reader.ComponentUpdated += OnUpdateReceived;
                reader.RunningUpdated += OnRunningChanged;
            }
        }

        void OnDisable()
        {
            if (reader != null) // TODO remove in UTY-791
            {
                reader.ComponentUpdated -= OnUpdateReceived;
                reader.RunningUpdated -= OnRunningChanged;
            }
        }

        private void Awake()
        {
            renderer = transform.Find("Shape").GetComponent<MeshRenderer>();
            Debug.Log(renderer);
        }

        private void OnRunningChanged(BlittableBool isRunningUpdate)
        {
            Debug.Log("Running update received");
            if (isRunningUpdate)
            {
                isRunning = true;
                if (!isFlashing)
                {
                    renderer.SetPropertyBlock(runningMaterial);
                }
            }
            else
            {
                isRunning = false;
                if (!isFlashing)
                {
                    renderer.SetPropertyBlock(basicMaterial);
                }
            }
        }

        private void OnUpdateReceived(SpatialOSPlayerInput.Update updatedata)
        {
            Debug.Log("Update received");
            isFlashing = true;
            flashStartTime = Time.time;
            renderer.SetPropertyBlock(flashingMaterial);
        }

        private void Update()
        {
            if (isFlashing && Time.time - flashStartTime > flashTime)
            {
                isFlashing = false;
                if (isRunning)
                {
                    renderer.SetPropertyBlock(runningMaterial);
                }
                else
                {
                    renderer.SetPropertyBlock(basicMaterial);
                }
            }
        }
    }
}

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

        [SerializeField] private float flashDurationSeconds = 0.05f;

        [Require] PlayerInput.Requirables.Reader reader;

        private MeshRenderer renderer;
        private ILogDispatcher logger;
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
            logger = GetComponent<SpatialOSComponent>()?.LogDispatcher;
            renderer = transform.Find("Shape")?.GetComponent<MeshRenderer>();
            if (logger == null)
            {
                Debug.LogError("PlayerColor behaviour on GameObject that is not connected to Spatial systems properly");
            }
            else if (renderer == null)
            {
                logger.HandleLog(LogType.Error, new LogEvent(
                    "PlayerColor behaviour not on player character of expected structure")
                    .WithField("GameObject", gameObject));
            }
        }

        private void OnRunningChanged(BlittableBool isRunningUpdate)
        {
            isRunning = isRunningUpdate;
            if (!isFlashing)
            {
                renderer.SetPropertyBlock(isRunning ? runningMaterial : basicMaterial);
            }
        }

        private void OnUpdateReceived(SpatialOSPlayerInput.Update updatedata)
        {
            isFlashing = true;
            flashStartTime = Time.time;
            renderer.SetPropertyBlock(flashingMaterial);
        }

        private void Update()
        {
            if (isFlashing && Time.time - flashStartTime > flashDurationSeconds)
            {
                isFlashing = false;
                renderer.SetPropertyBlock(isRunning ? runningMaterial : basicMaterial);
            }
        }
    }
}

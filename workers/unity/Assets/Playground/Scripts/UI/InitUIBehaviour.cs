using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Playground.Scripts.UI
{
    public class InitUIBehaviour : MonoBehaviour
    {
        [Require] private PlayerInputWriter playerInput;
        [Require] private ScoreReader score;
        [Require] private LauncherReader launcher;

        [SerializeField] private GameObject uiPrefab;
        private UIComponent uiComponent;

        private void OnEnable()
        {
            var inst = Instantiate(uiPrefab, Vector3.zero, Quaternion.identity);

            uiComponent = inst.GetComponent<UIComponent>();
            uiComponent.TestText.text = $"Energy: {launcher.Data.EnergyLeft}";
            uiComponent.ScoreText.text = $"Score: {score.Data.Score}";

            UIComponent.Main = uiComponent;
        }

        private void OnDisable()
        {
            if (uiComponent != null)
            {
                Destroy(uiComponent.gameObject);
            }
        }
    }
}

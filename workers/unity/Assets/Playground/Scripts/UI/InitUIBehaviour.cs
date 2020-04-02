using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Playground;
using Playground.Scripts.UI;
using UnityEngine;

namespace Assets.Playground.Scripts.UI
{
    public class InitUIBehaviour : MonoBehaviour
    {
        [Require] private PlayerInputWriter playerInput;
        [Require] private ScoreReader score;
        [Require] private LauncherReader launcher;

        private void OnEnable()
        {
            var ui = Resources.Load("Prefabs/UIGameObject");
            var inst = (GameObject) Instantiate(ui, Vector3.zero, Quaternion.identity);
            var uiComponent = inst.GetComponent<UIComponent>();
            UIComponent.Main = uiComponent;
            uiComponent.TestText.text = $"Energy: {launcher.Data.EnergyLeft}";
            uiComponent.ScoreText.text = $"Score: {score.Data.Score}";
        }

        private void OnDisable()
        {
            if (UIComponent.Main != null)
            {
                UnityObjectDestroyer.Destroy(UIComponent.Main.gameObject);
            }
        }
    }
}

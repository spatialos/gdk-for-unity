using UnityEngine;
using UnityEngine.UI;

namespace Playground.Scripts.UI
{
    public class UIComponent : MonoBehaviour
    {
        public static UIComponent Main;

        public GameObject CanvasGameObject;
        public Canvas Canvas;
        public Text TestText;
        public Text ScoreText;
        public RawImage Reticle;
    }
}

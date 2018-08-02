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


        public static Rect ToScreenRect(RectTransform transform)
        {
            var size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            var x = transform.position.x + transform.anchoredPosition.x;
            var y = Screen.height - transform.position.y - transform.anchoredPosition.y;

            return new Rect(x, y, size.x, size.y);
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image bgImg;
    [SerializeField] private Image joystickImg;

    public Vector2 InputDirection;
    private int? lastTouch;

    public void Start()
    {
        InputDirection = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!lastTouch.HasValue || eventData.pointerId != lastTouch)
        {
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, eventData.position,
            eventData.pressEventCamera, out var pos))
        {
            pos /= bgImg.rectTransform.sizeDelta;

            var x = bgImg.rectTransform.pivot.x == 1 ? pos.x * 2 + 1 : pos.x * 2 - 1;
            var y = bgImg.rectTransform.pivot.y == 1 ? pos.y * 2 + 1 : pos.y * 2 - 1;

            InputDirection = new Vector2(x, y);
            InputDirection = InputDirection.sqrMagnitude > 1 ? InputDirection.normalized : InputDirection;

            joystickImg.rectTransform.anchoredPosition = InputDirection * bgImg.rectTransform.sizeDelta / 3;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (lastTouch.HasValue && eventData.pointerId == lastTouch.Value)
        {
            InputDirection = Vector3.zero;
            joystickImg.rectTransform.anchoredPosition = Vector3.zero;
            lastTouch = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!lastTouch.HasValue)
        {
            lastTouch = eventData.pointerId;
            OnDrag(eventData);
        }
    }
}

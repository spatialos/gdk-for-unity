using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImg;
    private Image joystickImg;

    public Vector3 InputDirection;

    public void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
        InputDirection = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, eventData.position,
            eventData.pressEventCamera, out pos))
        {
            pos = pos / bgImg.rectTransform.sizeDelta;

            var x = bgImg.rectTransform.pivot.x == 1 ? pos.x * 2 + 1 : pos.x * 2 - 1;
            var y = bgImg.rectTransform.pivot.y == 1 ? pos.y * 2 + 1 : pos.y * 2 - 1;

            InputDirection = new Vector3(x, y, 0);
            InputDirection = InputDirection.magnitude > 1 ? InputDirection.normalized : InputDirection;

            joystickImg.rectTransform.anchoredPosition =
                new Vector3(InputDirection.x * (bgImg.rectTransform.sizeDelta.x / 3),
                    InputDirection.y * (bgImg.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
}

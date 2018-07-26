﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image bgImg;
    [SerializeField] private Image joystickImg;

    public Vector2 InputDirection;

    public void Start()
    {
        InputDirection = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, eventData.position,
            eventData.pressEventCamera, out var pos))
        {
            pos /= bgImg.rectTransform.sizeDelta;

            var x = bgImg.rectTransform.pivot.x == 1 ? pos.x * 2 + 1 : pos.x * 2 - 1;
            var y = bgImg.rectTransform.pivot.y == 1 ? pos.y * 2 + 1 : pos.y * 2 - 1;

            InputDirection = new Vector2(x, y);
            InputDirection = InputDirection.sqrMagnitude > 1 ? InputDirection.normalized : InputDirection;

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

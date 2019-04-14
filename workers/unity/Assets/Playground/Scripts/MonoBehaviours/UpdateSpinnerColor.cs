using System;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class UpdateSpinnerColor : MonoBehaviour
    {
#pragma warning disable 649
        [Require] private SpinnerColorWriter spinnerColorWriter;
#pragma warning restore 649

        private Array colorValues;
        private int colorIndex;
        private float nextColorChangeTime;

        private void Awake()
        {
            colorValues = Enum.GetValues(typeof(Color));
        }

        private void Update()
        {
            if (Time.time < nextColorChangeTime)
            {
                return;
            }

            colorIndex = (colorIndex + 1) % colorValues.Length;
            nextColorChangeTime = Time.time + 2;

            spinnerColorWriter.SendUpdate(new SpinnerColor.Update
            {
                Color = (Color) colorValues.GetValue(colorIndex)
            });
        }
    }
}

using System;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

#endregion

namespace Playground.MonoBehaviours
{
    public class UpdateSpinnerColor : MonoBehaviour
    {
        [Require] private SpinnerColorWriter spinnerColorWriter;

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

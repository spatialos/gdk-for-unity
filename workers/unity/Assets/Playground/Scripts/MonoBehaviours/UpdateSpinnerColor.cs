using System;
using Improbable.Gdk.GameObjectRepresentation;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class UpdateSpinnerColor : MonoBehaviour
    {
        [Require] private SpinnerColor.Requirable.Writer writer;

        private Array colorValues;
        private int colorIndex = 0;
        private float nextColorChangeTime = 0;

        void Awake()
        {
            colorValues = Enum.GetValues(typeof(Color));
        }

        void Update()
        {
            if (Time.time < nextColorChangeTime)
            {
                return;
            }

            colorIndex = (colorIndex + 1) % colorValues.Length;
            nextColorChangeTime = Time.time + 2;

            writer.Send(new SpinnerColor.Update
            {
                Color = (Color) colorValues.GetValue(colorIndex)
            });
        }
    }
}

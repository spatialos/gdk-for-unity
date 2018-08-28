using System;
using Generated.Playground;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;
using Color = Generated.Playground.Color;

namespace Playground.MonoBehaviours
{
    public class UpdateSpinnerColor : MonoBehaviour
    {
        [Require] private SpinnerColor.Requirables.Writer writer;

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

            writer.Send(new SpatialOSSpinnerColor.Update
            {
                Color = (Color) colorValues.GetValue(colorIndex)
            });
        }
    }
}

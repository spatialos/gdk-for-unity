using Improbable.Common;
using Improbable.Gdk.GameObjectRepresentation;
using UnityEngine;

#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

#endregion

namespace Playground.MonoBehaviours
{
    public class ToggleRotationCommandReceiver : MonoBehaviour
    {
        public float TimeBetweenSpinChanges = 1.0f;

        [Require] private SpinnerRotation.Requirable.CommandRequestHandler requestHandler;

        private RotationBehaviour rotationBehaviour;

        private float nextAvailableSpinChangeTime;

        private void OnEnable()
        {
            rotationBehaviour = GetComponent<RotationBehaviour>();
            requestHandler.OnSpinnerToggleRotationRequest += OnSpinnerToggleRotationRequest;
        }

        private void OnDisable()
        {
            nextAvailableSpinChangeTime = Time.time;
        }

        private void OnSpinnerToggleRotationRequest(SpinnerRotation.SpinnerToggleRotation.RequestResponder spinnerToggleRotationRequest)
        {
            if (Time.time < nextAvailableSpinChangeTime)
            {
                spinnerToggleRotationRequest.SendResponseFailure("Cannot change spinning direction too frequently.");
            }
            else
            {
                rotationBehaviour.RotatingClockWise = !rotationBehaviour.RotatingClockWise;

                nextAvailableSpinChangeTime = Time.time + TimeBetweenSpinChanges;

                spinnerToggleRotationRequest.SendResponse(new Empty());
            }
        }
    }
}

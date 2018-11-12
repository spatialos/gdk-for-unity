using Improbable.Common;
using Improbable.Gdk.Subscriptions;
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

        [Require] private SpinnerRotationCommandReceiver requestHandler;

        private RotationBehaviour rotationBehaviour;

        private float nextAvailableSpinChangeTime;

        private void OnEnable()
        {
            rotationBehaviour = GetComponent<RotationBehaviour>();
            requestHandler.OnSpinnerToggleRotationRequestReceived += OnSpinnerToggleRotationRequest;
        }

        private void OnDisable()
        {
            nextAvailableSpinChangeTime = Time.time;
        }

        private void OnSpinnerToggleRotationRequest(SpinnerRotation.SpinnerToggleRotation.ReceivedRequest spinnerToggleRotationRequest)
        {
            if (Time.time < nextAvailableSpinChangeTime)
            {
                requestHandler.SendSpinnerToggleRotationResponse(new SpinnerRotation.SpinnerToggleRotation.Response(
                    spinnerToggleRotationRequest.RequestId, "Cannot change spinning direction too frequently."));
            }
            else
            {
                rotationBehaviour.RotatingClockWise = !rotationBehaviour.RotatingClockWise;

                nextAvailableSpinChangeTime = Time.time + TimeBetweenSpinChanges;

                requestHandler.SendSpinnerToggleRotationResponse(
                    new SpinnerRotation.SpinnerToggleRotation.Response(spinnerToggleRotationRequest.RequestId,
                        new Empty()));
            }
        }
    }
}

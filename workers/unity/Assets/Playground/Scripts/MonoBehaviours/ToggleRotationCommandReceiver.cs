using Generated.Playground;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class ToggleRotationCommandReceiver : MonoBehaviour
    {
        [Require] private SpinnerRotation.Requirable.CommandRequestHandler requestHandler;

        private RotationBehaviour rotationBehaviour;

        private float nextAvailableSpinChangeTime;

        public float timeBetweenSpinChanges = 1.0f;

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

                nextAvailableSpinChangeTime = Time.time + timeBetweenSpinChanges;

                spinnerToggleRotationRequest.SendResponse(new Empty());
            }
        }
    }
}

using Generated.Playground;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class ToggleRotationCommandReceiver : MonoBehaviour
    {
        [Require] private SpinnerRotation.Requirables.CommandRequestHandler requestHandler;

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
            requestHandler.OnSpinnerToggleRotationRequest -= OnSpinnerToggleRotationRequest;

            nextAvailableSpinChangeTime = Time.time;
        }

        private void OnSpinnerToggleRotationRequest(SpinnerRotation.SpinnerToggleRotation.ReceivedRequest request)
        {
            SpinnerRotation.SpinnerToggleRotation.Response response;

            if (Time.time < nextAvailableSpinChangeTime)
            {
                response = SpinnerRotation
                    .SpinnerToggleRotation
                    .CreateResponseFailure(request, "Cannot change spinning direction too frequently.");
            }
            else
            {
                rotationBehaviour.RotatingClockWise = !rotationBehaviour.RotatingClockWise;

                nextAvailableSpinChangeTime = Time.time + timeBetweenSpinChanges;

                response = SpinnerRotation
                    .SpinnerToggleRotation
                    .CreateResponse(request, new Void());
            }

            requestHandler.SendSpinnerToggleRotationResponse(response);
        }
    }
}

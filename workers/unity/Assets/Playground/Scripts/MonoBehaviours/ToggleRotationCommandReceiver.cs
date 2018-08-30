using Generated.Playground;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class ToggleRotationCommandReceiver : MonoBehaviour
    {
        [Require] private SpinnerRotation.Requirables.CommandRequestHandler requestHandler;
        private RotationBehaviour rotationBehaviour;

        private void OnEnable()
        {
            rotationBehaviour = GetComponent<RotationBehaviour>();
            requestHandler.OnSpinnerToggleRotationRequest += OnSpinnerToggleRotationRequest;
        }

        private void OnDisable()
        {
            requestHandler.OnSpinnerToggleRotationRequest -= OnSpinnerToggleRotationRequest;
        }

        private void OnSpinnerToggleRotationRequest(SpinnerRotation.SpinnerToggleRotation.ReceivedRequest request)
        {
            rotationBehaviour.RotatingClockWise = !rotationBehaviour.RotatingClockWise;
        }
    }
}

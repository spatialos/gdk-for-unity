using Generated.Playground;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class ToggleRotationCommandReceiver : MonoBehaviour
    {
        [Require] private MonoBehaviourTest.Requirables.CommandRequestHandler requestHandler;
        private RotationBehaviour rotationBehaviour;

        private void OnEnable()
        {
            rotationBehaviour = GetComponent<RotationBehaviour>();
            if (requestHandler != null) // TODO UTY-791: Needed until prefab preprocessing is implemented, remove as part of UTY-791
            {
                requestHandler.OnSpinnerToggleRotationRequest += OnSpinnerToggleRotationRequest;
            }
        }

        private void OnSpinnerToggleRotationRequest(MonoBehaviourTest.SpinnerToggleRotation.ReceivedRequest request)
        {
            rotationBehaviour.RotatingClockWise = !rotationBehaviour.RotatingClockWise;
        }
    }
}

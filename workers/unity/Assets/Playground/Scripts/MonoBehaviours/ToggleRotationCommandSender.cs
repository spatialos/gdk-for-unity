using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class ToggleRotationCommandSender : MonoBehaviour
    {
        [Require] private SpinnerRotation.Requirables.Reader reader;
        [Require] private SpinnerRotation.Requirables.CommandRequestSender requestSender;
        private EntityId ownEntityId;

        private void OnEnable()
        {
            ownEntityId = GetComponent<SpatialOSComponent>().SpatialEntityId;
            requestSender.OnSpinnerToggleRotationResponse += ResponseHandlerOnOnSpinnerToggleRotationResponse;
        }

        public void OnDisable()
        {
            requestSender.OnSpinnerToggleRotationResponse -= ResponseHandlerOnOnSpinnerToggleRotationResponse;
        }

        private void ResponseHandlerOnOnSpinnerToggleRotationResponse(
            SpinnerRotation.SpinnerToggleRotation.ReceivedResponse obj)
        {
            if (obj.StatusCode != StatusCode.Success)
            {
                GetComponent<SpatialOSComponent>().LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent($"Spin command request failed: {obj.StatusCode}, message: {obj.Message}"));
            }
        }

        private void Update()
        {
            if (reader.Authority != Authority.NotAuthoritative)
            {
                // Perform sending logic only on non-authoritative workers.
                return;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                requestSender.SendSpinnerToggleRotationRequest(ownEntityId, new Void());
            }
        }
    }
}

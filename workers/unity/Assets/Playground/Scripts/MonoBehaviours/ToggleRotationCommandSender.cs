using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

#endregion

namespace Playground.MonoBehaviours
{
    public class ToggleRotationCommandSender : MonoBehaviour
    {
        [Require] private SpinnerRotationReader reader;
        [Require] private SpinnerRotationCommandSender requestSender;

        //[SamiRequire] private SpinnerRotation.Requirable.CommandResponseHandler responseHandler;

        [Require] private EntityId ownEntityId;

        private void OnEnable()
        {
            //ownEntityId = GetComponent<SpatialOSComponent>().SpatialEntityId;
            //responseHandler.OnSpinnerToggleRotationResponse += ResponseHandlerOnOnSpinnerToggleRotationResponse;
        }

        private void ResponseHandlerOnOnSpinnerToggleRotationResponse(
            SpinnerRotation.SpinnerToggleRotation.ReceivedResponse obj)
        {
            if (obj.StatusCode != StatusCode.Success)
            {
                // GetComponent<SpatialOSComponent>().Worker.LogDispatcher.HandleLog(LogType.Error,
                //     new LogEvent($"Spin command request failed: {obj.StatusCode}, message: {obj.Message}"));
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
                var request = SpinnerRotation.SpinnerToggleRotation.CreateRequest(ownEntityId, new Empty());
                requestSender.SendSpinnerToggleRotationCommand(request);
            }
        }
    }
}

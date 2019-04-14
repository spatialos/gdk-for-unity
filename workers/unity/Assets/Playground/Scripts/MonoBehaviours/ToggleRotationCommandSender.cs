using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class ToggleRotationCommandSender : MonoBehaviour
    {
#pragma warning disable 649
        [Require] private SpinnerRotationReader reader;
        [Require] private SpinnerRotationCommandSender requestSender;

        [Require] private EntityId ownEntityId;
#pragma warning restore 649

        private void Update()
        {
            if (reader.Authority != Authority.NotAuthoritative)
            {
                // Perform sending logic only on non-authoritative workers.
                return;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                var request = new SpinnerRotation.SpinnerToggleRotation.Request(ownEntityId, new Empty());
                requestSender.SendSpinnerToggleRotationCommand(request);
            }
        }
    }
}

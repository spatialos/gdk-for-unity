using Improbable;
using Improbable.Common;
using Improbable.Gdk.GameObjectRepresentation;
using UnityEngine;

namespace Playground
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class ValidateShotBehaviour : MonoBehaviour
    {
        [Require] private ShootBullet.Requirable.CommandRequestHandler commandHandler;
        [Require] private Health.Requirable.CommandRequestSender healthCommandRequestSender;


        private void OnEnable()
        {
            commandHandler.OnShootRequest += OnShootRequest;
        }

        private void OnShootRequest(ShootBullet.Shoot.RequestResponder requestResponder)
        {
            var position = requestResponder.Request.Payload.StartPoint.ToUnityVector();
            var raycastDirection = requestResponder.Request.Payload.Direction.ToUnityVector();
            if (Physics.Raycast(position, raycastDirection, out var hit, Mathf.Infinity))
            {
                Debug.Log($"Hit {hit.transform.gameObject.name}");
                var spatialOSComponent = hit.transform.gameObject.GetComponent<SpatialOSComponent>();
                if (spatialOSComponent != null)
                {
                    var payload = new TakeDamageRequest(requestResponder.Request.Payload.Damage);
                    healthCommandRequestSender.SendTakeDamageRequest(spatialOSComponent.SpatialEntityId, payload);
                }
                requestResponder.SendResponse(new Empty());
            }
        }
    }
}

using Improbable.Common;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker;
using UnityEngine;

namespace Playground
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class TakeDamageBehaviour : MonoBehaviour
    {
        [Require] private Health.Requirable.CommandRequestHandler healthCommandRequestHandler;
        [Require] private Health.Requirable.Writer healthWriter;

        private EntityId entityId;

        private void OnEnable()
        {
            healthCommandRequestHandler.OnTakeDamageRequest += OnTakeDamageRequest;
            entityId = GetComponent<SpatialOSComponent>().SpatialEntityId;
        }

        private void OnTakeDamageRequest(Health.TakeDamage.RequestResponder requestResponder)
        {
            var newHealth = healthWriter.Data.Health - requestResponder.Request.Payload.Damage;
            if (newHealth < 0)
            {
                transform.position = Vector3.zero;
                newHealth = 100;
            }
            healthWriter.Send(new Health.Update
            {
                Health = newHealth,
            });
            requestResponder.SendResponse(new Empty());
        }
    }
}

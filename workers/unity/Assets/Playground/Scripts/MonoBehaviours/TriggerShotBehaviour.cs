using Improbable;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker;
using UnityEngine;

namespace Playground
{
    [WorkerType(WorkerUtils.UnityClient, WorkerUtils.AndroidClient, WorkerUtils.iOSClient)]
    public class TriggerShotBehaviour : MonoBehaviour
    {
        [Require] private ShootBullet.Requirable.CommandRequestSender commandSender;

        public int Damage = 10;

        private EntityId entityId;

        private void OnEnable()
        {
            entityId = GetComponent<SpatialOSComponent>().SpatialEntityId;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                
                var direction = transform.TransformDirection(Vector3.forward);
                var payload = new ShootRequest
                {
                    Damage = Damage,
                    Direction = direction.ToSpatialVector3f(),
                    StartPoint = transform.position.ToSpatialVector3f(), 
                };
                commandSender.SendShootRequest(entityId, payload);
            }
        }
    }
}

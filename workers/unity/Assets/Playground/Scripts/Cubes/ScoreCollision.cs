using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public class ScoreCollision : MonoBehaviour
    {
        private EntityManager entityManager;
        // private SpatialOSComponent component;

        private void Start()
        {
            // component = GetComponent<SpatialOSComponent>();
            // entityManager = component.World.GetExistingManager<EntityManager>();
        }

        public void OnCollisionEnter(Collision col)
        {
            return;
            // if (!col.gameObject || !col.gameObject.CompareTag("Cube"))
            // {
            //     return;
            // }
            //
            // var otherComponent = col.gameObject.GetComponentInParent<SpatialOSComponent>();
            // if (entityManager.HasComponent<Launchable.Component>(component.Entity)
            //     && !entityManager.HasComponent<CollisionComponent>(component.Entity)
            //     && otherComponent)
            // {
            //     entityManager.AddComponentData(component.Entity, new CollisionComponent(otherComponent.Entity));
            // }
        }
    }
}

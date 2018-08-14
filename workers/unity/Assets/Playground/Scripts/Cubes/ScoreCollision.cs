using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

public class ScoreCollision : MonoBehaviour
{
    private EntityManager entityManager;
    private SpatialOSComponent component;

    public void OnCollisionEnter(Collision col)
    {
        if (entityManager == null)
        {
            component = GetComponent<SpatialOSComponent>();
            if (!component)
            {
                Debug.LogError("Could not get SpatialOSComponent.");
                return;
            }

            entityManager = component.World.GetExistingManager<EntityManager>();
        }

        if (!col.gameObject || !col.gameObject.CompareTag("Cube"))
        {
            return;
        }

        var otherComponent = col.gameObject.GetComponent<SpatialOSComponent>();
        if (entityManager.HasComponent(component.Entity, typeof(SpatialOSLaunchable))
            && !entityManager.HasComponent(component.Entity, typeof(Playground.CollisionComponent))
            && otherComponent)
        {
            entityManager.AddComponentData(component.Entity, new Playground.CollisionComponent
            {
                OwnEntity = component.Entity,
                OtherEntity = otherComponent.Entity
            });
        }
    }
}

using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

public class ScoreCollision : MonoBehaviour
{
    private EntityManager entityManager;

    public void OnCollisionEnter(Collision col)
    {
        var component = GetComponent<SpatialOSComponent>();
        if (!component)
        {
            Debug.LogError("Could not get SpatialOSComponent.");
            return;
        }
        if (entityManager == null)
        {
            entityManager = component.World.GetExistingManager<EntityManager>();
        }
        if (col.gameObject && col.gameObject.tag == "Cube")
        {
            if (entityManager.HasComponent(component.Entity, typeof(SpatialOSLaunchable))
                && !entityManager.HasComponent(component.Entity, typeof(Playground.CollisionComponent)))
            {
                entityManager.AddComponent(component.Entity, typeof(Playground.CollisionComponent));
                entityManager.SetComponentData(component.Entity, new Playground.CollisionComponent
                {
                    ownEntity = component.Entity,
                    otherEntity = col.gameObject.GetComponent<SpatialOSComponent>().Entity,
                });
            }
        }
    }
}

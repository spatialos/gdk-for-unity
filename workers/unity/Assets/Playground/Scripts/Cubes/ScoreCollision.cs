using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

public class ScoreCollision : MonoBehaviour
{
    private MutableView view;
    private EntityManager entityManager;

    public void Start()
    {
        var component = GetComponent<SpatialOSComponent>();
        if (component)
        {
            view = WorkerRegistry.GetWorkerForWorld(component.World).View;
            entityManager = component.World.GetExistingManager<EntityManager>();
        }
        else
        {
            Debug.LogError("Could not retrieve SpatialOSComponent from game object.");
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if (entityManager == null)
        {
            return;
        }

        if (col.gameObject && col.gameObject.tag == "Cube")
        {
            var component = GetComponent<SpatialOSComponent>();
            if (component != null && view.HasComponent(component.Entity, typeof(SpatialOSLaunchable))
                && !view.HasComponent(component.Entity, typeof(Playground.CollisionComponent)))
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

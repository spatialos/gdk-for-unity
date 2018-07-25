using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

[RemoveAtEndOfTick]
public struct CollisionComponent : IComponentData
{
    public SpatialOSLaunchable own;
    public SpatialOSLaunchable other;
}

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
        Debug.Log(WorkerRegistry.GetWorkerForWorld(GetComponent<SpatialOSComponent>().World).WorkerId);
        if (entityManager == null)
        {
            return;
        }

        if (col.gameObject && col.gameObject.tag == "Cube")
        {
            var component = col.gameObject.GetComponent<SpatialOSComponent>();
            if (component != null && view.HasComponent(component.Entity, typeof(SpatialOSLaunchable)))
            {
                SpatialOSLaunchable otherEntity = view.GetComponent<SpatialOSLaunchable>(component.Entity);
                Entity collisionEventEntity = entityManager.CreateEntity(typeof(SpatialOSLaunchable),
                    typeof(CollisionComponent),
                    typeof(CommandRequestSender<SpatialOSLauncher>));
                entityManager.SetComponentData(collisionEventEntity, otherEntity);
                entityManager.SetComponentData(collisionEventEntity, new CollisionComponent
                {
                    own = view.GetComponent<SpatialOSLaunchable>(component.Entity),
                    other = otherEntity
                });
                entityManager.SetComponentData(collisionEventEntity, new CommandRequestSender<SpatialOSLauncher>());
            }
        }
    }
}

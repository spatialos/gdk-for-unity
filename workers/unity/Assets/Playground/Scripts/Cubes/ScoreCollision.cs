using Improbable.Gdk.Core;
using UnityEngine;
using Unity.Entities;
using Generated.Playground;
using Unity.Mathematics;

[RemoveAtEndOfTick]
public struct CollisionComponent : IComponentData
{
    public SpatialOSLaunchable other;
}

public class ScoreCollision : MonoBehaviour
{
    private MutableView view;
    private SpatialOSLaunchable ownLaunchable;

    public void Start()
    {
        var component = GetComponent<SpatialOSComponent>();
        if (component)
        {
            view = WorkerRegistry.GetWorkerForWorld(component.World).View;
            ownLaunchable = view.GetComponent<SpatialOSLaunchable>(component.Entity);
        }
        else
        {
            Debug.LogError("Could not retrieve SpatialOSComponent from game object.");
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject && col.gameObject.tag == "Cube")
        {
            var component = col.gameObject.GetComponent<SpatialOSComponent>();
            if (component != null && view.HasComponent(component.Entity, typeof(SpatialOSLaunchable)))
            {
                SpatialOSLaunchable otherEntity = view.GetComponent<SpatialOSLaunchable>(component.Entity);
                var entityManager = World.Active.GetExistingManager<EntityManager>();
                Entity collisionEventEntity = entityManager.CreateEntity(typeof(SpatialOSLaunchable), typeof(CollisionComponent),
                    typeof(CommandRequestSender<SpatialOSLauncher>));
                entityManager.SetComponentData(collisionEventEntity, otherEntity);
                entityManager.SetComponentData(collisionEventEntity, new CollisionComponent
                {
                    other = otherEntity
                });
                entityManager.SetComponentData(collisionEventEntity, new CommandRequestSender<SpatialOSLauncher>());
            }
        }
    }
}

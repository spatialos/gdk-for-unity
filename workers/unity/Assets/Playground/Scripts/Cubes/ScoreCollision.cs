using Generated.Playground;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Unity.Entities;
using UnityEngine;

public class ScoreCollision : MonoBehaviour
{
    private EntityManager entityManager;
    private SpatialOSComponent component;

    private void Start()
    {
        component = GetComponent<SpatialOSComponent>();
        entityManager = component.World.GetExistingManager<EntityManager>();
    }

    public void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject || !col.gameObject.CompareTag("Cube"))
        {
            return;
        }

        var otherComponent = col.gameObject.GetComponentInParent<SpatialOSComponent>();
        if (entityManager.HasComponent<SpatialOSLaunchable>(component.Entity)
            && !entityManager.HasComponent<Playground.CollisionComponent>(component.Entity)
            && otherComponent)
        {
            entityManager.AddComponentData(component.Entity, new Playground.CollisionComponent(otherComponent.Entity));
        }
    }
}

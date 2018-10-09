[//]: # (Doc of docs reference 22)

# SpatialOS entities: Creating entity templates
_This document relates to both GameObject-MonoBehaviour and ECS workflows._

Before reading this document, make sure you are familiar with the documentation on the [GameObject-MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spos-entities) and the [workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk) overview.

Whether you are using the GameObject and MonoBehaviour workflow or the ECS workflow, you need to set up entity templates to create a [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity). You use the `EntityTemplate` class to specify all the [components]({{urlRoot}}/content/glossary#spatialos-component) that a SpatialOS entity has, the initial values of those components, and which workers have [write access]({{urlRoot}}/content//glossary#authority) to each component.

For information on how to create SpatialOS entities once you have set up entity templates, see the documentation on [creating entities (GameObject and MonoBehaviour workflow)]({{urlRoot}}/content/gameobject/create-delete-spos-entries) and [ECS World commands (ECS workflow)]({{urlRoot}}/content/ecs/ecs-world-commands).

## How to create a SpatialOS entity template

You have to create an `EntityTemplate` to specify which components a [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity) has and the initial values of those [components]({{urlRoot}}/content/glossary#spatialos-component). You have to also specify which workers have write access (also known as ”[authority]({{urlRoot}}/content/glossary#authority)” on a per-component basis.

You use the [`EntityBuilder` class]({{urlRoot}}/content/api-entity-builder) to do this.

There are examples of how to use this class below.


### Create `ComponentData`
For each [schema component]({{urlRoot}}/content/glossary#schema) you define, the class created by the [code generator]({{urlRoot}}/content/code-generator) has a method to create a `ComponentData` object for that component. For example, for the following schema:

```
component Health {
  id = 1;
  int32 current_health = 1;
}
```

The code generator creates a static method `Health.Component.CreateSchemaComponentData(int currentHealth)` which instantiates and populates a `ComponentData` object.

The following code snippet shows an example of how to define an `EntityTemplate` using the `EntityBuilder` class. You can use this `EntityTemplate` to spawn a SpatialOS `creature` entity via either the [GameObject-MonoBehaviour world commands]({{urlRoot}}/content/gameobject/gomb-world-commands) or [ECS world commands]({{urlRoot}}/content/ecs/ecs-world-commands), depending on your [workflow]({{urlRoot}}/content/intro-workflows-spos-entities).


**Note**: You need to create a new EntityTemplate for each call to `CreateEntity`.

**Example**<br/>
Example defining a template for a SpatialOS entity `creature`.

```csharp
public static class CreatureTemplate
{
    public static readonly List<string> AllWorkerAttributes =
        new List<string>
        {
            "UnityClient",
            "UnityGameLogic"
        };

    public static EntityTemplate CreateCreatureEntityTemplate(Coordinates coords)
    {
        var healthComponent = Health.Component.CreateSchemaComponentData(currentHealth: 100);

        var entity = EntityBuilder.Begin()
            .AddPosition(coords.X, coords.Y, coords.Z, "UnityGameLogic")
            .AddMetadata("Creature", "UnityGameLogic")
            .SetPersistence(true)
            .SetReadAcl(AllWorkerAttributes)
            .AddComponent(healthComponent, "UnityGameLogic")
            .Build();

        return entityTemplate;
    }
}
```

## Feature Module components

To take advantage of some Feature Modules, you need to add their SpatialOS components to your SpatialOS entity. You do this by calling the appropriate extension methods on the `EntityBuilder` for each Feature Module that requires additional components. This document lists the helper methods and how to use them.

### Transform synchronization module

To add the SpatialOS components required for the transform synchronization Feature Module to work on a SpatialOS entity, use the `AddTransformSynchronizationComponents` extension method. This Feature Module requires the [worker attribute]({{urlRoot}}/content/glossary#worker-attribute) of the [worker]({{urlRoot}}/content/glossary#worker) you are giving [write-access]({{urlRoot}}/content/glossary#authority) over SpatialOS transforms to.

You have the option to set a starting `location` of type `Vector3`, `velocity` of type `Vector3`, and `rotation` of type `Quaternion`. If you do not provide these, location and velocity default to `Vector3.zero`, and rotation defaults to `Quaternion.identity`.

The following code snippet shows how to use the `AddTransformSynchronizationComponents` extension method with an optional `location` parameter.

**Example**<br/>
```csharp
public static class CubeTemplate
{
    public static EntityTemplate CreateCubeEntityTemplate(Coordinates coords)
    {
        var entityBuilder = EntityBuilder.Begin()
            .AddPosition(coords.X, coords.Y, coords.Z, "UnityGameLogic")
            ...
            ...
            .AddTransformSynchronizationComponents("UnityGameLogic",
				location: coords.NarrowToUnityVector());

        return entityBuilder.Build();
    }
}
```

### Player lifecycle module

The `AddPlayerLifecycleComponents` extension method for this Feature Module adds the `PlayerHeartbeatClient` and `PlayerHeartbeatServer` components to an entity. It requires the [worker attributes]({{urlRoot}}/content/glossary#worker-attribute) of the client, `clientAttribute`, and the server, `serverAttribute`.

The following code snippet shows how to pass  [worker attributes]({{urlRoot}}/content/glossary#worker-attribute) into the `AddPlayerLifecycleComponents` extension method. The `workerId` is the ID of the worker that sent a player creation request, and `clientAttributeSet` is the full list of attributes of this worker. Both fields are populated by the `HandleCreatePlayerRequestSystem.cs` system within the player lifecycle module.

**Example**<br/>
```csharp
public static class PlayerTemplate
{
    public static EntityTemplate CreatePlayerEntityTemplate(string workerId, List<string> clientAttributeSet, Improbable.Vector3f position)
    {
        // Obtain unique client attribute
        var clientAttribute = clientAttributeSet.First(attribute => attribute != "UnityClient");

        var entityBuilder = EntityBuilder.Begin()
            .AddPosition(position.X, position.Y, position.Z, "UnityGameLogic")
            ...
            ...
            .AddPlayerLifecycleComponents(clientAttribute, "UnityGameLogic");

        return entityBuilder.Build();
    }
}
```

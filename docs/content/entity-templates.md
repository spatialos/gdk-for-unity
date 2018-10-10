[//]: # (Doc of docs reference 22)

# SpatialOS entities: Creating entity templates
_This document relates to both [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

Before reading this document, make sure you are familiar with the documentation on the [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities) and the [workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk) overview.

Whether you are using the MonoBehaviour workflow or the ECS workflow, you need to set up entity templates to create a [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity). You use the `EntityTemplate` class to specify all the [components]({{urlRoot}}/content/glossary#spatialos-component) that a SpatialOS entity has, the initial values of those components, and which workers have [write access]({{urlRoot}}/content//glossary#authority) to each component.

For information on how to create SpatialOS entities once you have set up entity templates, see the documentation on [creating entities (MonoBehaviour workflow)]({{urlRoot}}/content/gameobject/create-delete-spatialos-entities) and [ECS World commands (ECS workflow)]({{urlRoot}}/content/ecs/world-commands).

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

The following code snippet shows an example of how to define an `EntityTemplate` using the `EntityBuilder` class. You can use this `EntityTemplate` to spawn a SpatialOS `creature` entity via either the [MonoBehaviour world commands]({{urlRoot}}/content/gameobject/world-commands) or [ECS world commands]({{urlRoot}}/content/ecs/world-commands), depending on your [workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities).


> You need to create a new EntityTemplate for each call to `CreateEntity`.

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

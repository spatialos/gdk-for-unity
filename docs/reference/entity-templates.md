[//]: # (Doc of docs reference 22)

<%(TOC)%>
# Entity templates
_This document relates to both [MonoBehaviour and ECS workflows](\{\{urlRoot\}\}/reference/intro-workflows-spatialos-entities)._

Before reading this document, make sure you are familiar with the documentation on the [MonoBehaviour and ECS workflows](\{\{urlRoot\}\}/reference/intro-workflows-spatialos-entities) and the [workers in the GDK](\{\{urlRoot\}\}/reference/workers/workers-in-the-gdk) overview.

Whether you are using the MonoBehaviour workflow or the ECS workflow, you need to set up entity templates to create a [SpatialOS entity](\{\{urlRoot\}\}/reference/glossary#spatialos-entity). You use the `EntityTemplate` class to specify all the [components](\{\{urlRoot\}\}/reference/glossary#spatialos-component) that a SpatialOS entity has, the initial values of those components, and which workers have [write access](\{\{urlRoot\}\}/reference//glossary#authority) to each component.

For information on how to create SpatialOS entities once you have set up entity templates, see the documentation on [creating entities (MonoBehaviour workflow)](\{\{urlRoot\}\}/reference/gameobject/create-delete-spatialos-entities) and [ECS World commands (ECS workflow)](\{\{urlRoot\}\}/reference/ecs/world-commands).

## How to create a SpatialOS entity template

You have to create an `EntityTemplate` to specify which components a [SpatialOS entity](\{\{urlRoot\}\}/reference/glossary#spatialos-entity) has and the initial values of those [components](\{\{urlRoot\}\}/reference/glossary#spatialos-component). You have to also specify which type of workers can have write access (also known as ”[authority](\{\{urlRoot\}\}/reference/glossary#authority)”) on a per-component basis.

You use the `EntityTemplate` class.

There are examples of how to use this class below.


### Create component snapshots
For each [schema component](\{\{urlRoot\}\}/reference/glossary#schema) you define, the `code generator` creates a struct which inherits from `ISpatialComponentSnapshot`, the generated [snapshot struct](\{\{urlRoot\}\}/reference/ecs/component-generation#overview). For example, for the following schema:

```
component Health {
  id = 1;
  int32 current_health = 1;
}
```

The code generator creates:

```csharp
public partial class Health
{
    public struct Snapshot
    {
        int CurrentHealth;
    }
}
```

The following code snippet shows an example of how to define an `EntityTemplate`. You can use this `EntityTemplate` to spawn a SpatialOS `creature` entity via either the [MonoBehaviour world commands](\{\{urlRoot\}\}/reference/gameobject/world-commands) or [ECS world commands](\{\{urlRoot\}\}/reference/ecs/world-commands), depending on your [workflow](\{\{urlRoot\}\}/reference/intro-workflows-spatialos-entities).

**Example**<br/>
Example defining a template for a SpatialOS entity `creature`.

```csharp
public static class CreatureTemplate
{

    public static EntityTemplate CreateCreatureEntityTemplate(Coordinates coords)
    {
        var entityTemplate = new EntityTemplate();

        entityTemplate.AddComponent(new Position.Snapshot { Coords = coords }, "UnityGameLogic");
        entityTemplate.AddComponent(new Metadata.Snapshot { EntityType = "Creature"}, "UnityGameLogic");
        entityTemplate.AddComponent(new Persistence.Snapshot(), "UnityGameLogic");
        entityTemplate.AddComponent(new Health.Snapshot { CurrentHealth = 100 }, "UnityGameLogic");
        entityTemplate.SetReadAccess("UnityGameLogic", "UnityClient");
        entityTemplate.SetComponentWriteAccess(EntityAcl.ComponentId, "UnityGameLogic");

        return entityTemplate;
    }
}
```

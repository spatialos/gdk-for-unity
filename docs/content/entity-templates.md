**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

------

## Creating entity templates

To create an [entity](https://docs.improbable.io/reference/latest/shared/glossary#entity) in the SpatialOS GDK for Unity, you need to create a template which specifies which components should be added to the entity and allows you to specify [authority](ecs/authority.md) on a per-[component](https://docs.improbable.io/reference/latest/shared/glossary#component) basis.

Create the entity template using the `EntityBuilder` which is part of the `Improbable.Gdk.Core` assembly. The `EntityBuilder` class has the following public methods:

| Public method                                                | Description                                                  |
| :----------------------------------------------------------- | :----------------------------------------------------------- |
| `Begin()`                                                    | Create a new `EntityBuilder` instance for creating an entity template. |
| `AddPosition(double x, double y, double z, string writeAccess)` | Add a [`Position`](https://docs.improbable.io/reference/latest/shared/schema/standard-schema-library#position-required) component to your entity and specify which worker type(s) can have authority over it. |
| `SetPersistence(bool persistence)`                           | Specify whether your entity should be saved in snapshots. For more information, see the documentation on [persistence](https://docs.improbable.io/reference/latest/shared/glossary#persistence). |
| `SetReadAcl(string attribute, param string[] attribute)` or `SetReadAcl(List<string> attribute)` | Specify which worker type(s) can have [read access](https://docs.improbable.io/reference/latest/shared/glossary#read-and-write-access-authority) to the entity. |
| `SetEntityAclComponentWriteAccess(string attribute)`         | Specify which worker type can have authority over the [`EntityAcl`](https://docs.improbable.io/reference/latest/shared/schema/standard-schema-library#entityacl-required) component of your entity. This is useful if, while the game is running, you want to change which worker type(s) can have authority over the entity's other components. |
| `AddComponent(ComponentData data, string writeAccess)`       | Add a user-defined component to your entity and specify which worker type(s) can have authority over it. |
| `Build()`                                                    | Create a finished entity template.                           |

For each [schema component](https://docs.improbable.io/reference/13.2/shared/glossary#schema) you define, [the code generator](ecs/code-generator.md) generates a helper method to create a `ComponentData` object for that component. For example, for the following schema:

```
component Health {
  id = 1;
  int32 health_value = 1;
}
```

There is a method `Health.Component.CreateSchemaComponentData(int healthValue)` which will instantiate and populate a `ComponentData` object.

Below is an example of how to define an entity template using the `EntityBuilder`. In this example we will be creating a template for a creature:

```csharp
public static class CreatureTemplate
{
    public static Entity CreateCreatureEntityTemplate(Coordinates coords)
    {
        var healthComponent = Health.Component.CreateSchemaComponentData(healthValue: 100);

        var entity = EntityBuilder.Begin()
            .AddPosition(coords.X, coords.Y, coords.Z, WorkerUtils.UnityGameLogic)
            .AddMetadata("Creature", WorkerUtils.UnityGameLogic)
            .SetPersistence(true)
            .SetReadAcl(WorkerUtils.AllWorkerAttributes)
            .AddComponent(healthComponent, WorkerUtils.UnityGameLogic)
            .Build();

        return entity;
    }
}
```

You can use this entity template to spawn a Creature entity. This can be done using the [MonoBehaviour](gameobject/world-commands.md) or [ECS](ecs/world-commands.md) workflow.

------

**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).

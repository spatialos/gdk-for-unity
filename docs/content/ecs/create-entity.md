**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----


## ECS: Creating entities

To create an [entity](https://docs.improbable.io/reference/latest/shared/glossary#entity) in SpatialOS, you need to:
1. Create an entity definition using the `EntityBuilder`
2. Send a `CreateEntity` world command

### 1. Create an entity definition using the `EntityBuilder`

The `EntityBuilder` class is part of the `Improbable.Gdk.Core` assembly. It's a convenient way to create entity definitions and allows you to specify [authority](authority.md) on a per-[component](https://docs.improbable.io/reference/latest/shared/glossary#component) basis.

The `EntityBuilder` class has the following public methods:

| Public method  | Description |
| :------------- | :------------- |
| `Begin()`  | Create a new `EntityBuilder` instance for creating an entity definition. |
| `AddPosition(double x, double y, double z, string writeAccess)`  | Add a [`Position`](https://docs.improbable.io/reference/latest/shared/schema/standard-schema-library#position-required) component to your entity and specify which worker type(s) can have authority over it. |
| `SetPersistence(bool persistence)` | Specify whether your entity should be saved in snapshots. For more information, see the documentation on [persistence](https://docs.improbable.io/reference/latest/shared/glossary#persistence). |
| `SetReadAcl(string attribute, param string[] attribute)` or `SetReadAcl(List<string> attribute)` | Specify which worker type(s) can have [read access](https://docs.improbable.io/reference/latest/shared/glossary#read-and-write-access-authority) to the entity. |
| `SetEntityAclComponentWriteAccess(string attribute)` | Specify which worker type can have authority over the [`EntityAcl`](https://docs.improbable.io/reference/latest/shared/schema/standard-schema-library#entityacl-required) component of your entity. This is useful if, while the game is running, you want to change which worker type(s) can have authority over the entity's other components. |
| `AddComponent(ComponentData data, string writeAccess)` | Add a user-defined component to your entity and specify which worker type(s) can have authority over it. |
| `Build()` | Create a finished entity definition. |

For each schema component you define, [the code generator](./code-generator.md) generates a helper method to create a `ComponentData` object for that component. For example, for the following schema:

```
component Example {
  id = 1;
  int32 value = 1;
}
```
There is a method `Example.CreateSchemaComponentData(int value)` which will instantiate and populate a `ComponentData` object.


Below is an example of how to create an entity definition using the `EntityBuilder`:
```csharp
private static readonly string GameLogicAttribute = UnityGameLogic.WorkerAttribute;

private static readonly List<string> AllWorkersAttributes = new List<string> { UnityGameLogic.WorkerAttribute, UnityClient.WorkerAttribute};

public Entity CreateCreatureEntityDefinition(Coordinates position)
{
    var healthComponent = Health.CreateSchemaComponentData(healthValue: 100);

    return EntityBuilder
        .Begin()
        .AddPositionComponent(position.x, position.y, position.z, GameLogicAttribute)
        .SetPersistence(true)
        .SetReadAcl(AllWorkersAttributes)
        .AddMetadata("Creature", GameLogicAttribute)
        .AddComponent(healthComponent, GameLogicAttribute)
        .Build();
}
```

### 2. Send a `CreateEntity` world command

Below is an example of how to send a `CreateEntity` world command. For more information, see [World commands](commands.md#world-commands).

```csharp
public class CreateEntitySystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public ComponentDataArray<Foo> Foo;
        public ComponentDataArray<WorldCommands.CreateEntity.CommandSender> CreateEntitySender;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var entity = EntityBuilder.Begin()
                ...
                ...
                .Build();

            var request = new WorldCommands.CreateEntity.Request
            {
                Entity = entity
            };

            data.CreateEntitySender[i].Add(request);
        }
    }
}
```

----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../../README.md#give-us-feedback).

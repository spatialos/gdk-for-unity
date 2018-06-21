**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----


## Creating entities

To create an [entity](https://docs.improbable.io/reference/13.0/shared/glossary#entity) in SpatialOS, you need to:
1. Create an entity definition using the `EntityBuilder`
2. Send a `CreateEntity` world command

### 1. Create an entity definition using the `EntityBuilder`

The `EntityBuilder` class is part of the `Improbable.Gdk.Legacy` assembly. It's a convenient way to create entity definitions, and allows you to specify [authority](authority.md) on a per-[component](https://docs.improbable.io/reference/13.0/shared/glossary#component) basis.

The `EntityBuilder` class has the following public methods:

| Public method  | Description |
| :------------- | :------------- |
| `Begin()`  | Create a new `EntityBuilder` instance for creating an entity definition. |
| `AddPositionComponent(Coordinates position, WorkerRequirementSet writeAcl)`  | Add a [`Position`](https://docs.improbable.io/reference/13.0/shared/schema/standard-schema-library#position-required) component to your entity and specify which worker type(s) can have authority over it. |
| `SetPersistence(bool persistence)` | Specify whether your entity should be saved in snapshots. For more information, see the documentation on [persistence](https://docs.improbable.io/reference/13.0/shared/glossary#persistence). |
| `SetReadAcl(WorkerRequirementSet readAcl)` | Specify which worker type(s) can have [read access](https://docs.improbable.io/reference/13.0/shared/glossary#read-and-write-access-authority) to the entity. |
| `SetEntityAclComponentWriteAccess(WorkerRequirementSet writeAcl)` | Specify which worker type(s) can have authority over the [`EntityAcl`](https://docs.improbable.io/reference/13.0/shared/schema/standard-schema-library#entityacl-required) component of your entity. This is useful if, while the game is running, you want to change which worker type(s) can have authority over the entity's other components. |
| `AddComponent<T>(IComponentData<T> data, WorkerRequirementSet writeAcl)` | Add a user-defined component `T` to your entity and specify which worker type(s) can have authority over it. |
| `Build()` | Create a finished entity definition. |

Below is an example of how to create an entity definition using the `EntityBuilder`:
```csharp
private static readonly WorkerRequirementSet GameLogicSet =
            WorkerRegistry.GetWorkerRequirementSet(typeof(UnityGameLogic));

private static readonly WorkerRequirementSet AllWorkersSet =
            WorkerRegistry.GetWorkerRequirementSet(typeof(UnityClient), typeof(UnityGameLogic));

public Entity CreateCreatureEntityDefinition(Coordinates position)
{
    return EntityBuilder
        .Begin()
        .AddPositionComponent(position, GameLogicSet)
        .SetPersistence(true)
        .SetReadAcl(AllWorkersSet)
        .AddComponent(new Metadata.Data(entityType : "Creature"), GameLogicSet)
        .AddComponent(new Health.Data(healthValue : 100), GameLogicSet)
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
        public int Length;
        public ComponentDataArray<Foo> Foo;
        public ComponentDataArray<SystemCommandSender> SystemCommandSender;
    }

    [Inject] Data data;

    protected void OnUpdate()
    {
        for(var i = 0; i < data.Length; i++)
        {
            var entity = EntityBuilder
                .Begin()
                ...
                ...
                .Build();

            data.SystemCommandSender[i].SendCreateEntityRequest(entity);
        }
    }
}
```

----
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).
<%(TOC)%>

# ECS: Component data

<%(Callout message="
Before reading this document, make sure you are familiar with

  * [ECS component generation]({{urlRoot}}/reference/concepts/code-generation)
  * [ECS system update order]({{urlRoot}}/workflows/ecs/concepts/system-update-order)
")%>

When a SpatialOS entity is [checked out]({{urlRoot}}/reference/glossary#checking-out), its components are added to the corresponding ECS entity as part of the entity's check out process.

### How to read component data

The components of an ECS entity contain the latest data that the worker has received of a corresponding SpatialOS entity.

To read an ECS component's data, construct an ECS EntityQuery with a `ReadOnly<T>` constraint on the component you wish to read. You can then iterate through the matching components using the ECS `.ForEach` syntax.

```csharp
public class ReadExampleComponentSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreate()
    {
        base.OnCreate();

        query = GetEntityQuery(
            ComponentType.ReadOnly<Example.Component>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach((ref Example.Component exampleComponent) =>
        {
            // Read every Example component in the worker's World.
            var someField = exampleComponent.SomeField;
        });
    }
}
```

### How to send a component update

To send a component update, change the value of at least one field of the component. The component tracks these changes, and the GDK constructs a component update to send at the end of the current update loop.

> **Note:** This means that you should be conscious of when you make changes to SpatialOS components as this directly correlates to network bandwidth!

```csharp
public class SendExampleUpdateSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreate()
    {
        base.OnCreate();

        query = GetEntityQuery(
            ComponentType.ReadWrite<Example.Component>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach((ref Example.Component exampleComponent) =>
        {
            exampleComponent.SomeField = 10;
        });
    }
}
```

#### Component update edge case

As a result of how component changes are tracked and [C# reference type](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) semantics, there is one edge case that you need to be careful about.

Simply adding or removing elements to a `List<T>` or a `Dictionary<K, V>` field in a component is not sufficient to trigger a component update. Instead, you need to **set the field back to itself to trigger the update**, as shown in the example below. This applies to `List<T>` and `Dictionary<K, V>` fields within types as well.

The following is the correct way to trigger an automatic update for a `List<T>` or a `Dictionary<K, V>`.

```csharp
public class ExampleSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreate()
    {
        base.OnCreate();

        query = GetEntityQuery(
            ComponentType.ReadWrite<Example.Component>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach((ref Example.Component exampleComponent) =>
        {
            // Mutate the list or dictionary field.
            // This is not sufficient to trigger an update.
            exampleComponent.SomeInts.Add(10);
            // Setting the field back to itself marks it as dirty.
            exampleComponent.SomeInts = exampleComponent.SomeInts;

            // Mutate the nested list or dictionary field.
            // This is not sufficient to trigger an update.
            exampleComponent.SomeTypeField.SomeNestedFloats.Add(10);
            // Setting the field back to itself marks it as dirty.
            exampleComponent.SomeTypeField = exampleComponent.SomeTypeField;
        });
    }
}
```

## How to react to a component update

The `GetComponentUpdatesReceived<T>` method on the [`ComponentUpdateSystem`]({{urlRoot}}/api/core/component-update-system) allows you to retrieve a list of all the component updates, given the type of the update `T`, that have been received since the previous frame.

The example below shows how to use this method to handle component updates a worker receives.

```csharp
public class ProcessChangedHealthSystem : ComponentSystem
{
    private ComponentUpdateSystem updateSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();
    }

    protected override void OnUpdate()
    {
        var exampleUpdates = updateSystem.GetComponentUpdatesReceived<Example.Update>();

        for (var i = 0; i < exampleUpdates.Count; i++)
        {
            var exampleUpdate = exampleUpdates[i];

            // Process a received update.
            Debug.Log($"Received update for entity ID {exampleUpdate.EntityId}");
        }
    }
}
```

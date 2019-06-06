<%(TOC)%>

# ECS: Component updates

<%(Callout message="
Before reading this document, make sure you are familiar with

  * [ECS component generation]({{urlRoot}}/reference/concepts/code-generation)
  * [ECS system update order]({{urlRoot}}/reference/workflows/ecs/system-update-order)
")%>

When a SpatialOS entity is [checked out]({{urlRoot}}/reference/glossary#checking-out), its components are added to the corresponding ECS entity as part of the entity's check out process.

### How to send a component update

To send a component update, change the value of at least one field of the component. A component update is constructed and sent at the end of the current update loop.

```csharp
public class SendExampleUpdateSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        query = GetEntityQuery(
            ComponentType.ReadWrite<Example.Component>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach(
            (ref Example.Component exampleComponent) =>
            {
                exampleComponent.Value = 10;
            });
    }
}
```

#### Component update edge case

There is one edge case that you need to be careful about: this is a result of how component changes are tracked and [C# reference type](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) semantics.

Simply adding or removing elements to a `List<T>` or a `Dictionary<K, V>` field in a component is not sufficient to trigger a component update. Instead, you need to set the field back to itself to trigger the update, as shown in the example below. This applies to `List<T>` and `Dictionary<K, V>` fields within types as well.

Given the following schema:

```schemalang
package improbable.examples;

type Foo {
    list<float> some_floats = 1;
}

component Bar {
    id = 10001;
    list<int32> some_ints = 1;
    Foo some_foo = 2;
}
```

The following is the correct way to trigger an automatic update for a `List<T>` or a `Dictionary<K, V>`.

```csharp
public class ExampleSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        query = GetEntityQuery(
            ComponentType.ReadWrite<Bar.Component>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(query).ForEach(
            (ref Bar.Component barComponent) =>
            {
                // Mutate the list or dictionary field.
                // This is not sufficient to trigger an update.
                barComponent.SomeInts.Add(10);
                // Setting the field back to itself marks it as dirty.
                barComponent.SomeInts = barComponent.SomeInts;

                // Mutate the nest list or dictionary field.
                // This is not sufficient to trigger an update.
                barComponent.SomeFoo.SomeFloats.Add(10);
                // Setting the field back to itself marks it as dirty.
                barComponent.SomeFoo = barComponent.SomeFoo;
            });
    }
}
```

### How to react to a component update

When a component update is received this will be added as a [reactive component]({{urlRoot}}/reference/workflows/ecs/interaction/reactive-components/overview).
To access all component updates for the `Health` component that have happened since the last frame, access `Health.ReceivedUpdates.Updates`.
If you only want the latest values, you can access the `Health.Component` directly.

```csharp
public class ProcessChangedHealthSystem : ComponentSystem
{
    private EntityQuery query;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        // Set up a query for Example components that have received updates.
        query = GetEntityQuery(
            ComponentType.ReadOnly<Example.ReceivedUpdates>()
        );
    }

    protected override void OnUpdate()
    {
        // Iterate through components that have received updates.
        Entities.With(query).ForEach(
            (ref Example.ReceivedUpdates exampleUpdates) =>
            {
                foreach (var update in exampleUpdates.Updates)
                {
                    // Process received updates.
                }
            });
    }
}
```

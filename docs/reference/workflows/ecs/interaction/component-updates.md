[//]: # (Doc of docs reference 33)
[//]: # (TODO - Tech writer review)

<%(TOC)%>
#  ECS: Component updates
 _This document relates to the [ECS workflow]({{urlRoot}}/reference/workflows/which-workflow)._

Before reading this document, make sure you are familiar with

  * [ECS component generation]({{urlRoot}}/reference/concepts/code-generation)
  * [ECS system update order]({{urlRoot}}/reference/workflows/ecs/system-update-order)

The following schema file is used for the examples described below.

```
package improbable.examples;

component Health {
  id = 10000;
  int32 current_health = 1;
  int32 damage_taken = 2;
}
```

When a SpatialOS entity is [checked out]({{urlRoot}}/reference/glossary#checking-out), its components are added to the corresponding ECS entity as part of the entity's check out process.

### How to send a component update

To send a component update, change the value of at least one field of the component . A component update is constructed and sent at the end of the current update loop.

```csharp
public class SendHealthUpdateSystem : ComponentSystem
{
    private struct Data
    {
        public readonly int Length;
        public ComponentDataArray<Health.Component> HealthComponents;
    }

    [Inject] private Data data;

    protected void OnUpdate()
    {
        for (var i = 0; i < data.Length; ++i)
        {
            var exampleComponent = data.ExampleComponents[i];
            exampleComponent.Value = 10;
            data.ExampleComponents[i] = exampleComponent;
        }
    }
}
```

#### Component update edge case

There is one edge case that you need to be careful about: this is a result of how component changes are tracked and [C# reference type](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) semantics.

Simply adding or removing elements to a `List<T>` or a `Dictionary<K, V>` field in a component is not sufficient to trigger a component update. Instead, you need to set the field back to itself to trigger the update, as shown in the example below. This applies to `List<T>` and `Dictionary<K, V>` fields within types as well.

Given the following schema:

```
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
    private struct Data
    {
        public readonly int Length;
        public ComponentDataArray<Bar.Component> BarComponents;
    }

    [Inject] private Data data;

    protected void OnUpdate()
    {
        // For a top level list or dictionary field, this is how you trigger an automatic update.
        for (var i = 0; i < data.Length; ++i)
        {
            var bar = data.BarComponents[i];

            // Mutate the list. This is not sufficient to trigger an update.
            bar.SomeInts.Add(10);
            // Setting the field back to itself marks it as dirty.
            bar.SomeInts = bar.SomeInts;
            
            data.BarComponents[i] = bar;
        }

        // For a nested list or dictionary field, this is how you trigger an automatic update.
        for (var i = 0; i < data.Length; ++i)
        {
            var bar = data.BarComponents[i];

            // Mutate the list. This is not sufficient to trigger an update.
            bar.SomeFoo.SomeFloats.Add(10);
            // Setting the field back to itself marks it as dirty.
            bar.SomeFoo = bar.SomeFoo;
            
            data.BarComponents[i] = bar;
        }
    }
}
```

### How to react to a component update

When a component update is received this will be added as a [reactive component]({{urlRoot}}/reference/workflows/ecs/reactive-components).
To access all component updates for the `Health` component that have happened since the last frame, access `Health.ReceivedUpdates.Updates`.
If you only want the latest values, you can access the `Health.Component` directly.

```csharp
public class ProcessChangedHealthSystem : ComponentSystem
{
    private struct Data
    {
        public readonly int Length;
        // the component has already been updated to the latest values
        [ReadOnly] public ComponentDataArray<Health.Component> HealthComponents;
        // inject the ReceivedUpdates component to ensure this system only runs when the
        // component has changed
        [ReadOnly] public ComponentDataArray<Health.ReceivedUpdates> Updates;
    }

    [Inject] private Data data;

    protected void OnUpdate()
    {
        for (var i = 0; i < data.Length; ++i)
        {
            var updates = data.Updates[i].Updates;
            foreach (var update in updates)
            {
                // process received updates
            }
        }
    }
}
```

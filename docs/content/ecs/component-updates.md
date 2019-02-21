[//]: # (Doc of docs reference 33)
[//]: # (TODO - Tech writer review)

<%(TOC)%>
#  Component updates
 _This document relates to the [ECS workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

Before reading this document, make sure you are familiar with

  * [ECS component generation]({{urlRoot}}/content/ecs/component-generation)
  * [ECS system update order]({{urlRoot}}/content/ecs/system-update-order)

The following schema file is used for the examples described below.

```
package improbable.examples;

component Health {
  id = 10000;
  int32 current_health = 1;
  int32 damage_taken = 2;
}
```

When a SpatialOS entity is [checked out]({{urlRoot}}/content/glossary#checking-out), its components are automatically added to the corresponding ECS entity as part of the entity's check out process.

### How to send a component update
To send a component update, set the component to the value to be sent. A component update will be constructed and sent at the end of the current update loop.
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
        for(var i = 0; i < data.Length; ++i)
        {
            var exampleComponent = data.ExampleComponents[i];
            exampleComponent.Value = 10;
            data.ExampleComponents[i] = exampleComponent;
        }
    }
}
```

### How to react to a component update

When a component update is received this will be added as a [reactive component]({{urlRoot}}/content/ecs/reactive-components).
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

<%(TOC)%>

# ECS: Temporary components

A temporary component is a type of component which only exists for a single frame. This pattern is useful when you want to represent transient events as an ECS component, for example, the [`OnConnected`]({{urlRoot}}/api/core/on-connected) component which denotes that your worker has connected to the SpatialOS Runtime as described in the [ECS: Worker entity documentation]({{urlRoot}}/reference/workflows/ecs/worker-entity).

We provide you with the [`Improbable.Gdk.Core.RemoveAtEndOfTick`]({{urlRoot}}/api/core/remove-at-end-of-tick-attribute) attribute, which can be applied to any ECS component to make it a temporary component.

If a temporary component is added to an entity, the component will be removed at the end of the update loop. This means that when you use temporary components, you must consider the ordering of your systems carefully. Any system which runs logic predicated on the temporary component should be run after the temporary component is added and before it is removed.

The following code snippet shows an example of how to annotate your ECS component with the [`RemoveAtEndOfTick`]({{urlRoot}}/api/core/remove-at-end-of-tick-attribute) attribute.

```csharp
[RemoveAtEndOfTick]
public struct SomeTemporaryComponent : IComponentData
{
}
```
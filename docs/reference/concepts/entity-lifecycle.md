<%(TOC)%>

# Entity lifecycle

_This document relates to both [MonoBehaviour and ECS workflows]({{urlRoot}}/reference/workflows/overview)._

The [SpatialOS runtime]({{urlRoot}}/reference/glossary#spatialos-runtime) manages the lifecycle of [SpatialOS entities]({{urlRoot}}/reference/glossary#spatialos-entity) in your [worker’s view]({{urlRoot}}/reference/glossary#worker-s-view), or the part of the [game world]({{urlRoot}}/reference/glossary#spatialos-world) that your worker has access to. The SpatialOS GDK for Unity interacts with the SpatialOS runtime through [Operations](https://docs.improbable.io/reference/latest/shared/design/operations#operations-how-workers-communicate-with-spatialos) and integrates the lifecycle natively into Unity.

This means that interacting with the entity lifecycle outside of the provided APIs can cause runtime errors or undefined behaviour.

> Warning: Manually deleting entities locally will cause runtime errors. Use the [`DeleteEntity` world command]({{urlRoot}}/reference/workflows/ecs/interaction/commands/world-commands#delete-an-entity) instead.

## What happens when an entity enters your view

When an entity moves into your worker's [view]({{urlRoot}}/reference/glossary#worker-s-view), the SpatialOS runtime sends a set of operations to your worker describing the current state of that entity. For a single entity, your worker receives a set of messages describing the entity:

 - A message stating which entity has entered your view
 - A message stating the current state of each [SpatialOS component]({{urlRoot}}/reference/glossary#spatialos-component) on that entity that your worker has interest in
 - (Optionally) A message stating that your worker has been delegated [authority]({{urlRoot}}/reference/glossary#authority) over a SpatialOS component.

The SpatialOS GDK for Unity turns these messages into a single ECS Entity in a process described in the [Entity Contracts documentation]({{urlRoot}}/reference/workflows/ecs/entity-contracts). You can also optionally associate a GameObject with this entity as described in the [GameObject Creation feature module documentation]({{urlRoot}}/modules/game-object-creation/overview).

## What happens when an entity leaves your view

When an entity moves out of your worker's [checkout region](https://docs.improbable.io/reference/latest/shared/concepts/workers-load-balancing), the SpatialOS runtime sends a set of operations to your worker to represent that change. For a single entity, your worker receives a set of messages:

- (Optionally) A message stating that your worker has been undelegated authority over a SpatialOS component.
- A message stating that a SpatialOS component has been removed from your view for each SpatialOS component your worker has interest in.
- A message stating which entity has been removed from your view.

The SpatialOS GDK for Unity uses these messages to remove the ECS Entity and clean up any data associated with it. If you choose to associate a GameObject with this entity, you will receive a callback to clean up the GameObject.

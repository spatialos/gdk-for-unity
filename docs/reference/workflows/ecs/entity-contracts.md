<%(TOC)%>

# ECS entity contracts
  _This document relates to the [ECS workflow]({{urlRoot}}/reference/workflows/overview)._

Before reading this document, make sure you are familiar with:

  * [SpatialOS entities: update entity lifecycle]({{urlRoot}}/reference/concepts/entity-lifecycle)
  * [System update order]({{urlRoot}}/reference/workflows/ecs/system-update-order)
  * [Temporary components]({{urlRoot}}/reference/workflows/ecs/temporary-components)
  * [Reactive components]({{urlRoot}}/reference/workflows/ecs/reactive-components)
  * [Workers: Worker entity]({{urlRoot}}/reference/workflows/ecs/worker-entity)

This documentation describes the guarantees we provide for the components that an ECS Entity has.

## Temporary components
Temporary components are components that only exist for one frame. We guarantee that all temporary components are removed from all ECS Entities when `InternalSpatialOSCleanGroup` is run.

## Reactive components
A reactive component is implemented as a temporary component, and so has the same guarantees as a temporary component.

## Components on ECS Entities that represent SpatialOS entities

For each SpatialOS entity that a [worker]({{urlRoot}}/reference/concepts/worker) checks out, the [`EcsViewSystem`]({{urlRoot}}/api/core/ecs-view-system) creates an ECS Entity for that worker’s ECS world.
The following guarantees hold for any ECS Entity representing a SpatialOS entity:

  * Any SpatialOS entity that is in the worker's view is represented as an ECS Entity.
  * If a SpatialOS entity gets removed from the worker's view, the corresponding ECS Entity is deleted.
  * The [`NewlyAddedSpatialOSEntity`]({{urlRoot}}/api/core/newly-added-spatial-os-entity) component is a temporary component and added upon creating these entities.
  * The [`SpatialEntityId`]({{urlRoot}}/api/core/spatial-entity-id) component is always available on these entities.
  * The `{name of component}.Component` components are always available on these entities for all schema components that belong to these entities.
  * The `WorldCommands.{name of world command}.CommandSender` components are always available on these entities and contain the API to send [world commands]({{urlRoot}}/reference/workflows/ecs/interaction/commands/world-commands).
  * `{name of component}.CommandSenders.{name of command}` components are always available on these entities for every single command defined in your schema.
  * `{name of component}.CommandResponders.{name of command}` components are available on these entities for all components ands that the worker is authoritative over.
  * `{name of component}.EventSender.{name of event}` components are available on these entities for all components that the worker is authoritative over.
  * At least one authority component is available on these entities based on the worker's component write access
    * [`Authoritative<T>`]({{urlRoot}}/api/reactive-components/authoritative): If the worker has write access to the component.
    * [`NotAuthoritative<T>`]({{urlRoot}}/api/reactive-components/not-authoritative): If the worker only has read access to the component.
    * [`AuthorityLostImminent<T>`]({{urlRoot}}/api/reactive-components/authority-loss-imminent): If the worker is about to lose write access to the component. Note that the [`Authoritative<T>`]({{urlRoot}}/api/reactive-components/authoritative) component is still attached to these entities.

### Guarantees when receiving updates or messages
Whenever a component update or message is received by the worker, the following holds for the entity that the worker received it for:

  * All updates received are stored as a list in a reactive component called `{name of component}.ReceivedUpdates`.
  * The `{name of component}.Component` component is updated to the values stored in the latest update received
  * All events received are stored as a list in a reactive component called `{name of component}.ReceivedEvents.{name of event}`.
  * All command requests received are stored as a list in a reactive component called `{name of component}.{name of command}.CommandRequests`.
  * All command responses received are stored as a list in a reactive component called `{name of component}.{name of command}.CommandResponses`.

### Guarantees when sending updates or messages
Whenever a component update or message is sent by the worker, the following holds for the entity that the worker sends it from:

  * Whenever a field inside a `{name of component}.Component` changes, the component update will be sent the next time the [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.
  * All events added to the `{name of component}.EventSender.{name of event}`.Events list will be sent the next time the [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.
  * All command requests added to the `{name of component}.CommandSenders.{name of command}.RequestsToSend`  will be sent the next time the [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.
  * All command responses added to the `{name of component}.CommandResponders.{name of command}.ResponsesToSend`  will be sent the next time the [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.

## Components on worker entities
A worker entity is created for each ECS World that is associated with a [worker]({{urlRoot}}/reference/concepts/worker).
We guarantee the following:

  * The worker entity will exist for as long as the ECS World exists.
  * The [`WorkerEntityTag`]({{urlRoot}}/api/core/worker-entity-tag) component is always available on the worker entity.
  * The `WorldCommands.{name of world command}.CommandSender` components are always available on the worker entity and contain the API to send [world commands]({{urlRoot}}/reference/workflows/ecs/interaction/commands/world-commands).
  * `{name of component}.CommandSenders.{name of command}` components are always available on the worker entity for every single command defined in your schema.
  * `{name of component}.CommandResponders.{name of command}` components are always available on the worker entity for every single command defined in your schema.
  * The [`OnConnected`]({{urlRoot}}/api/core/on-connected) component is a temporary component added upon creating the worker entity.
  * The [`OnDisconnected`]({{urlRoot}}/api/core/on-disconnected) component is a temporary component added after the connection to the SpatialOS Runtime has been lost.

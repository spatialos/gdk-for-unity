[//]: # (Doc of docs reference 4)
[//]: # (TODO - Tech writer pass)

<%(TOC)%>
# ECS entity contracts
  _This document relates to the [ECS workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

Before reading this document, make sure you are familiar with:

  * [SpatialOS entities: update entity lifecycle]({{urlRoot}}/content/entity-lifecycle)
  * [System update order]({{urlRoot}}/content/ecs/system-update-order)
  * [Temporary components]({{urlRoot}}/content/ecs/temporary-components)
  * [Reactive components]({{urlRoot}}/content/ecs/reactive-components)
  * [Workers: Worker entity]({{urlRoot}}/content/workers/worker-entity)

This documentation describes the guarantees we provide for the components that an ECS Entity has.

## Temporary components
Temporary components are components that only exist for one frame. We guarantee that all temporary components are removed from all ECS Entities when `InternalSpatialOSCleanGroup` is run.

## Reactive components
A reactive component is implemented as a temporary component, and so has the same guarantees as a temporary component.

## Components on ECS Entities that represent SpatialOS entities

For each SpatialOS entity that a [worker]({{urlRoot}}/content/workers/workers-in-the-gdk) checks out, the `SpatialOSReceiveSystem` creates an ECS Entity for that worker’s ECS world.
The following guarantees hold for any ECS Entity representing a SpatialOS entity:

  * Any SpatialOS entity that is in the worker's view is represented as an ECS Entity.
  * If a SpatialOS entity gets removed from the worker's view, the corresponding ECS Entity is deleted.
  * The `NewlyAddedSpatialOSEntity` component is a temporary component and added upon creating these entities.
  * The `SpatialEntityId` component is always available on these entities.
  * The `{name of component}.Component` components are always available on these entities for all schema components that belong to these entities.
  * The `WorldCommands.{name of world command}.CommandSender` components are always available on these entities and contain the API to send [world commands]({{urlRoot}}/content/ecs/world-commands).
  * `{name of component}.CommandSenders.{name of command}` components are always available on these entities for every single command defined in your schema.
  * `{name of component}.CommandHandlers.{name of command}` components are available on these entities for all components ands that the worker is authoritative over.
  * `{name of component}.CommandResponders.{name of command}` components are always available on these entities for every single command defined in your schema.
  * `{name of component}.EventSender.{name of event}` components are available on these entities for all components that the worker is authoritative over.
  * At least one authority component is available on these entities based on the worker's component write access
    * `Authoritative<T>`: If the worker has write access to the component.
    * `NotAuthoritative<T>`: If the worker only has read access to the component.
    * `AuthorityLostImminent<T>`: If the worker is about to lose write access to the component. Note that the `Authoritative<T>` component is still attached to these entities.

### Guarantees when receiving updates or messages
Whenever a component update or message is received by the worker, the following holds for the entity that the worker received it for:

  * All updates received are stored as a list in a reactive component called `ReceivedUpdates<T>`, where `T` is the type of the component.
  * The `{name of component}.Component` component is updated to the values stored in the latest update received
  * All events received are stored as a list in a reactive component called `ReceivedEvents<T>`, where `T` is the type of the component.
  * All command requests received are stored as a list in a reactive component called `{name of component}.{name of command}.CommandRequests`, where `T` is the type of the component.
  * All command responses received are stored as a list in a reactive component called `{name of component}.{name of command}.CommandResponses`, where `T` is the type of the component.

### Guarantees when sending updates or messages
Whenever a component update or message is sent by the worker, the following holds for the entity that the worker sends it from:

  * Whenever a field inside a `{name of component}.Component` changes, the component update will be sent the next time the `SpatialOSSendSystem` is run unless a [custom replication system]() has been set up for this component.
  * All events added to the `{name of component}.EventSender.{name of event}`.Events will be sent the next time the `SpatialOSSendSystem` is run unless a custom replication system has been set up for this component.
  * All command requests added to the `{name of component}.CommandSenders.{name of command}.RequestsToSend`  will be sent the next time the `SpatialOSSendSystem` is run unless a custom replication system has been set up for this component.
  * All command responses added to the `{name of component}.CommandResponders.{name of command}.ResponsesToSend`  will be sent the next time the `SpatialOSSendSystem` is run unless a custom replication system has been set up for this component.

## Components on worker entities
A worker entity is created for each ECS World that is associated with a [worker]({{urlRoot}}/content/workers/workers-in-the-gdk).
We guarantee the following:

  * The worker entity will exist for as long as the ECS World exists.
  * The `WorkerEntityTag` component is always available on the worker entity.
  * The `WorldCommands.{name of world command}.CommandSender` components are always available on the worker entity and contain the API to send [world commands]({{urlRoot}}/content/ecs/world-commands).
  * `{name of component}.CommandSenders.{name of command}` components are always available on the worker entity for every single command defined in your schema.
  * `{name of component}.CommandResponders.{name of command}` components are always available on the worker entity for every single command defined in your schema.
  * The `OnConnected` component is a temporary component added upon creating the worker entity.
  * The `OnDisconnected` component is a temporary component added after the connection to the SpatialOS Runtime has been lost.

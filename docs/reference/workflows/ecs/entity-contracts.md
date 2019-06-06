<%(TOC)%>

# ECS: Entity contracts

<%(Callout message="
Before reading this document, make sure you are familiar with:

* [Temporary components]({{urlRoot}}/reference/workflows/ecs/temporary-components)
* [Entity lifecycle]({{urlRoot}}/reference/concepts/entity-lifecycle)
* [Worker entity]({{urlRoot}}/reference/workflows/ecs/worker-entity)
* [System update order]({{urlRoot}}/reference/workflows/ecs/system-update-order)
")%>

This documentation describes the guarantees we provide for the components that an ECS Entity has.

## Temporary components

Temporary components are components that only exist for one frame. We guarantee that all temporary components are removed from all ECS Entities when `InternalSpatialOSCleanGroup` is run.

## Worker entities

A worker entity is created for each ECS World that is associated with a [worker]({{urlRoot}}/reference/concepts/worker).

The following guarantees hold for worker entities:

* The worker entity will exist for as long as the ECS World exists.
* A [`WorkerEntityTag`]({{urlRoot}}/api/core/worker-entity-tag) component is always available on the worker entity.
* `WorldCommands.{name of world command}.CommandSender` components are always available on the worker entity and contain the API to send [world commands]({{urlRoot}}/reference/workflows/ecs/interaction/commands/world-commands).
* An [`OnConnected`]({{urlRoot}}/api/core/on-connected) temporary component is added upon creating the worker entity.
* An [`OnDisconnected`]({{urlRoot}}/api/core/on-disconnected) temporary component is added after the connection to the SpatialOS Runtime has been lost.

## ECS Entities representing SpatialOS entities

For each SpatialOS entity that a [worker]({{urlRoot}}/reference/concepts/worker) checks out, the [`EcsViewSystem`]({{urlRoot}}/api/core/ecs-view-system) creates an ECS Entity for that worker’s ECS world.

The following guarantees hold for any ECS Entity representing a SpatialOS entity:

* Any SpatialOS entity that is in the worker's view is represented as an ECS Entity.
* If a SpatialOS entity gets removed from the worker's view, the corresponding ECS Entity is deleted.
* A [`NewlyAddedSpatialOSEntity`]({{urlRoot}}/api/core/newly-added-spatial-os-entity) temporary component is added upon creating these entities.
* A [`SpatialEntityId`]({{urlRoot}}/api/core/spatial-entity-id) component is always available on these entities.
* `{name of component}.Component` components are always available on these entities for all schema components that belong to these entities.
* `{name of component}.ComponentAuthority` components are always available on these entities for all schema components that belong to these entities.
* `WorldCommands.{name of world command}.CommandSender` components are always available on these entities and contain the API to send [world commands]({{urlRoot}}/reference/workflows/ecs/interaction/commands/world-commands).

### Guarantees when receiving updates or messages

Whenever a component update or message is received by the worker, the following holds for the entity that the worker received it for:

* All updates received are stored as a list in a component called `{name of component}.ReceivedUpdates`.
* The `{name of component}.Component` component is updated to the values stored in the latest update received.

### Guarantees when sending updates or messages

Whenever a component update or message is sent by the worker, the following holds for the entity that the worker sends it from:

* Whenever a field inside a `{name of component}.Component` changes, the component update will be sent the next time [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.

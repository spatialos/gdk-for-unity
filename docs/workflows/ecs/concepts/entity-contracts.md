<%(TOC)%>

# ECS: Entity contracts

<%(Callout message="
Before reading this document, make sure you are familiar with:

* [Temporary components]({{urlRoot}}/workflows/ecs/concepts/temporary-components)
* [Worker entity]({{urlRoot}}/workflows/ecs/worker-entity)
* [System update order]({{urlRoot}}/workflows/ecs/concepts/system-update-order)
")%>

This documentation describes the guarantees we provide for the components that an ECS Entity has.

## Worker entities

A worker entity is created for each ECS World that is associated with a [worker]({{urlRoot}}/reference/concepts/worker).

The following guarantees hold for worker entities:

* The worker entity will exist for as long as the ECS World exists.
* A [`WorkerEntityTag`]({{urlRoot}}/api/core/worker-entity-tag) component is always available on the worker entity.
* An [`OnConnected`]({{urlRoot}}/api/core/on-connected) temporary component is added upon creating the worker entity.
* An [`OnDisconnected`]({{urlRoot}}/api/core/on-disconnected) temporary component is added after the connection to the SpatialOS Runtime has been lost.

## ECS Entities representing SpatialOS entities

For each SpatialOS entity that a [worker]({{urlRoot}}/reference/concepts/worker) checks out, the [`EcsViewSystem`]({{urlRoot}}/api/core/ecs-view-system) creates an ECS Entity for that worker’s ECS world.

The following guarantees hold for any ECS Entity representing a SpatialOS entity:

* Any SpatialOS entity that is in the worker's view is represented as an ECS Entity.
* If a SpatialOS entity gets removed from the worker's view, the corresponding ECS Entity is deleted.
* A [`NewlyAddedSpatialOSEntity`]({{urlRoot}}/api/core/newly-added-spatial-os-entity) temporary component is added upon creating these entities.
* A [`SpatialEntityId`]({{urlRoot}}/api/core/spatial-entity-id) component is always available on these entities.
* `{component name}.Component` components are always available on these entities for all schema components that belong to these entities.
* `{component name}.ComponentAuthority` components are always available on these entities for all schema components that belong to these entities.
* `WorldCommands.{name of world command}.CommandSender` components are always available on these entities and contain the API to send [world commands]({{urlRoot}}/workflows/ecs/interaction/world-commands).

### Guarantees when receiving updates or messages

Whenever a component update or message is received by the worker, the following holds for the entity that the worker received it for:

* The `{component name}.Component` component is updated to the values stored in the latest update received.

### Guarantees when sending updates or messages

Whenever a component update or message is sent by the worker, the following holds for the entity that the worker sends it from:

* Whenever a field inside a `{component name}.Component` changes, the component update will be sent the next time [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.

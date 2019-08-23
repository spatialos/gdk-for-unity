<%(TOC)%>

# ECS: Reactive component entity contracts

<%(Callout message="
Before reading this document, make sure you are familiar with:

* [Temporary components]({{urlRoot}}/workflows/ecs/concepts/temporary-components)
* [Reactive components]({{urlRoot}}/workflows/ecs/interaction/reactive-components/overview)
")%>

This documentation describes the guarantees we provide for the reactive components that an ECS entity can have.

## ECS entities representing SpatialOS entities

* `{component name}.CommandSenders.{command name}` components are always available on these entities for every single command defined in your schema.
* `{component name}.CommandResponders.{command name}` components are available on these entities for all components ands that the worker is authoritative over.
* `{component name}.EventSender.{event name}` components are available on these entities for all components that the worker is authoritative over.

## Authority

At least one authority component is available on these entities based on the worker's component write access.

* `Authoritative<T>`: If the worker has write access to the component.
* `NotAuthoritative<T>`): If the worker only has read access to the component.
* `AuthorityLostImminent<T>`: If the worker is about to lose write access to the component. Note that the `Authoritative<T>` component is still attached to these entities.

## Receiving updates or messages

* All component updates received are stored as a list in a reactive component called `{component name}.ReceivedUpdates`.
* All events received are stored as a list in a reactive component called `{component name}.ReceivedEvents.{event name}`.
* All command requests received are stored as a list in a reactive component called `{component name}.{command name}.CommandRequests`.
* All command responses received are stored as a list in a reactive component called `{component name}.{command name}.CommandResponses`.

## Sending updates or messages

* All events added to the `{component name}.EventSender.{event name}`.Events list will be sent the next time the [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.
* All command requests added to the `{component name}.CommandSenders.{command name}.RequestsToSend`  will be sent the next time the [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.
* All command responses added to the `{component name}.CommandResponders.{command name}.ResponsesToSend`  will be sent the next time the [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.

## Worker entity

* `{component name}.CommandSenders.{command name}` components are always available on the worker entity for every single command defined in your schema.
* `{component name}.CommandResponders.{command name}` components are always available on the worker entity for every single command defined in your schema.
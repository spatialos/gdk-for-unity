<%(TOC)%>

# ECS: Reactive component entity contracts

<%(Callout message="
Before reading this document, make sure you are familiar with:

* [Temporary components]({{urlRoot}}/reference/workflows/ecs/temporary-components)
* [Reactive components]({{urlRoot}}/reference/workflows/ecs/interaction/reactive-components/overview)
")%>

This documentation describes the guarantees we provide for the reactive components that an ECS entity can have.

## ECS entities representing SpatialOS entities

* `{name of component}.CommandSenders.{name of command}` components are always available on these entities for every single command defined in your schema.
* `{name of component}.CommandResponders.{name of command}` components are available on these entities for all components ands that the worker is authoritative over.
* `{name of component}.EventSender.{name of event}` components are available on these entities for all components that the worker is authoritative over.

## Authority

At least one authority component is available on these entities based on the worker's component write access.

* [`Authoritative<T>`]({{urlRoot}}/api/reactive-components/authoritative): If the worker has write access to the component.
* [`NotAuthoritative<T>`]({{urlRoot}}/api/reactive-components/not-authoritative): If the worker only has read access to the component.
* [`AuthorityLostImminent<T>`]({{urlRoot}}/api/reactive-components/authority-loss-imminent): If the worker is about to lose write access to the component. Note that the [`Authoritative<T>`]({{urlRoot}}/api/reactive-components/authoritative) component is still attached to these entities.

## Receiving updates or messages

* All events received are stored as a list in a reactive component called `{name of component}.ReceivedEvents.{name of event}`.
* All command requests received are stored as a list in a reactive component called `{name of component}.{name of command}.CommandRequests`.
* All command responses received are stored as a list in a reactive component called `{name of component}.{name of command}.CommandResponses`.

## Sending updates or messages

* All events added to the `{name of component}.EventSender.{name of event}`.Events list will be sent the next time the [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.
* All command requests added to the `{name of component}.CommandSenders.{name of command}.RequestsToSend`  will be sent the next time the [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.
* All command responses added to the `{name of component}.CommandResponders.{name of command}.ResponsesToSend`  will be sent the next time the [`SpatialOSSendSystem`]({{urlRoot}}/api/core/spatial-os-send-system) is run.

## Worker entity

* `{name of component}.CommandSenders.{name of command}` components are always available on the worker entity for every single command defined in your schema.
* `{name of component}.CommandResponders.{name of command}` components are always available on the worker entity for every single command defined in your schema.
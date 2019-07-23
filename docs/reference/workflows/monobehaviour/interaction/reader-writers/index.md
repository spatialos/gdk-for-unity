<%(TOC)%>

# Readers and Writers

_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/reference/workflows/overview#monobehaviour-centric-workflow)._

Before reading this document, make sure you are familiar with:

* [Workers]({{urlRoot}}/reference/concepts/worker)
* (Optional) [SpatialOS schema]({{urlRoot}}/reference/glossary#schema).
* (Optional) [Read and write access]({{urlRoot}}/reference/glossary#authority)

Readers and Writers allow you to inspect and change the state of SpatialOS components using MonoBehaviours by letting you perform the following actions:

**Reader**

* Read the data of a SpatialOS component.
* Read the authority state of your worker over a SpatialOS component.
* Register callbacks for reacting to property value changed of a SpatialOS component.
* Register callbacks for reacting to events corresponding to a SpatialOS component.
* Register callbacks for reacting to changes to the authority state of your worker over a SpatialOS component.

**Writer**

* Change the property values of a SpatialOS component.
* Send events defined in a SpatialOS component.
* Send acknowledgements for [`AuthorityLossImminent` notifications](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/understanding-access#enabling-and-configuring-authoritylossimminent-notifications).
* All the functionality that a Reader provides.

> Note that a writer will only receive authority state change callbacks for `AuthorityLossImminent`.

For every SpatialOS component defined in schema, the GDK generates a Reader and Writer. After code generation has finished, the generated Readers and Writers are located in the following namespace:

  * `<namespace of schema component>.<component name>Reader`
  * `<namespace of schema component>.<component name>Writer`

You can use Readers and Writers by declaring a field in your MonoBehaviour and decorating it with the `[Require]` attribute (See documentation on [interacting with SpatialOS using MonoBehaviours)]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/lifecycle). The GDK automatically injects these fields with their corresponding Readers and Writers, if the following requirements are fulfilled:

  * A reader for a specific component can be injected as long as the worker has read access over this component.
  * A writer for a specific component can only be injected, if the worker has write access over this component.

You can find out more about how to work with Readers and Writers in:

  * [How to interact with SpatialOS using MonoBehaviours]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/lifecycle)
  * [How to read, update and react to changes]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/component-data-updates)
  * [How to send and receive events]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/events)

## Reader API

For the following API, let:

* `TComponentData` be the component data type for the SpatialOS component of this Reader. (`<namespace of schema component>.<component name>.Component`)
* `TComponentUpdate` be the update type for the SpatialOS component of this Reader. (`<namespace of schema component>.<component name>.Update`)

**Fields:**

| Field         	| Type               	| Description                	|
|-------------------|------------------------|--------------------------------|
| Data  	| `TComponentData`              	| The data stored inside the component that this Reader is associated with. |
| Authority | `Authority` | The [authority]({{urlRoot}}/reference/glossary#authority) status of the current worker of the component that this Reader is associated with. |

**Events:**
```csharp
event Action<TComponentUpdate> OnUpdate;
```
Register to this event to receive a callback whenever any property of a
component was changed.

Callback parameter:

  * `TComponentUpdate`: This will contain fields wrapped inside the `Option` struct to identify which fields were changed.

```csharp
event Action<TField> On{Name of TField}Updated;
```

Register to this event to receive a callback whenever a specific field of the
component is updated.

Callback parameter:

  * `TField`: The type of this field is code-generated and depends on the schema definition of the field. It will contain the updated value of the specified field.

> General `OnUpdate` callbacks are invoked before specific property update callbacks.

```csharp
event Action<TEventPayload> On{Name of TEvent}Event;
```

Register to this event to receive a callback whenever the corresponding SpatialOS event is triggered on this SpatialOS entity.

Callback parameter:

  * `TEventPayload`: The type of the event payload for `TEvent`. This type depends on the schema definition of the event. It will contain the event payload.


```csharp
event Action<Authority> OnAuthorityUpdate;
```
Register to this event to receive a callback whenever the authority
status of your worker over the corresponding component is changed.

Callback Parameters

  * `Authority`: Contains the new authority status of the worker.

## Writer API

> Note that a Writer inherits from the Reader for the same SpatialOS component and such the Reader API is also available from a Writer.

For the following API, let:

* `TComponentData` be the component data type for the SpatialOS component of this Reader. (`<namespace of schema component>.<component name>.Component`)
* `TComponentUpdate` be the update type for the SpatialOS component of this Reader. (`<namespace of schema component>.<component name>.Update`)

**Methods**
```csharp
void SendUpdate(TComponentUpdate update);
```
Allows you to send a component update to the [SpatialOS Runtime]({{urlRoot}}/reference/glossary#spatialos-runtime).

Parameters:

  * `TComponentUpdate update`:  An update object which contains the changed field data. Any field that should not change, should not be set.

```csharp
void Send{name of event}Event(TEventPayload payload);
```

Allows you to send an event. These methods are code-generated for each event in your component.

Parameters:

  * `TEventPayload payload`: The data that you want to send with your event. The exact type is specified by the schema definition of the event.

```csharp
void AcknowledgeAuthorityLoss();
```
Allows you to send acknowledgements for [`AuthorityLossImminent` notifications](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/design/understanding-access#enabling-and-configuring-authoritylossimminent-notifications).

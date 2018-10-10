**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only.

------

[//]: # (Doc of docs reference 6.1)
[//]: # (TODO - Tech writer pass)
[//]: # (TODO - See if `option` struct needs defining)


## (GameObject-MonoBehaviour) Readers and Writers
_This document relates to the [GameObject-MonoBehaviour workflow](../intro-workflows-spos-entities.md#spatialos-entities)._

Before reading this document, make sure you are familiar with:
  * [Linking SpatialOS entities with GameObjects](./linking-spos-entities-gameobjects.md)
  * [Workers](../workers/workers-in-the-gdk.md)
  * (Optional) [SpatialOS schema](../glossary.md#schema).
  * (Optional) [Read and write access](../glossary.md#authority)

Readers and Writers allow you to inspect and change the state of SpatialOS components using MonoBehaviours by letting you perform the following actions:

**Reader**
- Read the data of a SpatialOS component.
- Read the authority state of your worker over a SpatialOS component.
- Register callbacks for reacting to property value changed of a SpatialOS component.
- Register callbacks for reacting to events corresponding to a SpatialOS component.
- Register callbacks for reacting to changes to the authority state of your worker over a SpatialOS component.

**Writer**
- Change the property values of a SpatialOS component.
- Send events defined in a SpatialOS component.
- All the functionality that a Reader provides (except for listening to authority state changes)

For every SpatialOS component defined in schema, the GDK generates a Reader and Writer. After code generation has finished, the generated Readers and Writers are located in the following namespace:

- `<namespace of schema component>.<component name>.Requirable.Reader`
- `<namespace of schema component>.<component name>.Requirable.Writer`

You can use Readers and Writers by declaring a field in your MonoBehaviour and decorating it with the `[Require]` attribute (See documentation on [interacting with SpatialOS using MonoBehaviours)](./interact-spos-monobehaviours.md). The GDK automatically injects these fields with their corresponding Readers and Writers, if the following requirements are fulfilled:
  * A reader for a specific component can be injected as long as the worker has read access over this component.
  * A writer for a specific component can only be injected, if the worker has write access over this component.

You can find out more about how to work with Readers and Writers in:
  * [How to interact with SpatialOS using MonoBehaviours](that's the current Developing SpatialOS games using GameObjects and MonoBehaviours doc)
  * [How to read, update and react to changes](reading-and-writing-component-data.md)
  * [How to send and receive events](sending-receiving-events.md)

#### Reader API
The `IReader` interface is bound to the following generic:
  * `TSpatialComponentData : ISpatialComponentData`
 
The exact type depends on the component that the reader is generated for.

**Fields:**

| Field         	| Type               	| Description                	|
|-------------------|------------------------|--------------------------------|
| Data  	| TSpatialComponentData              	| The data stored inside the component that this Reader is associated with. |
| Authority | [Authority](../glossary.md#schema) | The authority state of the current worker of the component that this Reader is associated with. |

**Events:**
```csharp
event Action<TComponentUpdate> ComponentUpdated;
```
Register to this event to receive a callback whenever any property of a
component was changed.

Callback parameter:
  * `TComponentUpdate`: This will contain fields wrapped inside the `Option` struct to identify which fields were changed.

```csharp
event Action<TField> {Name of TField}Updated;
```
Register to this event to receive a callback whenever a specific field of the
component is updated.

Callback parameter:
  * `TField`: The type of this field is code-generated and depends on the schema definition of the field. It will contain the updated value of the specified field.
 
> General `ComponentUpdated` callbacks are invoked before specific property update callbacks.

```csharp
event Action<Authority> AuthorityChanged;
```
Register to this event to receive a callback whenever the authority
status of your worker over the corresponding component is changed.

Callback Parameters
  * `Authority`: Contains the new authority status of the worker.

#### Writer API
The `IWriter` interface is bound to the following generics:
  * `TSpatialComponentData : ISpatialComponentData`
  * `TSpatialComponentUpdate : ISpatialComponentUpdate`
 
The exact type of these generics depends on the component that the writer is generated for.

**Fields:**

| Field         	| Type               	| Description                	|
|-------------------|------------------------|--------------------------------|
| Data  	| TSpatialComponentData              	| Thee data stored inside the component that this Reader is associated with. |
| Authority | [Authority](add link to SpatialOS doc) | The [authority]() status of the current worker of the component that this Reader is associated with. |


**Events:**
```csharp
event Action<TSpatialComponentData> ComponentUpdated;
```
Register to this event to receive a callback whenever any property of a
component was changed.

Callback parameter:
  * `TSpatialComponentData`: It will contain non-empty values for all fields that were changed.

```csharp
event Action<TField> {Name of TField}Updated;
```
Register to this event to receive a callback whenever a specified field of the
component is updated.

Callback parameter:
  * `TField`: The type of this field is code generated and depends on the schema definition of the field. It will contain the updated value of the specified field.
 
> General `ComponentUpdated` callbacks are invoked before specific field update callbacks.

```csharp
event Action<Authority> AuthorityChanged;
```
Register to this event to receive a callback whenever the authority
status of your worker over the corresponding component is changed.

> This callback, although available, will not be invoked on Writers. A MonoBehaviour requiring a Writer is automatically enabled and disabled upon gaining/losing authority.

Callback Parameters
  * `Authority`: Contains the new authority status of the worker.

**Methods**
```csharp
void Send(TComponentUpdate update);
```
Allows you to send a component update to the [SpatialOS Runtime](link to glossary).

Parameters:
  * `TComponentUpdate update`:  An update object which contains the changed field data. Any field that should not change, should not be set.

```csharp
void Send{name of event}(TEventPayload payload);
```

Allows you to send an event. These methods are code-generated for each event in your component.

Parameters:
  * `TEventPayload payload`: The data that you want to send with your event. The exact type is specified by the schema definition of the event.

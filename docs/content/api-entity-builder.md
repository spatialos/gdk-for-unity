[//]: # (Doc of docs reference 28)

# SpatialOS entities: API - EntityBuilder
_This document relates to both *[GameObject-MonoBehaviour and  ECS workflows](../intro-workflows-spos-entities.md)_

Before reading this document, make sure you are familiar with the documentation on [entity templates](./entity-templates.md).

Use the `EntityBuilder` class to create entity templates. You use the `EntityTemplate` class to specify all the [components](./glossary.md#spatialos-components) that a SpatialOS entity has, the initial values of those components, and which workers have [write access](./glossary.md#authority) to each component.

The `EntityBuilder` class is part of the `Improbable.Gdk.Core` assembly.


## Public methods
Each method returns the `EntityBuilder` instance using the builder design pattern (Wikipedia).

**Methods**<br/>
```csharp
.Begin()
```
Create a new `EntityBuilder` instance for creating an `EntityTemplate`.

```csharp
.AddPosition(double x, double y, double z, string writeAccess)
```
Add a [Position](./glossary.md#position) component to your entity and specify which worker type(s) can have authority over it.

```csharp
.SetPersistence(bool persistence)
```
Specify whether your entity should be saved in [snapshots](./snapshots.md). For more information, see the SpatialOS documentation on [persistence](./glossary.md#persistence).

```csharp
.SetReadAcl(string attribute, param string[] attribute)
```
```csharp
.SetReadAcl(List<string> attribute)
```
Specify which worker type(s) can have [read access](./glossary.md#authority) to the entity.

```csharp
.SetEntityAclComponentWriteAccess(string attribute)
```
Specify which worker type can have [write access](./glossary.md#authority) over the [EntityAcl](./glossary.md#authority) component of your entity. This is useful if, while the game is running, you want to change which [worker type(s)] (TODO: link to the glossary worker types) can have write-access over the entity's other components.

```csharp
.AddComponent(ComponentData data, string writeAccess)
```
Add a user-defined component to your entity and specify which [worker type(s)] (TODO: link to the glossary worker types)  can have [write access](./glossary.md#authority) over it.

```csharp
.Build()
```
Create a finished `EntityTemplate`.

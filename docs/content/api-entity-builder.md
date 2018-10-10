[//]: # (Doc of docs reference 28)

# SpatialOS entities: API - EntityBuilder
_This document relates to both [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities)_

Before reading this document, make sure you are familiar with the documentation on [entity templates]({{urlRoot}}/content/entity-templates).

Use the `EntityBuilder` class to create entity templates. You use the `EntityTemplate` class to specify all the [components]({{urlRoot}}/content/glossary#spatialos-component) that a SpatialOS entity has, the initial values of those components, and which workers have [write access]({{urlRoot}}/content/glossary#authority) to each component.

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
Add a [Position]({{urlRoot}}/content/glossary#position) component to your entity and specify which worker type(s) can have authority over it.

```csharp
.SetPersistence(bool persistence)
```
Specify whether your entity should be saved in [snapshots]({{urlRoot}}/content/snapshots). For more information, see the SpatialOS documentation on [persistence]({{urlRoot}}/content/glossary#persistence).

```csharp
.SetReadAcl(string attribute, param string[] attribute)
```
```csharp
.SetReadAcl(List<string> attribute)
```
Specify which worker type(s) can have [read access]({{urlRoot}}/content/glossary#read-access) to the entity.

```csharp
.SetEntityAclComponentWriteAccess(string attribute)
```
Specify which worker type can have [write access]({{urlRoot}}/content/glossary#write-access) over the [EntityAcl]({{urlRoot}}/content/glossary#access-control-list-acl) component of your entity. This is useful if, while the game is running, you want to change which [worker type(s)]({{urlRoot}}/content/glossary#worker-types) can have write-access over the entity's other components.

```csharp
.AddComponent(ComponentData data, string writeAccess)
```
Add a user-defined component to your entity and specify which [worker type(s)]({{urlRoot}}/content/glossary#worker-types) can have [write access](({{urlRoot}}/content/glossary#authority) over it.

```csharp
.Build()
```
Create a finished `EntityTemplate`.

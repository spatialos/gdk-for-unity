<%(TOC)%>

# Entity templates

<%(Callout message="
Before reading this document, make sure you are familiar with:

  * [MonoBehaviour and ECS workflows]({{urlRoot}}/reference/workflows/which-workflow)
  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
  * [Standard schema library components](https://docs.improbable.io/reference/latest/shared/glossary#standard-schema-library-components)
")%>

An [`EntityTemplate`]({{urlRoot}}/api/core/entity-template) specifies what [components]({{urlRoot}}/reference/glossary#spatialos-component) a [SpatialOS entity]({{urlRoot}}/reference/glossary#spatialos-entity) will have, the initial values of these components and which [layers](https://docs.improbable.io/reference/latest/shared/glossary#layers) get [write access]({{urlRoot}}/reference//glossary#authority) to each component.

## How to define an entity template

To define an entity template, you must first construct an `EntityTemplate`.

```csharp
var entityTemplate = new EntityTemplate();
```

An `EntityTemplate` can be mutated and used multiple times.

### Add components to entity template

All SpatialOS entities require the `Position` and `EntityAcl` components.

The `Position` component must be added to the entity template manually. It is used by SpatialOS for load-balancing purposes and some types of queries.

The `EntityAcl` component is automatically handled by the `EntityTemplate` class. It is this component that determines which types of workers have read access to an entity and, for each component, which type of worker can have write access.

> Understand more about the standard schema library components [here](https://docs.improbable.io/reference/latest/shared/glossary#standard-schema-library-components).

To add components to the entity template, use the `AddComponent` method. For example, to add a `Position` component with [write access]({{urlRoot}}/reference//glossary#authority) given to UnityGameLogic workers:

```csharp
entityTemplate.AddComponent(new Position.Snapshot(coords), "UnityGameLogic");
```

The `AddComponent` method updates the write access attribute for the given component in the `EntityAcl`.

### Set read access

To ensure that interested workers can read the components and values of an entity, the read access must be set in the `EntityAcl` component.

This is done by calling the `SetReadAccess` method.

```csharp
// Ensures that any UnityGameLogic and UnityClient worker can read all components on an entity
entityTemplate.SetReadAccess("UnityGameLogic", "UnityClient");
```

## Advanced usage

In addition to adding components and setting read access, the `EntityTemplate` class provides other utility methods for:

* getting, overriding or removing component snapshots from the template
* getting or overriding component write access attributes for a given component in the template
* creating an `Entity` instance from the template

You can find more information about these methods in the [API reference documentation]({{urlRoot}}/api/core/entity-template#entitytemplate-class).

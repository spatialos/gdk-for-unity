<%(TOC)%>
# How it works

<%(Callout message="
Before reading this document, make sure you are familiar with:

  * [Query-based interest](https://docs.improbable.io/reference/latest/shared/reference/query-based-interest)
  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

## Interest

Query-based interest is enabled for an entity by adding the [`improbable.Interest`](https://docs.improbable.io/reference/latest/shared/schema/standard-schema-library#interest-optional) component to that entity.

`Interest` is effectively a mapping from a component ID to a list of queries, where each list of queries is only run on a worker that is authoritative over the component with that ID.

For example, if you want to control the interest of a worker responsible for simulating the position of a player entity, you could use the `Position` component, whose ID is `54`, as the key to map a set of queries to. These queries would then define which components the worker should be interested in.

## Queries

A query is represented as a constraint and what components the query should return from entities that satisfy the constraint. You can specify exactly what components to return or simply request that the query return _all_ components on matching entities.

## Constraints

Constraints are used to define what entities a query should be looking for.

The available constraints with Query-based interest are:

|Constraint|Description|
|---|---|
|Sphere|Entities in a sphere around a given point.|
|Cylinder|Entities in a cylinder around a given point.|
|Box|Entities in a box around a given point.|
|Relative sphere|Entities in a sphere around an entity's `Position`.|
|Relative cylinder|Entities in a cylinder around an entity's `Position`.|
|Relative box|Entities in a box around an entity's `Position`.|
|Entity ID|Entities matching a given entity ID.|
|Component|Entities in the world that have a given component.|
|And|Entities matching all given constraints.|
|Or|Entities matching at least one given constraint.|

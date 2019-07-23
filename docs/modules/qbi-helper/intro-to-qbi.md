<%(TOC)%>

# Introduction to query-based interest

<%(Callout message="
Before reading this document, make sure you are familiar with:

  * [Query-based interest](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/worker-configuration/query-based-interest#query-based-interest-beta)
  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

This page provides a quick overview of the key primitives in query-based interest.

## Interest

Query-based interest is enabled for an entity by adding the [`improbable.Interest`](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/schema/standard-schema-library#interest-optional) component to that entity.

`Interest` is effectively a mapping from a component ID to a list of queries, where each list of queries is only active on the worker that is authoritative over the component with that ID.

For example, if you want to control the interest of a worker responsible for simulating the position of a player entity, you could use the `Position` component, whose ID is `54`, as the key to a set of queries. These queries would then define which components the worker should be interested in.

> The [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template) class provides methods to define and manipulate an `Interest` component.

## Queries

A query is represented as a constraint. The constraint specifies for which entities we want to receive components . You can either return _all_ components or specify a list of components that should be returned for all entities satisfying the constraint.

> The [`InterestQuery`]({{urlRoot}}/api/query-based-interest/interest-query) class provides methods to create a query, set a constraint and define what components the query should return.

## Constraints

Constraints are used to define what entities a query should be looking for.

The available constraints with query-based interest are:

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

> The static [`Constraint`]({{urlRoot}}/api/query-based-interest/constraint) class provides constructors for each constraint defined in the table above.

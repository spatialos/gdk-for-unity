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

<%(#Expandable title="How is interest defined in schema?")%>

Interest is defined by the `Improbable.Interest` component, which can be found in the [standard schema library](https://docs.improbable.io/reference/latest/shared/schema/standard-schema-library).

```
component Interest {
   id = 58;
   map<uint32, ComponentInterest> component_interest = 1;
}

type ComponentInterest {
   ...

   list<Query> queries = 1;
}
```

<%(/Expandable)%>

## Queries

A query is represented as a constraint and what components the query should return from entities that satisfy the constraint. You can specify exactly what components to return or simply request that the query return _all_ components on matching entities.

<%(#Expandable title="How is a query defined in schema?")%>

A query is represented by the `Query` type, defined inside the `Improbable.Interest` component found in the standard schema library.

```
type Query {
   QueryConstraint constraint = 1;

   // Either full_snapshot_result or a list of result_component_id
   // should be provided. Providing both is invalid.
   option<bool> full_snapshot_result = 2;
   list<uint32> result_component_id = 3;
}
```

<%(/Expandable)%>

## Constraints

Constraints are used to define what entities a query should be looking for. This should be familiar if you have used [queries](https://docs.improbable.io/reference/13.6/shared/glossary#queries) outside of Query-based interest before.

For a query to be a valid, only one of the following constraints should be provided:

* `sphere_constraint`: entities in sphere around a given point.
* `cylinder_constraint`: entities in cylinder around a given point.
* `box_constraint`: entities in box around a given point.
* `relative_sphere_constraint`: entities in sphere around an entity's `Position`.
* `relative_cylinder_constraint`: entities in cylinder around an entity's `Position`.
* `relative_box_constraint`: entities in box around an entity's `Position`.
* `entity_id_constraint`: entities matching a given entity ID.
* `component_constraint`: entities in the world that have a given component.
* `and_constraint`: entities matching all given constraints.
* `or_constraint`: entities matching at least one given constraint.

<%(#Expandable title="How is a constraint defined in schema?")%>

A constraint is represented by the `QueryConstraint` type, which is defined inside the `Improbable.Interest` component found in the standard schema library. Only one constraint field within a `QueryConstraint` must be set to construct a valid query.

```
type QueryConstraint {
    // Only one constraint should be provided. Providing more than one is
    // invalid.

    option<SphereConstraint> sphere_constraint = 1;
    option<CylinderConstraint> cylinder_constraint = 2;
    option<BoxConstraint> box_constraint = 3;
    option<RelativeSphereConstraint> relative_sphere_constraint = 4;
    option<RelativeCylinderConstraint> relative_cylinder_constraint = 5;
    option<RelativeBoxConstraint> relative_box_constraint = 6;
    option<int64> entity_id_constraint = 7;
    option<uint32> component_constraint = 8;
    list<QueryConstraint> and_constraint = 9;
    list<QueryConstraint> or_constraint = 10;
}
```

<%(/Expandable)%>

<!-- ignore this -->

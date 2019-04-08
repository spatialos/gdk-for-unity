<%(TOC)%>
# Interest construction

<%(Callout message="
Before reading this document, make sure you are familiar with:

  * [How Query-based interest works]({{urlRoot}}/modules/qbi-helper/how-it-works)
")%>

This module offers 3 classes to help construct your `Interest` component:

* `InterestTemplate`, to define and manipulate an `Interest` component.
* `InterestQuery`, an interest query with methods to easily set a `Constraint` and define what components the query should return.
* `Constraint`, a static class with constructors for all the possible constraints an `InterestQuery` can have.

## InterestTemplate

An `InterestTemplate` is effectively a wrapper around the `Interest` component, providing ways to more easily add, replace and clear queries from the underlying interest map.

### Initialise

You initialise an empty `InterestTemplate` by calling `InterestTemplate.Create()`. This is useful when defining a set of interest queries for the first time, for example when defining Entity Templates.

```csharp
var basicInterestTemplate = InterestTemplate.Create();
```

You can also construct this from an existing `InterestTemplate` or set of interest queries. This creates a deep copy of the `InterestTemplate`, to allow you to modify the queries without affecting the original set.

```csharp
var advancedInterestTemplate = InterestTemplate.Create(basicInterestTemplate);
```

### Modify

When modifying an `InterestTemplate`, you must specify which particular component's queries you are modifying. To do this, you can either:

* provide the component as a type argument
* pass the component ID as the first argument

For example, to add a query you might do either:

```csharp
InterestTemplate.Create()
    .AddQueries<Position.Component>(query1, query2);
```

or

```csharp
InterestTemplate.Create()
    .AddQueries(Position.ComponentId, query1, query2);
```

#### Add

When adding queries, you can either provide the queries as parameters or an enumerable set:

```csharp
// Parameters
InterestTemplate.Create()
    .AddQueries<Position.Component>(query1, query2);

// Enumerable
var queryList = new List() { query1, query2 };

InterestTemplate.Create()
    .AddQueries<Position.Component>(queryList);
```

#### Replace

By doing this, you replace all the existing queries for an authoritative component with the new queries you pass in. In a similar way to adding queries, you must provide the queries as parameters or an enumerable set.

```csharp
// Parameters
InterestTemplate.Create(basicInterestTemplate)
    .ReplaceQueries<Position.Component>(query1);

// Enumerable
var queryList = new List() { query1 };

InterestTemplate.Create(basicInterestTemplate)
    .ReplaceQueries<Position.Component>(queryList);
```

At least one query must be provided, otherwise the existing queries for a given authoritative component will not be removed.

#### Clear



###

## Basic construction

### Define a query

To create an interest query, you must call `InterestQuery.Query` and provide a `Constraint`. For example:

```csharp
var query = InterestQuery
    .Query(Constraint.RelativeSphere(20));
```

You can set a filter on which components to return in the query’s results.

```csharp
var query = InterestQuery
    .Query(constraint)
    .FilterResults(Position.ComponentId, Metadata.ComponentId);
```

### Add query to InterestTemplate


## Advanced features

### Replace

### Clear

### Creating query constraints
Query constraints are the conditions that an interest query must meet in order to return results. You can use the `Constraint` class to create constraints for interest queries. For example:

### Use InterestTemplate to add/replace/clear queries

##### Initialise

You initialise an InterestTemplate with an empty set of Interest queries by calling `InterestTemplate.Create()`. This is useful when defining a set of Interest queries for the first time, for example when defining Entity Templates.

You can also use an existing Interest component or set of Interest queries to initialise an InterestTemplate.

```charp
//Start with an initial set of Interest queries without modifying the given set
InterestTemplate.Create(InterestTemplate interestTemplate)
InterestTemplate.Create(Dictionary<uint, ComponentInterest> interest)
InterestTemplate.Create(Interest.Component)

//Mutate the given set of Interest queries directly
InterestTemplate.CreateReference(Dictionary<uint, ComponentInterest> interest)
InterestTemplate.CreateReference(Interest.Component)
```

Use this to update the Interest component when the game is running, for example:

```csharp
InterestTemplate.CreateReference(InterestWriter.Data)
   .ReplaceQueries<Position.Component>(query);
```

##### Add, replace, clear

The InterestTemplate exposes methods for adding, replacing and clearing queries from your Interest component. You can use these methods interchangeably in any order. You must provide at least one query to add or replace queries. The generic parameter provides a way to specify the authoritative component you would like to add the queries to. For example:

```csharp
InterestTemplate.Create(InterestComponent)
    .AddQueries<Position.Component>(query1, query2)
    .ReplaceQueries<ServerMovement.Component>(query3, query4)
    .ClearQueries<PowerUp.Component>();
```

Alternatively, omit the generic parameter and provide the component ID as the first argument directly:

```csharp
InterestTemplate.Create(InterestComponent)
    .AddQueries(Position.ComponentId, query1, query2)
    .ReplaceQueries(ServerMovement.ComponentId, query3, query4)
    .ClearQueries(PowerUp.ComponentId);
```

You can also remove the queries for all authoritative components at once, for example:

```csharp
InterestTemplate.Create(InterestComponent)
    .ClearAllQueries();
```

##### Output

After manipulating Interest with the InterestTemplate, you can choose to return a Snapshot or the underlying Dictionary that holds the Interest queries. For example:

```csharp
var interestTemplate = InterestTemplate.Create()
    .AddQueries<Position.Component>(query1, query2);

var entityTemplate = new EntityTemplate();
…
entityTemplate.AddComponent(interestTemplate.ToSnapshot(), WorkerUtils.UnityGameLogic);
```

When the game is running, you can update the Interest component like so:

```csharp
InterestWriter.Data.ComponentInterest = InterestTemplate.Create(InterestWriter.Data)
    .ClearAllQueries()
    .ToComponentInterest();
```

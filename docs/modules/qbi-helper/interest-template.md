<%(TOC)%>

# How to use InterestTemplate

<%(Callout message="
Before reading this document, make sure you have read:

  * [Introduction to Query-based interest]({{urlRoot}}/modules/qbi-helper/intro-to-qbi)
")%>

An [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template) is a wrapper around the `Interest` component, providing intuitive methods to add, replace and clear queries from the underlying interest map.

## Create a template

You create an empty [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template) by calling `InterestTemplate.Create()`. This is useful when defining a set of interest queries for the first time, for example when defining Entity Templates.

```csharp
var basicInterestTemplate = InterestTemplate.Create();
```

You can also construct this from an existing [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template) or a set of interest queries. This creates a deep copy of the [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template), to allow you to modify the queries without affecting the original set.

```csharp
var advancedInterestTemplate = InterestTemplate.Create(basicInterestTemplate);
```

## Modify a template

When modifying an [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template), you must specify which particular component's queries you are modifying. You can either:

* provide the component as a type argument
* provide the component ID as the first argument

For example, to add a query you might do one of the following:

```csharp
// Component identified by type argument
InterestTemplate.Create()
    .AddQueries<Position.Component>(query1, query2);

// Component identified by ID given as first argument
InterestTemplate.Create()
    .AddQueries(Position.ComponentId, query1, query2);
```

### Add queries

When adding queries, you can either provide the queries as parameters or an enumerable set.

```csharp
// Parameters
InterestTemplate.Create()
    .AddQueries<Position.Component>(query1, query2);

// Enumerable
var queryList = new List() { query1, query2 };
InterestTemplate.Create()
    .AddQueries<Position.Component>(queryList);
```

### Replace queries

This operation replaces all the existing queries for the given component ID with the new queries you pass in. In a similar way to adding queries, you can provide the queries as parameters or an enumerable set.

```csharp
// Parameters
InterestTemplate.Create(basicInterestTemplate)
    .ReplaceQueries<Position.Component>(query1);

// Enumerable
var queryList = new List() { query1 };
InterestTemplate.Create(basicInterestTemplate)
    .ReplaceQueries<Position.Component>(queryList);
```

> At least one query must be provided, otherwise this operation does not remove the existing queries for a given authoritative component.

### Clear queries

You can either clear queries for a given authoritative component or clear _all_ queries in the [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template).

```csharp
// Removes all queries added for the Position component
basicInterestTemplate.ClearQueries<Position.Component>();

// Removes all queries
basicInterestTemplate.ClearAllQueries();
```

## Get Interest from the template

After adding, removing or modifying a set of queries, there are two ways to get `Interest` out of the [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template).

### Snapshot

To return a [`Snapshot`]({{urlRoot}}/api/core/snapshot) of the `Interest` component that can be used when defining entity templates, call `ToSnapshot()` on the [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template).

```csharp
// Create an interest template
var interestTemplate = InterestTemplate.Create()
    .AddQueries<Position.Component>(query1, query2);

// Add the Interest component to an entity template
var entityTemplate = new EntityTemplate();
...
entityTemplate.AddComponent(interestTemplate.ToSnapshot(), WorkerUtils.UnityGameLogic);
```

### ComponentInterest

To update `Interest` at execution time, you need to modify the underlying Dictionary within the `Interest` component.

The [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template) class provides the `AsComponentInterest` method to return this Dictionary. This can be used to update the `Interest` component.

For example, to update an entity's interest with a completely new set of queries in the MonoBehaviour workflow:

```csharp
// Create the new interest template
var newInterestTemplate = InterestTemplate.Create()
    .AddQueries<Position.Component>(query1, query2);

// Require the InterestWriter and update the ComponentInterest field
InterestWriter.SendUpdate(new Interest.Update
{
    ComponentInterest = newInterestTemplate.AsComponentInterest();
});
```

<%(#Expandable title="How would I update Interest at runtime with the ECS workflow?")%>

Instead of requiring and writing to an `InterestWriter`, you need to update an `Interest.Component` object.

```csharp
// Some logic to get the Interest component
var interestComponent = someComponentGroup.InterestComponents[i];

// Create the new interest template
var newInterestTemplate = InterestTemplate.Create()
    .AddQueries<Position.Component>(query1, query2);

// Update the ComponentInterest field on the Interest component and write back the component
interestComponent.ComponentInterest = newInterestTemplate.AsComponentInterest();
someComponentGroup.InterestComponents[i] = interestComponent;
```

<%(/Expandable)%>

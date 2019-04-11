<%(TOC)%>

# How to construct an InterestQuery

<%(Callout message="
Before reading this document, make sure you have read:

  * [Introduction to query-based interest]({{urlRoot}}/modules/qbi-helper/intro-to-qbi)
  * [How to use InterestTemplate]({{urlRoot}}/modules/qbi-helper/interest-template)
")%>

An [`InterestQuery`]({{urlRoot}}/api/query-based-interest/interest-query) object contains the constraint of a query and the components that the query will return from entities that match the given constraint.

> In the future, the Runtime will also provide functionality to limit the frequency at which a query returns results.

## Create a query

To create an interest query, call `InterestQuery.Query` and provide a [`Constraint`]({{urlRoot}}/api/query-based-interest/constraint).

```csharp
var query = InterestQuery
    .Query(Constraint.RelativeSphere(20));
```

<%(#Expandable title="What constraints can I use?")%>

To learn more about the available constraints, see:

* [Available constraints]({{urlRoot}}/modules/qbi-helper/intro-to-qbi#constraints)
* [`Constraint` API reference]({{urlRoot}}/api/query-based-interest/constraint)

<%(/Expandable)%>

To limit what components the query returns, you must set a filter. If no filter is set, the query will register interest for **all** components on matching entities.

## (Optional) Set a filter

You can set a filter on which components to return in the query’s results. This is done by calling `FilterResults` and providing the component IDs of the components that you would like the query to return.

Similar to the Add, Replace and Clear functionality of the [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template) class, the component IDs can either be provided as parameters or an enumerable set.

```csharp
// Parameters
var query = InterestQuery
    .Query(constraint)
    .FilterResults(Position.ComponentId, Metadata.ComponentId);

// Enumerable
var resultComponentIds = new List() { Position.ComponentId, Metadata.ComponentId };
var query = InterestQuery
    .Query(constraint)
    .FilterResults(resultComponentIds);
```

<%(TOC)%>
# Examples

## Minimap

[Minimap](https://docs.improbable.io/reference/latest/shared/reference/query-based-interest#minimap)

```csharp
var playerConstraint = Constraint.All(
    Constraint.RelativeSphere(20),
    Constraint.Component<PlayerInfo.Component>());

var minimapConstraint = Constraint.All(
    Constraint.RelativeBox(50, double.PositiveInfinity, 50)
    Constraint.Component<MinimapRepresentation.Component>());

var playerQuery = InterestQuery
    .Query(playerConstraint)
    .FilterResults(Position.ComponentId, PlayerInfo.ComponentId);

var minimapQuery = InterestQuery
    .Query(minimapConstraint)
    .FilterResults(Position.ComponentId, MinimapRepresentation.ComponentId);

var interest = InterestTemplate.Create()
    .AddQueries<PlayerControls.Component>(playerQuery, minimapQuery);
```

## Teams

[Teams](https://docs.improbable.io/reference/latest/shared/reference/query-based-interest#teams)

```csharp
var teamComponentId;
//some logic to determine teamComponentId

var teamQuery = InterestQuery
    .Query(Constraint.Component(teamComponentId))
    .FilterResults(Position.ComponentId);

var interest = InterestTemplate.Create()
    .AddQueries<PlayerControls.Component>(teamQuery)
```

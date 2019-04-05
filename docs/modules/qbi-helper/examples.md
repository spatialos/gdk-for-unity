<%(TOC)%>
# Examples

## Minimap

Consider a simple game that displays a minimap to players, with three components:

* `PlayerControls`, authoritative on client-worker
* `PlayerInfo`, authoritative on server-worker
* `MinimapRepresentation`, authoritative on server-worker

<%(#Expandable title="See schema")%>

```
component PlayerInfo {
    id = 2000;
    int32 player_id = 1;
}

component PlayerControls {
    id = 2001;
    int32 input_value = 1;
}

component MinimapRepresentation {
    id = 2002;
    uint32 map_icon = 1;
    uint32 faction = 2;
}
```

<%(/Expandable)%>

There are two things our client-worker wants to observe:

* players within a 20m radius
* minimap objects within a 50m x 50m box

This translates to two queries:

* `Position` and `PlayerInfo` component updates for entities:
  * with a `PlayerInfo` component
  * are within a 20m radius of the client's player
* `Position` and `MinimapRepresentation` component updates for entities:
  * with a `MinimapRepresentation` component
  * are within a 50m x 50m box around the client's player

We can build up our [`Constraint`]({{urlRoot}}/api/query-based-interest/constraint) and use this to construct our [`InterestQuery`]({{urlRoot}}/api/query-based-interest/interest-query):

```csharp
var playerQuery = InterestQuery
    .Query(
        Constraint.All(
            Constraint.Component<PlayerInfo.Component>(),
            Constraint.RelativeSphere(20)))
    .FilterResults(Position.ComponentId, PlayerInfo.ComponentId);

var minimapQuery = InterestQuery
    .Query(
        Constraint.All(
            Constraint.Component<MinimapRepresentation.Component>(),
            Constraint.RelativeBox(50, double.PositiveInfinity, 50)))
    .FilterResults(Position.ComponentId, MinimapRepresentation.ComponentId);
```

We can then use the [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template) class to construct our interest. As we are specifying interest for the client-worker, we tie the queries to a component the client-worker is authoritative over, `PlayerControls`:

```csharp
var interestTemplate = InterestTemplate.Create()
    .AddQueries<PlayerControls.Component>(playerQuery, minimapQuery);
```

Lastly, we add the interest component to our player entity template.

```csharp
entityTemplate.AddComponent(interestTemplate.ToSnapshot(), WorkerUtils.UnityGameLogic);
```

The worker authoritative over the `Interest` component can make changes to the interest queries at runtime. For example, you may decide to remove the minimap query if the player disables the minimap.

## Teams

<%(#Expandable title="See schema")%>

```
component PlayerControls {
   id = 2001;
   int32 input_value = 1;
}

component RedTeam {
   id = 2004;
}

component BlueTeam {
   id = 2005;
}
```

<%(/Expandable)%>

```csharp
var teamComponentId;
//some logic to determine teamComponentId

var teamQuery = InterestQuery
    .Query(Constraint.Component(teamComponentId))
    .FilterResults(Position.ComponentId);

var interest = InterestTemplate.Create()
    .AddQueries<PlayerControls.Component>(teamQuery)
```

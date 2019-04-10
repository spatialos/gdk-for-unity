<%(TOC)%>

# Examples

## Minimap

Consider a simple game that displays a minimap to players, with three components:

* `PlayerControls`, authoritative on client-worker instances
* `PlayerInfo`, authoritative on server-worker instances
* `MinimapRepresentation`, authoritative on server-worker instances

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

* other players within a 20m radius of your own player
* minimap objects within a 50m x 50m box centered on your player

This translates to two queries:

* `Position` and `PlayerInfo` component updates for entities:
  * with a `PlayerInfo` component
  * that are within a 20m radius of the client's player
* `Position` and `MinimapRepresentation` component updates for entities:
  * with a `MinimapRepresentation` component
  * that are within a 50m x 50m box around the client's player

We build up our [`Constraint`]({{urlRoot}}/api/query-based-interest/constraint) and use this to construct our [`InterestQuery`]({{urlRoot}}/api/query-based-interest/interest-query).

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

We then use the [`InterestTemplate`]({{urlRoot}}/api/query-based-interest/interest-template) class to specify the interest. As we define the interest for the client-worker, we tie the queries to a component the client-worker is authoritative over. In this example, we choose the `PlayerControls` component.

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

This example shows how a player could observe the position of all other players on their team.

Suppose there is a Red team and a Blue team. Entities representing players are given either a `RedTeam` or `BlueTeam` component by a server-worker, to express which team they belong to.

We then consider two components:

* `PlayerControls`, authoritative on client-worker instances
* `RedTeam` or `BlueTeam`, authoritative on server-worker instances

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

Our client-worker wants to know the positions of all players with the same team component as the client-worker's player. This is represented as a query with a component constraint on either the `RedTeam` or `BlueTeam` component ID, returning just the `Position` component.

```csharp
// Some logic to determine teamComponentId
var teamComponentId = GetPlayerTeamId();

var teamQuery = InterestQuery
    .Query(Constraint.Component(teamComponentId))
    .FilterResults(Position.ComponentId);

var interest = InterestTemplate.Create()
    .AddQueries<PlayerControls.Component>(teamQuery)
```

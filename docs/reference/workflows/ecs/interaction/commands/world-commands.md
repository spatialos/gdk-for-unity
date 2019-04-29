<%(TOC)%>

# ECS: World commands

World commands are special commands that are sent to the SpatialOS runtime to ask it to reserve entity ids, create or delete entities, or request information about entities. (See the SpatialOS documentation on [world commands](https://docs.improbable.io/reference/latest/shared/design/commands#world-commands) for more information.)

World command requests can be created through the `CommandSystem`, which also handles received responses.

## 1. Reserve an entity ID

You can use the [`ReserveEntityIds`]({{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids) world command to reserve groups of entity IDs that you can use in entity creation.

Create a [`WorldCommands.ReserveEntityIds.Request`]({{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/request) and pass in the number of Entity IDs you wish to reserve. The `TimeoutMillis` field is optional.

To send the request, call the `SendCommand` method on the `CommandSystem`.

```csharp
var numIdsToReserve = 10;
var reserveEntityIdsRequest = new WorldCommands.ReserveEntityIds.Request(numIdsToReserve);
commandSystem.SendCommand(reserveEntityIdsRequest);
```

You can get responses by calling `GetResponses` on the `CommandSystem` with the [`WorldCommands.ReserveEntityIds.ReceivedResponse`]({{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/received-response) type parameter.

```csharp
commandSystem.GetResponses<WorldCommands.ReserveEntityIds.ReceivedResponse>();
```

## 2. Create an entity

The [`CreateEntity`]({{urlRoot}}/api/core/commands/world-commands/create-entity) world command is used to request the creation of a new SpatialOS entity, specified using an [entity template]({{urlRoot}}/reference/concepts/entity-templates).

You must construct a [`WorldCommands.CreateEntity.Request`]({{urlRoot}}/api/core/commands/world-commands/create-entity/request) struct and provide an entity template. The `EntityId` and `TimeoutMillis` fields are optional. If you do specify an `EntityId`, you need to get this from a [`ReserveEntityIds`]({{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids) command.

To send the request, call the `SendCommand` method on the `CommandSystem`.

```csharp
var entity = Creatures.CreatureEntityTemplate(new Coordinates(0, 0, 0));
var createEntityRequest = new WorldCommands.CreateEntity.Request(entity);
commandSystem.SendCommand(createEntityRequest);
```

You can get responses by calling `GetResponses` on the `CommandSystem` with the [`WorldCommands.CreateEntity.ReceivedResponse`]({{urlRoot}}/api/core/commands/world-commands/create-entity/received-response) type parameter.

```csharp
commandSystem.GetResponses<WorldCommands.CreateEntity.ReceivedResponse>();
```

## 3. Delete an entity

You can delete entities via the [`DeleteEntity`]({{urlRoot}}/api/core/commands/world-commands/delete-entity) world command. You need to know the SpatialOS entity ID of the entity you want to delete.

Create a [`WorldCommands.DeleteEntity.Request`]({{urlRoot}}/api/core/commands/world-commands/delete-entity/request) struct with the EntityId of the entity you want to delete. The `TimeoutMillis` field is optional.

To send the request, call the `SendCommand` method on the `CommandSystem`.

```csharp
var deleteEntityRequest = new WorldCommands.DeleteEntity.Request(targetEntityId);
commandSystem.SendCommand(deleteEntityRequest);
```

You can get responses by calling `GetResponses` on the `CommandSystem` with the [`WorldCommands.DeleteEntity.ReceivedResponse`]({{urlRoot}}/api/core/commands/world-commands/delete-entity/received-response) type parameter.

```csharp
commandSystem.GetResponses<WorldCommands.CreateEntity.ReceivedResponse>();
```

## 4. Entity query

You can use entity queries to get information about entities in the world. For more information, see [entity queries](https://docs.improbable.io/reference/latest/shared/glossary#queries) in the SpatialOS documentation.

Create a [`WorldCommands.EntityQuery.Request`]({{urlRoot}}/api/core/commands/world-commands/entity-query/request) struct with the [`EntityQuery`]({{urlRoot}}/api/core/commands/world-commands/entity-query#entityquery-class). The `TimeoutMillis` field is optional.

To send the request, call the `SendCommand` method on the `CommandSystem`.

```csharp
var playerCreatorQuery = new EntityQuery
{
    Constraint = new ComponentConstraint(PlayerCreator.ComponentId),
    ResultType = new SnapshotResultType()
};
var entityQueryRequest = new WorldCommands.EntityQuery.Request(playerCreatorQuery);
commandSystem.SendCommand(entityQueryRequest);
```

You can get responses by calling `GetResponses` on the `CommandSystem` with the [`WorldCommands.EntityQuery.ReceivedResponse`]({{urlRoot}}/api/core/commands/world-commands/entity-query/received-response) type parameter.

```csharp
commandSystem.GetResponses<WorldCommands.EntityQuery.ReceivedResponse>();
```

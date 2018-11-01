# World commands
 _This document relates to the [ECS workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

World commands are special commands that are sent to the SpatialOS runtime to ask it to reserve entity ids, create or delete entities, or request information about entities. (See the SpatialOS documentation on [world commands](https://docs.improbable.io/reference/latest/shared/design/commands#world-commands) for more information.)

Each ECS entity that represents a SpatialOS entity has a set of components for sending world commands. For each world command, there is a component to send the command and receive the response.

## 1. Reserve an entity ID

You can use the `ReserveEntityIds` world command to reserve groups of entity IDs that you can use in entity creation.

To send a request use a `WorldCommands.ReserveEntityIds.CommandSender` component. This contains a list of `WorldCommands.ReserveEntityIds.Request` structs. Add a struct to the list to send the command.

- `TimeoutMillis` is optional.

To receive a response use `WorldCommands.ReserveEntityIds.CommandResponses`. This contains a list of `WorldCommands.ReserveEntityIds.ReceivedResponse` structs.

## 2. Create an entity

You can use the `CreateEntity` world command to request the creation of a new SpatialOS entity which you specified using an [entity template]({{urlRoot}}/content/entity-templates).

To send a request use a `WorldCommands.CreateEntity.CommandSender` component. This contains a list of `WorldCommands.CreateEntity.Request` structs. Add a struct to the list to send the command.

- `EntityId` and `TimeoutMillis` are optional.
- If you do specify an `EntityId`, you need to get this from a `ReserveEntityIds` command.

To receive a response use `WorldCommands.CreateEntity.CommandResponses`. This contains a list of `WorldCommands.CreateEntity.ReceivedResponse`.

Below is an example of creating a SpatialOS entity. For more information on how to create a `CreatureTemplate`, see the [creating entity templates]({{urlRoot}}/content/entity-templates) page.

```csharp
public class CreateCreatureSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<Foo> Foo;
        public ComponentDataArray<WorldCommands.CreateEntity.CommandSender> CreateEntitySender;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for (var i = 0; i < data.Length; i++)
        {
            var requestSender = data.CreateEntitySender[i];
            var entity = CreatureTemplate.CreateCreatureEntityTemplate(
                new Coordinates(0, 0, 0));

            requestSender.RequestsToSend.Add(WorldCommands.CreateEntity.CreateRequest
            (
                entity
            ));
            data.CreateEntitySender[i] = requestSender;
        }
    }
}
```

This system iterates through every entity with a `Foo` component and sends a create entity request.

## 3. Delete an entity

You can delete entities via the `DeleteEntity` world command. You need to know the SpatialOS entity ID of the entity you want to delete.

To send a request use a `WorldCommands.DeleteEntity.CommandSender` component. This contains a list of `WorldCommands.DeleteEntity.Request` structs. Add a struct to the list to send the command.

- `TimeoutMillis` is optional.

To receive a response use `WorldCommands.DeleteEntity.CommandResponses`. This contains a list of `WorldCommands.DeleteEntity.ReceivedResponse`.

```csharp
public class DeleteCreatureSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<Bar> Bar;
        [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
        public ComponentDataArray<WorldCommands.DeleteEntity.CommandSender> DeleteEntitySender;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for (var i = 0; i < data.Length; i++)
        {
            var requestSender = data.DeleteEntitySender[i];
            var entityId = data.SpatialEntityIds[i].EntityId;

            requestSender.RequestsToSend.Add(WorldCommands.DeleteEntity.CreateRequest
            (
                entityId
            ));
            data.DeleteEntitySender[i] = requestSender;
        }
    }
}
```

This system iterates through every entity with a `Bar` and a SpatialEntityId component and sends a delete entity request.

## 4. Entity query

You can use entity queries to get information about entities in the world.

To send a request use a `WorldCommands.EntityQuery.CommandSender` component. This contains a list of `WorldCommands.EntityQuery.Request` structs. Add a struct to the list to send the command.

  * For more information, see [entity queries](https://docs.improbable.io/reference/latest/shared/glossary#queries) in the SpatialOS documentation.
  * `TimeoutMillis` is optional.

To receive a response use `WorldCommands.EntityQuery.CommandResponses`. This contains a list of `WorldCommands.EntityQuery.ReceivedResponse`.

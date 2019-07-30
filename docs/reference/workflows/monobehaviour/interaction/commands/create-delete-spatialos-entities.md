<%(TOC)%>

# How to create and delete entities

_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/reference/workflows/overview)._

Before reading this document, make sure you are familiar with:

  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
  * [Read and write access]({{urlRoot}}/reference/glossary#authority)
  * [(MonoBehaviour) World commands]({{urlRoot}}/reference/workflows/monobehaviour/interaction/commands/world-commands)
  * [(MonoBehaviour) How to interact with SpatialOS using MonoBehaviours]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/lifecycle)
  * [SpatialOS entities: Creating entity templates]({{urlRoot}}/reference/concepts/entity-templates)


To see the exact API for using world commands inside a MonoBehaviour, take a look at the [World command request sender and receiver API]({{urlRoot}}/reference/workflows/monobehaviour/interaction/commands/world-commands)

### How to create a SpatialOS entity

To create an entity, you

  * [define an entity template]({{urlRoot}}/reference/concepts/entity-templates)
  * [send the `CreateEntity` world command]({{urlRoot}}/reference/workflows/monobehaviour/interaction/commands/world-commands#create-an-entity).

> When you create an entity, the SpatialOS GDK for Unity by default does not associate a GameObject with it. For more information on how to enable this, see [the GameObject Creation Feature Module]({{urlRoot}}/modules/game-object-creation/overview) documentation.

The following code snippet shows an example of how to create an entity inside a MonoBehaviour. This example MonoBehaviour would be enabled on any worker containing the corresponding GameObject.

```csharp
public class EntityCreationBehaviour : MonoBehaviour
{
    [Require] private WorldCommandSender commandSender;

    private EntityTemplate CreateExampleEntityTemplate()
    {
        var template = new EntityTemplate();
        // add components and set read access
        return template;
    }

    public void CreateExampleEntity()
    {
        var exampleEntity = CreateExampleEntityTemplate();
        var request = new WorldCommands.CreateEntity.Request(exampleEntity);
        commandSender.SendCreateEntityCommand(request, OnCreateEntityResponse);
    }

    private void OnCreateEntityResponse(WorldCommands.CreateEntity.ReceivedResponse response)
    {
        if (response.StatusCode == StatusCode.Success)
        {
            var createdEntityId = response.EntityId.Value;
            // handle success
        }
        else
        {
            // handle failure
        }
    }
}
```
## How to create multiple entities

There are two ways to create multiple entities. You can:

* create entities one by one without reserving an entity ID
* reserve multiple entity IDs first and then send a `CreateEntity` request for each of the received entity IDs.

#### Creating multiple entities through bulk reservation

The following code snippet shows an example of how to reserve and create multiple entities inside a MonoBehaviour. The example MonoBehaviour would be enabled on any worker containing the corresponding GameObject.

```csharp
public class MultipleEntityCreationBehaviour : MonoBehaviour
{
    [Require] private WorldCommandSender commandSender;

    public void CreateManyExampleEntities(uint numberOfEntitiesToCreate)
    {
        // Send reserve entities command request.
        // The response will be handled in the OnReserveEntityResponse method below.
        var request = new WorldCommands.ReserveEntityIds.Request(numberOfEntitiesToCreate);
        commandSender.SendReserveEntityIdsCommand(request, OnReserveEntityIdsResponse);
    }

    private void OnReserveEntityIdsResponse(WorldCommands.ReserveEntityIds.ReceivedResponse response)
    {
        if (response.StatusCode == StatusCode.Success)
        {
            var firstEntityId = response.FirstEntityId.Value;
            var numberOfReservedEntityIds = response.NumberOfEntityIds;

            CreateReservedEntities(firstEntityId, numberOfReservedEntityIds);
        }
        else
        {
            // Handle failure.
        }
    }

    private EntityTemplate CreateExampleEntityTemplate()
    {
        var template = new EntityTemplate();
        // add components and set read access
        return template;
    }

    private void CreateReservedEntities(EntityId firstEntityId, int numberOfReservedEntityIds)
    {
        var entityTemplate = CreateExampleEntityTemplate();

        for (var i = 0; i < numberOfReservedEntityIds; ++i)
        {
            var request = new WorldCommands.CreateEntity.Request(entityTemplate);
            commandSender.SendCreateEntityCommand(request, OnCreateEntityResponse);
        }
    }

    private void OnCreateEntityResponse(WorldCommands.CreateEntity.ReceivedResponse response)
    {
        if (response.StatusCode == StatusCode.Success)
        {
            var createdEntityId = response.EntityId.Value;
            // Handle success.
        }
        else
        {
            // Handle failure.
        }
    }
}
```
## How to delete a SpatialOS entity

To delete an entity, you

  * identify the SpatialOS entity ID of the entity that you want to delete
  * [send the `DeleteEntity` world command]({{urlRoot}}/reference/workflows/monobehaviour/interaction/commands/world-commands#delete-an-entity).

> Do not delete the linked GameObjects unless you are writing a [custom `IEntityGameObjectCreator` implementation]({{urlRoot}}/modules/game-object-creation/custom-usage).

#### Example of deleting an entity

The following code snippet shows an example of how to delete an entity inside a MonoBehaviour. This example MonoBehaviour would be enabled on any worker containing the corresponding GameObject.

```csharp
public class EntityDeletionBehaviour : MonoBehaviour
{
    [Require] private WorldCommandSender commandSender;

    public void Update()
    {
        if (WantToDeleteAnEntity())
        {
            var entityId = GetSomeEntityId();
            var request = new WorldCommands.DeleteEntity.Request(entityId);
            commandSender.SendDeleteEntityCommand(request, OnDeleteEntityResponse);
        }
    }

    private void OnDeleteEntityResponse(WorldCommands.DeleteEntity.ReceivedResponse response)
    {
        if (response.StatusCode == StatusCode.Success)
        {
            // handle success
        }
        else
        {
            // handle failure
        }
    }
}
```

#### Getting SpatialOS entity IDs to use for deletion

You can get the `EntityId` for an entity using these methods:

* To get the `EntityId` for the current GameObject, use `[Require] private EntityId entityId`.
* To get the `EntityId` for a different GameObject, use `otherGameObject.GetComponent<LinkedEntityComponent>().EntityId`.

[//]: # (Doc of docs reference 26)
[//]: # (TODO - technical author pass)

# (GameObject-MonoBehaviour) SpatialOS entities: How to create and delete entities
_This document relates to the [GameObject-MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spos-entities)._


Before reading this document, make sure you are familiar with:

  * [Workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk)
  * [Read and write access]({{urlRoot}}/content/glossary#authority)
  * [(GameObject-MonoBehaviour) World commands]({{urlRoot}}/content/gameobject/gomb-world-commands)
  * [(GameObject-MonoBehaviour) How to interact with SpatialOS using MonoBehaviours]({{urlRoot}}/content/gameobject/interact-spos-monobehaviours)
  * [World and component command requests and responses]({{urlRoot}}/content/world-component-commands-requests-responses)
  * [SpatialOS entities: Creating entity templates]({{urlRoot}}/content/entity-templates)


To see the exact API for using world commands inside a MonoBehaviour, take a look at the [World command request sender and receiver API](https://docs.google.com/document/d/1SRKwGHvoqoi3SwOZHFJ8e-gT_zM5AVe85Br4J-l4u_w/edit#)
### How to create a SpatialOS entity
To create an entity, you

  * [define the entity's template]({{urlRoot}}/content/entity-templates.md)
  * [send the `CreateEntity` world command]({{urlRoot}}/content/gameobject/gomb-world-commands#createentity).

> When you create an entity, the SpatialOS GDK for Unity by default does not associate a GameObject with it.  For more information on how to enable this, see [representing entities with gameobjects](https://docs.google.com/document/d/1OWSlvKSg9jNDFIgJ6Z3SyLGvh75znZa95SwSyG4_A14/edit#).


The following code snippet shows an example of how to create an entity inside a MonoBehaviour. This example MonoBehaviour would be enabled on any worker containing the corresponding GameObject.
```csharp
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker.Core;
using UnityEngine;

public class EntityCreationBehaviour : MonoBehaviour
{
    [Require] private WorldCommands.Requirable.WorldCommandRequestSender commandSender;
    [Require] private WorldCommands.Requirable.WorldCommandResponseHandler responseHandler;

    void OnEnable()
    {
        // Register callback for listening to any incoming create entity command responses for this entity
        responseHandler.OnCreateEntityResponse += OnCreateEntityResponse;
    }

    public void CreateExampleEntity()
    {
        var myEntityTemplate = EntityBuilder.Begin()
            .AddPosition(0, 0, 0, "UnityGameLogic")
            .AddMetadata("MyPrefab", "UnityGameLogic")
            .SetPersistence(false)
            .SetReadAcl("UnityGameLogic", "UnityClient")
            .AddComponent(ExampleComponent.Component.CreateSchemaComponentData(), "UnityGameLogic")
            .Build();


        // send create entity command request without reserving an entity id
        // The SpatialOS Runtime will automatically assign a SpatialOS entity id to the newly created entity
        commandSender.CreateEntity(myEntityTemplate);
    }

    void OnCreateEntityResponse(WorldCommands.CreateEntity.ReceivedResponse response)
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
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;

public class MultipleEntityCreationBehaviour : MonoBehaviour
{
    [Require] private WorldCommands.Requirable.WorldCommandRequestSender commandSender;
    [Require] private WorldCommands.Requirable.WorldCommandResponseHandler responseHandler;

    void OnEnable()
    {
        // Register callback for listening to any incoming reserve entity command responses for this entity.
        responseHandler.OnReserveEntityIdsResponse += OnReserveEntityIdsResponse;
        // Register callback for listening to any incoming create entity command responses for this entity.
        responseHandler.OnCreateEntityResponse += OnCreateEntityResponse;
    }

    public void CreateManyExampleEntities(uint numberOfEntitiesToCreate)
    {
        // Send reserve entities command request.
        // The response will be handled in the OnReserveEntityResponse method below.
        commandSender.ReserveEntityIds(numberOfEntitiesToCreate);
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

    public void CreateReservedEntities(EntityId firstEntityId, int numberOfReservedEntityIds)
    {
        for (var i = 0; i < numberOfReservedEntityIds; ++i)
        {
            var entityIdToCreate = new EntityId(firstEntityId.Id + numberOfReservedEntityIds);

            // Send create entity command request using the reserved entity id.
            // The SpatialOS Runtime will automatically assign a SpatialOS entity id to the newly created entity.
            var myEntityTemplate = EntityBuilder.Begin()
                .AddPosition(0, 0, 0, "UnityGameLogic")
                .AddMetadata("MyPrefab", "UnityGameLogic")
                .SetPersistence(false)
                .SetReadAcl("UnityGameLogic", "UnityClient")
                .AddComponent(ExampleComponent.Component.CreateSchemaComponentData(), "UnityGameLogic")
                .Build();

            commandSender.CreateEntity(myEntityTemplate, entityIdToCreate);
        }
    }

    void OnCreateEntityResponse(WorldCommands.CreateEntity.ReceivedResponse response)
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

  * identify the SpatialOS entity id of the entity that you want to delete
  * [send the `DeleteEntity` world command]({{urlRoot}}/content/gameobject/gomb-world-commands#deleteentity).

> Do not delete the linked GameObjects. The GDK handles deleting the linked GameObjects, f you used the [GameObject creation feature module](https://docs.google.com/document/d/1OWSlvKSg9jNDFIgJ6Z3SyLGvh75znZa95SwSyG4_A14/edit#) to link your SpatialOS entities to GameObjects. Deleting GameObjects locally will break many things badly.

#### Example of deleting an entity
The following code snippet shows an example of how to delete an entity inside a MonoBehaviour. This example MonoBehaviour would be enabled on any worker containing the corresponding GameObject.
```csharp
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker.Core;
using UnityEngine;

public class EntityDeletionBehaviour : MonoBehaviour
{
    [Require] WorldCommands.Requirable.WorldCommandRequestSender commandSender;
    [Require] WorldCommands.Requirable.WorldCommandResponseHandler responseHandler;

    private void OnEnable()
    {
        // Register callback for listening to any incoming delete entity command responses for this entity
        responseHandler.OnDeleteEntityResponse += OnDeleteEntityResponse;
    }

    public void Update()
    {
        if(WantToDeleteAnEntity())
        {
            var entityId = GetSomeEntityId();
            commandSender.DeleteEntity(entityId);
        }
    }

    private void DeleteEntity(EntityId entityId)
    {
        if (commandSender != null)
        {
            commandSender.DeleteEntity(entityId);
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

* To get the `EntityId` for the current GameObject, use `GetComponent<[SpatialOSComponent](link to spatialoscomp docs)>().SpatialEntityId`
* To get the `EntityId` for a different GameObject, use `SpatialOSComponent.TryGetSpatialOSEntityIdForGameObject(GameObject linkedGameObject, out EntityId entityId)`

> Using `SpatialOSComponent.TryGetSpatialOSEntityIdForGameObject` to access the SpatialOS entity id of another linked GameObject ensures that you receive the correct SpatialOS entity ID.

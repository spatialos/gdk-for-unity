<%(TOC)%>
# Customising player creation

<%(Callout message="
Before reading this document, make sure you are familiar with:

  * [Basic player spawning]({{urlRoot}}/modules/player-lifecycle/basic-player-creation)
  * [Creating entity templates]({{urlRoot}}/reference/concepts/entity-templates)
  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

## How to manually send player creation requests

By default, the Player Lifecycle Feature Module sends a player creation request as soon as the client-worker instance connects to SpatialOS.

To modify this behaviour, you must set `PlayerLifecycleConfig.AutoRequestPlayerCreation` to false by one of the following ways:

* Add an extra `false` argument when adding client systems: `PlayerLifecycleHelper.AddClientSystems(Worker.World, autoRequestPlayerCreation: false);`
* Set the field to false during set-up: `PlayerLifecycleConfig.AutoRequestPlayerCreation = false;`

To spawn the player after disabling the default behaviour, you need to call the `RequestPlayerCreation` method. This method can be found in the `SendCreatePlayerRequestSystem`.

```csharp
var playerCreationSystem = World.GetExistingManager<SendCreatePlayerRequestSystem>();
playerCreationSystem.RequestPlayerCreation();
```

To find out more about the `RequestPlayerCreation` method, take a look at the `SendCreatePlayerRequestSystem` [API reference documentation]({{urlRoot}}/api/player-lifecycle/send-create-player-request-system#methods).

## Adding a player creation callback

If manual player creation is enabled, you can configure a callback to be triggered when a player creation response is received. The callback must be of type `Action<PlayerCreator.CreatePlayer.ReceivedResponse>`.

For example:

```csharp
public void CallPlayerCreation()
{
    var playerCreationSystem = World.GetExistingManager<SendCreatePlayerRequestSystem>();
    playerCreationSystem.RequestPlayerCreation(callback: OnCreatePlayerResponse);
}

private void OnCreatePlayerResponse(PlayerCreator.CreatePlayer.ReceivedResponse response)
{
    if (response.StatusCode != StatusCode.Success)
    {
        Debug.LogWarning($"Error: {response.Message}");
    }
}
```

## Using arbitrary data in the player creation loop

The default player creation loop sends requests automatically. To use arbitrary data in the loop, you must ensure that `AutoRequestPlayerCreation` is set to false.

1. Disable `AutoRequestPlayerCreation` as described in the [step above](#manually-sending-player-creation-requests).
1. Call `RequestPlayerCreation` with serialized data.
2. Deserialize data back into the original type.

##### Call RequestPlayerCreation with serialized data

Get the `SendCreatePlayerRequestSystem`, and invoke the `RequestPlayerCreation` method. Ensure that your arbitrary data is serialized into a byte array.

```csharp
var myArguments = new SampleArgumentsObject
{
  PlayerName = "playerName",
  SpawnPosition = new Coordinates(50, 0, 75))
};
var playerCreationSystem = World.GetExistingManager<SendCreatePlayerRequestSystem>();
var serializedArguments = SerializeArguments(myArguments);
playerCreationSystem.RequestPlayerCreation(serializedArguments);
```

<%(#Expandable title="How do I serialize my data?")%>

There are numerous ways to serialize data. An example implementation could be:

```csharp
private byte[] SerializeArguments(object playerCreationArguments)
{
    using (var memoryStream = new MemoryStream())
    {
        var binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(memoryStream, playerCreationArguments);
        return memoryStream.ToArray();
    }
}
```

<%(/Expandable)%>

##### Deserialize data into the same type you originally serialized from

The data serialized in the previous step is sent across to the server-worker as part of the player creation request. This data is passed in as the `playerCreationArguments` parameter of the entity template delegate that is invoked.

Ensure that you are deserializing the byte array into the same type of object you serialized it from. For example:

```csharp
public static EntityTemplate CreatePlayerEntityTemplate(string workerId, byte[] playerCreationArguments)
{
    // Obtain unique client attribute of the client-worker that requested the player entity
    var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(workerId);
    // Obtain the attribute of your server-worker
    var serverAttribute = "UnityGameLogic";

    var deserializedArguments = DeserializeArguments<SampleArgumentsObject>(playerCreationArguments);

    var entityTemplate = new EntityTemplate();
    entityTemplate.AddPosition(new Position.Snapshot(deserializedArguments.SpawnPosition), serverAttribute);
    entityTemplate.AddComponent(new PlayerName.Snapshot(deserializedArguments.PlayerName), serverAttribute);
    // add all components that you want the player entity to have
    AddPlayerLifecycleComponents(entityTemplate, workerId, serverAttribute);

    return entityTemplate;
}
```

<%(#Expandable title="How do I deserialize my data?")%>

Like serialization, there are many ways to deserialize data. Given the serialization example earlier, a sample deserialization implementation could be:

```csharp
private T DeserializeArguments<T>(byte[] serializedArguments)
{
    using (var memoryStream = new MemoryStream())
    {
        var binaryFormatter = new BinaryFormatter();
        memoryStream.Write(serializedArguments, 0, serializedArguments.Length);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return (T) binaryFormatter.Deserialize(memoryStream);
    }
}
```

<%(/Expandable)%>

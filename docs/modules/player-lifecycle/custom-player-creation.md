<%(TOC)%>
# Customising player creation

<%(Callout message="
Before reading this document, make sure you are familiar with:

  * [Basic player spawning]({{urlRoot}}/modules/player-lifecycle/basic-player-creation)
  * [Creating entity templates]({{urlRoot}}/reference/concepts/entity-templates)
  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

## Manually sending player creation requests

By default, the Player Lifecycle Feature Module automatically sends a player creation request is sent as soon as the client-worker instance connects to SpatialOS.

To modify this behaviour, you must set `PlayerLifecycleConfig.AutoRequestPlayerCreation` to false by one of the following ways:

* Add an extra `false` argument when adding client systems: `PlayerLifecycleHelper.AddClientSystems(Worker.World, autoRequestPlayerCreation: false);`
* Set the field to false manually during set-up: `PlayerLifecycleConfig.AutoRequestPlayerCreation = false;`
* Directly modify `PlayerLifecycleConfig` to be false by default

Once `AutoRequestPlayerCreation` is set to false you must manually call the `RequestPlayerCreation` method, which can be found in the `SendCreatePlayerRequestSystem`.

```csharp
var playerCreationSystem = World.GetExistingManager<SendCreatePlayerRequestSystem>();
playerCreationSystem.RequestPlayerCreation();
```

To find out more about the `RequestPlayerCreation` method, take a look at the `SendCreatePlayerRequestSystem` [API reference documentation]({{urlRoot}}/api/player-lifecycle/send-create-player-request-system#methods).

## Adding a player creation callback

If manual player creation is enabled, you can configure a callback to be triggered when a player creation response is received. The callback must be of type `Action<PlayerCreator.CreatePlayer.ReceivedResponse>`.

For example:

```csharp
void CallPlayerCreation()
{
    var playerCreationSystem = World.GetExistingManager<SendCreatePlayerRequestSystem>();
    playerCreationSystem.RequestPlayerCreation(callback: OnCreatePlayerResponse);
}

void OnCreatePlayerResponse(PlayerCreator.CreatePlayer.ReceivedResponse response)
{
    if (response.StatusCode != StatusCode.Success)
    {
        Debug.LogWarning($"Error: {response.Message}");
    }
}
```

## Using arbitrary data in the player creation loop

The default player creation loop sends requests automatically. To use arbitrary data in the loop, you must ensure that `AutoRequestPlayerCreation` is set to false. Some

1. Disable `AutoRequestPlayerCreation`, some ways of doing this are described [here](#manually-sending-player-creation-requests).
1. Call `RequestPlayerCreation` with serialized data.
2. Deserialize data back into the original type.

##### Call RequestPlayerCreation with serialized data

Then, call `RequestPlayerCreation`. Ensure that your arbitrary data is serialized into a byte array.

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

Where an example implementation of `SerializeArguments` could be:

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

##### Deserialize data into the same type you originally serialized from

Lastly, ensure that you are deserializing the byte array into the same type of object you serialized it from. For example:

```csharp
public static class PlayerTemplate
{
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

    // An example implementation of DeserializeArguments
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
}
```

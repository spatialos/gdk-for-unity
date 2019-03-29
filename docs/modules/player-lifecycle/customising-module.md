<%(TOC)%>
# Customising the Player Lifecycle

<%(Callout message="
Before reading this document, make sure you are familiar with:

  * [Basic player spawning]({{urlRoot}}/modules/player-lifecycle/basic-player-spawning)
  * [Creating entity templates]({{urlRoot}}/reference/concepts/entity-templates)
  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

## How to use arbitrary data in the player creation loop

##### 1. Set AutoRequestPlayerCreation to false

First, ensure that `AutoRequestPlayerCreation` is set to false.

##### 2. Call RequestPlayerCreation with serialized data

Then, call `RequestPlayerCreation`. Ensure that your arbitrary data is serialized into a byte array.

```csharp
var myArguments = new SampleArgumentsObject { PlayerName = "playerName", SpawnPosition = new Coordinates(50, 0, 75)) };
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

##### 3. Deserialize data into the same type you originally serialized from

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

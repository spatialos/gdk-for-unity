<%(TOC)%>

# Reconnection logic

Mobile applications present new challenges in terms of handling client connections. A client may disconnect due to unstable connections or when pausing the application. We want to dive a bit deeper into the causes of a disconnect and how you can handle it in your game.

## Why clients disconnect

### Unstable connection / cellular reception

When creating a mobile application, you need to ensure that clients are able to play the game even with an unstable connection.

If a client's connection is too unstable, they might not be able to connect to your game or might disconnect at any time. You should provide a connection flow that allows them to disconnect properly, and handle reconnection as soon as they obtain better reception.

### Pausing of applications

When pausing an application or putting it into the background, there are two possible scenarios that can cause a disconnect:

#### 1. The device OS closes the application.

If the application has been closed, the user has to restart the application. The user will be brought back to the start screen the next time the application is opened and from there on, the application can connect and load any data necessary.

#### 2. The game stops sending data. If this happens for too long, SpatialOS will close the connection.

This scenario is a bit more tricky. The application is still alive, however the next time the user opens the application, SpatialOS will have already closed the connection. You need to add additional reconnection logic to your game to be able to handle this scenario. 

In the end this is a very game-specific question that depends entirely on what you want to offer to your users. The FPS Starter Project implements the simplest solution: a disconnect takes you back to the start screen, from where you can reconnect to continue the game.

## How to detect a disconnect

[Heartbeating]({{urlRoot}}/modules/player-lifecycle/heartbeating) is a technique used to verify that your client is still connected. If there are too many failed heartbeats, there are two ways your client may be seen as disconnected:

### 1. SpatialOS believes you have disconnected

If SpatialOS disconnects you, you receive a `DisconnectOp`. This object contains the reason behind the disconnection and triggers a disconnect event in the GDK.

You can listen for this event using the `Worker` object inside the `WorkerConnector` class in order to perform any kind of disconnection logic necessary.

```csharp
namespace YourGame
{
    public class MobileClientWorkerConnector : WorkerConnector
    {
    	...

        protected override void HandleWorkerConnectionEstablished()
        {
             Worker.OnDisconnect += OnDisconnected;
        }

        private void OnDisconnected(string reason)
        {
            Worker.LogDispatcher.HandleLog(LogType.Log, new LogEvent($"Worker disconnected")
                .WithField("WorkerId", Worker.WorkerId)
                .WithField("Reason", reason));
        }
    }
}
```

### 2. The server-worker believes you have disconnected

If SpatialOS believes you are still connected and your server-worker thinks you are disconnected, you won’t receive a `DisconnectOp`.

In multiplayer games, you often want most of the logic to be server-authoritative to prevent cheating. This means that your client is only authoritative over some components on exactly one entity: the player entity

If your server-worker disconnects you and you use our player lifecycle module, the server-worker will destroy your player entity.

At this point your client is still connected to SpatialOS, but without any entities to be authoritative over. If your client is not authoritative over any entity, it will not be able to have any entities in its view and the client's world will appear to be empty.

This needs to be handled by either: 

1. Requesting a new player entity.
1. Reconnecting the client-worker.
